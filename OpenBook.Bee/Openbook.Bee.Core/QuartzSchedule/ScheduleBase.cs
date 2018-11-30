using Openbook.Bee.Core.QuartzSchedule.TaskJob;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openbook.Bee.Core.QuartzSchedule
{
    /// <summary>
    /// 调度基类
    /// </summary>
    public class ScheduleBase
    {
        private static IScheduler _scheduler;
        /// <summary>
        ///  创建调度器
        /// </summary>
        public static IScheduler Scheduler
        {
            get
            {
                if (_scheduler != null)
                {
                    return _scheduler;
                }

                var properties = new NameValueCollection();
                properties["quartz.scheduler.instanceName"] = ConfigurationManager.AppSettings["quartz_scheduler_instanceName"];

                // 设置线程池
                properties["quartz.threadPool.type"] = ConfigurationManager.AppSettings["quartz_threadPool_type"];
                properties["quartz.threadPool.threadCount"] = ConfigurationManager.AppSettings["quartz_threadPool_threadCount"];
                properties["quartz.threadPool.threadPriority"] = ConfigurationManager.AppSettings["quartz_threadPool_threadPriority"];

                // 远程输出配置
                properties["quartz.scheduler.exporter.type"] = ConfigurationManager.AppSettings["quartz_scheduler_exporter_type"];
                properties["quartz.scheduler.exporter.port"] = ConfigurationManager.AppSettings["quartz_scheduler_exporter_port"];
                properties["quartz.scheduler.exporter.bindName"] = ConfigurationManager.AppSettings["quartz_scheduler_exporter_bindName"];
                properties["quartz.scheduler.exporter.channelType"] = ConfigurationManager.AppSettings["quartz_scheduler_exporter_channelType"];

                var schedulerFactory = new StdSchedulerFactory(properties);
                _scheduler = schedulerFactory.GetScheduler();
                return _scheduler;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        public static void AddSchedule<T>(JobService<T> service) where T : IJob
        {
            service.AddJobToSchedule(Scheduler); 
        }

        /// <summary>
        /// 添加多个作业
        /// </summary>
        public static void AddSchedules()
        {

        }
    }
}
