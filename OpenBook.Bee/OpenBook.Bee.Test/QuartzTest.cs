using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Openbook.Bee.Core;
using Openbook.Bee.Core.QuartzSchedule;
using Openbook.Bee.Core.QuartzSchedule.TaskJob;
using OpenBook.Bee.Entity;
using OpenBook.Bee.Utils;

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
    }
}
