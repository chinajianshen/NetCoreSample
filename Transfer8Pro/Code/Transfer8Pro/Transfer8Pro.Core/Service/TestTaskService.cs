using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using Quartz.Impl;

namespace Transfer8Pro.Core.Service
{
    //quartz 任务调度
    public class TestTaskService
    {
        public TestTaskService()
        {
            scheduler= StdSchedulerFactory.GetDefaultScheduler();
        }
        IScheduler scheduler = null;
        public void Start()
        {
         
            IJobDetail job = JobBuilder.Create<MyJob>().Build();
            ITrigger trigger = TriggerBuilder.Create().StartNow().WithCronSchedule("0/5 * * * * ? ").Build();
            scheduler.ScheduleJob(job,trigger);
            scheduler.Start();
        }

        public void Stop()
        {
            scheduler.Shutdown();
        }
    }

    public class MyJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
        }
    }
}
