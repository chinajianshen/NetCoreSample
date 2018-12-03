using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBook.Bee.Entity;

namespace Openbook.Bee.Core.QuartzSchedule
{
    /// <summary>
    /// 作业服务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class JobService<T> where T : IJob
    {
        /// <summary>
        /// 作业名
        /// </summary>
        protected abstract string JobName { get; }

        /// <summary>
        /// 组名
        /// </summary>
        protected abstract string GroupName { get; }

        public abstract T8TaskEntity T8TaskEntity { get; set; }

        /// <summary>
        /// 创建作业
        /// </summary>
        /// <returns></returns>
        protected virtual IJobDetail GetJobDetail()
        {           
            JobDataMap jobDataMap = new JobDataMap();
            jobDataMap.Add("prms", this.T8TaskEntity);

            var job = JobBuilder.Create<T>()
             .WithIdentity(JobName, GroupName)
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
