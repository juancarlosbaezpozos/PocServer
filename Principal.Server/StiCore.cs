using Principal.Server.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Principal.Server
{
    public class StiCore : IStiCore
    {
        public static StiCore Instance { get; private set; }

        public string SessionKey { get; set; }

        public List<IStiProcessor> Processors { get; set; }

        public StiCore()
        {
            LoadConfig();
            Instance = this;
            InitializeCore();
        }

        private void InitializeCore()
        {
            Processors = new List<IStiProcessor>();
            SessionKey = Guid.NewGuid().ToString().Replace("-", string.Empty);
        }

        private void LoadConfig()
        {
            //TODO: Load config
        }

        public virtual void Start(bool runProcessors = true)
        {
            LoadConfig();
            CreateProcessor(1, (int? index) => new EjemploProcessor(this, index));

            if (runProcessors)
            {
                Processors.ForEach(delegate (IStiProcessor p)
                {
                    p.Start();
                });
            }
        }

        public void Pause()
        {
            try
            {
                Processors.ForEach(delegate (IStiProcessor p)
                {
                    p.Pause();
                });
                //StiAppDomainWorker.ReleaseWorkers();
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception.Message);
            }
        }

        public void Resume()
        {
            try
            {
                Processors.ForEach(delegate (IStiProcessor p)
                {
                    p.Continue();
                });
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception.Message);
            }
        }

        public virtual void Stop()
        {
            try
            {
                LoadConfig();
                Processors.ForEach(delegate (IStiProcessor p)
                {
                    p.Stop();
                });
                var timeSpan = TimeSpan.FromSeconds(10.0);
                while (Processors.Any(processor => processor.ProcessorStatus != StiProcessorStatus.Stopped))
                {
                    var timeSpan2 = TimeSpan.FromSeconds(1.0);
                    Thread.Sleep(timeSpan2);
                    timeSpan -= timeSpan2;
                    if (timeSpan.TotalMilliseconds < 0.0)
                    {
                        break;
                    }
                }
                foreach (var processor in Processors)
                {
                    if (processor.ProcessorStatus != StiProcessorStatus.Stopped)
                    {
                        processor.Break();
                    }
                }
                Processors.Clear();
                //StiAppDomainWorker.ReleaseWorkers();
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception.Message);
            }
        }

        public virtual void Restart()
        {
            Stop();
            Start();
        }

        public virtual StiProcessorStatus GetStatus()
        {
            try
            {
                return /*(TasksProcessor != null) ? TasksProcessor.ProcessorStatus :*/ StiProcessorStatus.NotInitialized;
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception.Message);
            }
            return StiProcessorStatus.NotInitialized;
        }

        private void CreateProcessor(int processorsCount, Func<int?, StiProcessor> create)
        {
            if (processorsCount < 1)
            {
                processorsCount = 1;
            }
            if (processorsCount > 32)
            {
                processorsCount = 32;
            }
            for (var i = 1; i <= processorsCount; i++)
            {
                var arg = ((processorsCount == 1) ? null : new int?(i));
                var item = create(arg);
                Processors.Add(item);
            }
        }

    }
}
