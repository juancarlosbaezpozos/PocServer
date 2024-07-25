using Principal.Server.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Principal.Server
{
    public abstract class StiProcessor : IStiProcessor
    {
        private Thread thread;

        private readonly DispatcherTimer workTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(2.0)
        };

        private string workDescription;

        private bool IsWorkProcessing { get; set; }

        protected internal bool StopRequired { get; set; }

        public int? Index { get; }

        protected ManualResetEvent PauseHandle { get; }

        public IStiCore Core { get; set; }

        public StiProcessorStatus ProcessorStatus { get; set; } = StiProcessorStatus.NotInitialized;

        protected abstract TimeSpan SuspendTime { get; }

        public abstract string Name { get; }

        public string ProcessorName => Index.HasValue ? $"{Name}-{Index}" : Name;

        protected void BeginTask(string task, params object[] args)
        {
            workTimer.Stop();
            workDescription = string.Format(task, args);
            IsWorkProcessing = true;
        }

        protected void EndTask()
        {
            workTimer.Start();
        }

        public string GetTask()
        {
            return !IsWorkProcessing ? null : workDescription;
        }

        public async Task StartAsync()
        {
            await Task.Run(Start);
        }

        public virtual void Start()
        {
            if (ProcessorStatus != StiProcessorStatus.Started)
            {
                PauseHandle.Set();
                thread = new Thread(ThreadMethod)
                {
                    Name = ProcessorName
                };
                thread.Start();
                ProcessorStatus = StiProcessorStatus.Started;
            }
        }

        public async Task PauseAsync()
        {
            await Task.Run(Pause);
        }

        public virtual void Pause()
        {
            if (ProcessorStatus != StiProcessorStatus.Paused)
            {
                PauseHandle.Reset();
                ProcessorStatus = StiProcessorStatus.Paused;
            }
        }

        public async Task ContinueAsync()
        {
            await Task.Run(Continue);
        }

        public virtual void Continue()
        {
            if (ProcessorStatus != StiProcessorStatus.Started)
            {
                PauseHandle.Set();
                ProcessorStatus = StiProcessorStatus.Started;
            }
        }

        public async Task StopAsync()
        {
            await Task.Run(Stop);
        }

        public virtual void Stop()
        {
            if (ProcessorStatus != StiProcessorStatus.Stopped)
            {
                PauseHandle.Set();
                if (thread != null)
                {
                    StopRequired = true;
                }
            }
        }

        public async Task BreakAsync()
        {
            await Task.Run(Break);
        }

        public virtual void Break()
        {
            if (ProcessorStatus != StiProcessorStatus.Stopped)
            {
                PauseHandle.Set();
                if (thread != null)
                {
                    thread.Abort();
                    thread.Join(TimeSpan.FromSeconds(4.0));
                    thread = null;
                }
                ProcessorStatus = StiProcessorStatus.Stopped;
            }
        }

        public StiProcessorInfo GetProcessorInfo()
        {
            return new StiProcessorInfo
            {
                Name = Name,
                ProcessorName = ProcessorName,
                ProcessorStatus = ProcessorStatus,
                CurrentTask = GetTask()
            };
        }

        public override string ToString()
        {
            return ProcessorName;
        }

        protected virtual void ThreadMethod()
        {
            try
            {
                while (true)
                {
                    PauseHandle.WaitOne();
                    if (StopRequired)
                    {
                        break;
                    }

                    Process();
                    ThreadSleep();
                }
                StopRequired = false;
                ProcessorStatus = StiProcessorStatus.Stopped;
            }
            catch (ThreadAbortException)
            {
                thread?.Abort();
            }
        }

        protected virtual void ThreadSleep()
        {
            Thread.Sleep(SuspendTime);
        }

        protected abstract void Process();

        protected StiProcessor(IStiCore core, int? processorIndex)
        {
            Index = processorIndex;
            Core = core;
            PauseHandle = new ManualResetEvent(initialState: false);
            workTimer.Tick += delegate
            {
                workTimer.Stop();
                IsWorkProcessing = false;
            };
        }
    }
}
