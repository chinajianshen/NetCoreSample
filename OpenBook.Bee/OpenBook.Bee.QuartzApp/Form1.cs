using OpenBook.Bee.QuartzApp.QuartzManager;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBook.Bee.QuartzApp
{
    public partial class Form1 : Form
    {
       private IScheduler MyScheduler { get; set; }


        public Form1()
        {
            InitializeComponent();
           Task.FromResult(CreateScheduler());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task.FromResult(SchedulerJob());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MyScheduler.Shutdown();
        }

        private async Task CreateScheduler()
        {
            //1创建一个调度器
            NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };
            StdSchedulerFactory factory = new StdSchedulerFactory(props);
            MyScheduler = await factory.GetScheduler();
            await MyScheduler.Start();
        }


        private async Task SchedulerJob()
        {           
            if (MyScheduler.IsShutdown)
            {
                await MyScheduler.Start();
            }
            //2创建一个任务
            IJobDetail job = JobBuilder.Create<TimeJob>().WithIdentity("job1", "group1").Build();

            //3创建一个触发器
            //DateTimeOffset runTime = DateBuilder.EvenMinuteDate(DateTimeOffset.UtcNow);
            //ITrigger trigger = TriggerBuilder.Create()
            //    .WithIdentity("trigger1", "group1")
            //    .WithCronSchedule("0/5 * * * * ?")     //5秒执行一次
            //                                           //.StartAt(runTime)
            //    .Build();

            ITrigger trigger = TriggerBuilder.Create()
                           .WithIdentity("trigger1","group1")
                           .StartNow()
                           .WithSimpleSchedule(x => x.WithIntervalInSeconds(5).RepeatForever())
                           .Build();


            //4将任务与触发器添加到调度器中
            await MyScheduler.ScheduleJob(job, trigger);

            //await Task.Delay(TimeSpan.FromSeconds(60));

            //await scheduler.Shutdown();

        }


    }
}
