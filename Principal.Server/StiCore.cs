using Microsoft.Extensions.Configuration;
using Principal.Server.Objects;
using Principal.Server.Processors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Principal.Server
{
    public class StiCore : IStiCore
    {
        private IConfiguration _configuration;

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
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            if (_configuration == null)
                _configuration = builder.Build();
        }

        public virtual void Start(bool runProcessors = true)
        {
            LoadConfig();
            CreateProcessor(1, (int? index) => new EjemploProcessor(this, index));
            CreateProcessor(1, index => new OwinServerProcessor(this, index, _configuration));

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
                return StiProcessorStatus.NotInitialized;
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
