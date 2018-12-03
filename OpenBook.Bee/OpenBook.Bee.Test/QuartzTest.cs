using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Openbook.Bee.Core;
using Openbook.Bee.Core.QuartzSchedule;
using Openbook.Bee.Core.QuartzSchedule.TaskJob;
using OpenBook.Bee.Entity;
using OpenBook.Bee.Utils;
using Quartz;
using Quartz.Impl;

namespace OpenBook.Bee.Test
{
   public class QuartzTest
    {
        public void Test1()
        {
            T8TaskEntity t8Task = new T8TaskEntity();
            t8Task.TaskTitle = "传8月采集任务";
            t8Task.GenerateTime = DateTime.Now;
            t8Task.T8TaskStatus = T8TaskStatus.Created;

            T8FileEntity fileEntity = new T8FileEntity();
            fileEntity.SqlString = "select * from dual";
            fileEntity.SqlStartTime = DateTimeMaster.LastMonthBegin(DateTime.Now);
            fileEntity.SqlEndTime = DateTimeMaster.LastMonthEnd(DateTime.Now);
            fileEntity.DbFileType = DbFileType.SQLite;
            fileEntity.GroupName = "月数据采集组";
            fileEntity.JobName = "月数据采集作业";
            fileEntity.QuartzCronExpression = "0/10 * * * * ?";

            t8Task.T8FileEntity = fileEntity;

            ScheduleBase.Scheduler.Start();
            T8TaskJobService jobService = new T8TaskJobService();
            jobService.T8TaskEntity = t8Task;
            ScheduleBase.AddSchedule(jobService);

            Thread.Sleep(TimeSpan.FromSeconds(120));
        }

        private static IScheduler scheduler = null;
        public void InitRemoteScheduler()
        {
            try
            {
                NameValueCollection properties = new NameValueCollection();
                properties["quartz.scheduler.instanceName"] = "schedMaintenanceService";
                properties["quartz.scheduler.proxy"] = "true";
                properties["quartz.scheduler.proxy.address"] = string.Format("{0}://{1}:{2}/QuartzScheduler", "tcp", "localhost", "8008");
                ISchedulerFactory sf = new StdSchedulerFactory(properties);

                scheduler = sf.GetScheduler();
                QuartzPauseJob("月数据采集作业");
            }
            catch (Exception ex)
            {
                //LogHelper.WriteLog("初始化远程任务管理器失败" + ex.StackTrace);//
                Console.WriteLine("初始化远程任务管理器失败" + ex.StackTrace);
            }
        }

        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <param name="JobKey"></param>
        private void QuartzPauseJob(string JobKey)
        {
            try
            {
                JobKey jk = new JobKey(JobKey);
                if (scheduler.CheckExists(jk))
                {
                    //任务已经存在则暂停任务
                    scheduler.PauseJob(jk);
                    //LogHelper.WriteLog(string.Format("任务“{0}”已经暂停", JobKey));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
