using Common.Logging;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenBook.Bee.Test.QuartzStudy
{
   public class Chaper2
    {
        ILog log = LogManager.GetLogger(typeof(Chaper1));
        public void Test()
        {
            try
            {
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Start();

                IJobDetail job = JobBuilder.Create<DumbJob2>()
                    .WithIdentity("myJob", "group2")
                    .UsingJobData("jobSays", "Hello,World")
                    .UsingJobData("myFloatValue", 3.141f)
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity("trigger2", "group2")
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


    public class DumbJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {            
            JobKey key = context.JobDetail.Key;

            JobDataMap dataMap = context.MergedJobDataMap;  // Note the difference from the previous example

            string jobSays = dataMap.GetString("jobSays");
            float myFloatValue = dataMap.GetFloat("myFloatValue");
            //IList<DateTimeOffset> state = (IList<DateTimeOffset>)dataMap["myStateData"];
            //state.Add(DateTimeOffset.UtcNow);

            Console.Error.WriteLine("Instance " + key + " of DumbJob says: " + jobSays + ", and val is: " + myFloatValue);
        }
    }

    public class DumbJob2 : IJob
    {
        public string JobSays { private get; set; }
        public float MyFloatValue { private get; set; }

        public void Execute(IJobExecutionContext context)
        {
            JobKey key = context.JobDetail.Key;

            JobDataMap dataMap = context.MergedJobDataMap;  // Note the difference from the previous example

            //IList<DateTimeOffset> state = (IList<DateTimeOffset>)dataMap["myStateData"];
            //state.Add(DateTimeOffset.UtcNow);

            Console.Error.WriteLine("Instance " + key + " of DumbJob says: " + JobSays + ", and val is: " + MyFloatValue);
        }
    }

}
