using Common.Logging;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Quartz.MisfireInstruction;

namespace OpenBook.Bee.Test.QuartzStudy
{
   public class Chaper1
    {
        ILog log = LogManager.GetLogger(typeof(Chaper1));

        public void test()
        {
            try
            {               
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Start();

                IJobDetail job = JobBuilder.Create<HelloJob>().WithIdentity("job1", "group1").Build();
                ITrigger trigger = TriggerBuilder.Create()
                      .WithIdentity("trigger1", "group1")
                      .StartNow()
                      .WithSimpleSchedule(x => x.WithIntervalInSeconds(5).RepeatForever())
                      .Build();
                scheduler.ScheduleJob(job, trigger);
                      

                Thread.Sleep(TimeSpan.FromSeconds(60));
                scheduler.Shutdown();
            }
            catch (SchedulerException ex)
            {
                log.Error(ex);
                Console.WriteLine(ex);
            }
        }
    }

    public class HelloJob : IJob
    {
        private static ILog _log = LogManager.GetLogger(typeof(HelloJob));


        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("你好,HelloJob作业");
            _log.Info(string.Format("HelloJob作业 - {0}", System.DateTime.Now.ToString("r")));
        }
    }
}
