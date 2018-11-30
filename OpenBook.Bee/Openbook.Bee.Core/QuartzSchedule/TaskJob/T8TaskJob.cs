using OpenBook.Bee.Entity;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openbook.Bee.Core.QuartzSchedule.TaskJob
{
    public class T8TaskJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            JobKey key =  context.JobDetail.Key;
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            T8TaskEntity t8Task = dataMap["prms"] as T8TaskEntity;
            Console.WriteLine("1111");
        }
    }
}
