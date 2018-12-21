using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Transfer8Pro.Entity;
using System.Threading;

namespace Transfer8Pro.Core.QuartzJobs
{
    /// <summary>
    /// 作业服务抽象类
    /// </summary>
    public abstract class JobService<T> where T:IJob
    {
        /// <summary>
        /// 作业名
        /// </summary>
        protected abstract string JobName { get; }

        /// <summary>
        /// 组名
        /// </summary>
        protected abstract string GroupName { get; }

        public abstract string JobKey { get;}

        protected abstract CancellationToken CancelToken { get; }

        /// <summary>
        /// 创建作业
        /// </summary>
        /// <returns></returns>
        protected virtual IJobDetail GetJobDetail()
        {
            JobDataMap jobDataMap = new JobDataMap();
            jobDataMap.Add("CanellationTokenParam", CancelToken);

            var job = JobBuilder.Create<T>()
             //.WithIdentity(JobName, GroupName)
             .WithIdentity(JobKey)
             .WithDescription(JobName+"-" + GroupName)
             .UsingJobData(jobDataMap)            
             .Build();
            return job;
        }

        /// <summary>
        /// 创建触发器
        /// </summary>
        /// <returns></returns>
        protected abstract ITrigger GetTrigger();

        /// <summary>
        /// 添加调度器
        /// </summary>
        /// <param name="scheduler"></param>
        public void AddJobToSchedule(IScheduler scheduler)
        {
            scheduler.ScheduleJob(GetJobDetail(), GetTrigger());
        }
    }
}
