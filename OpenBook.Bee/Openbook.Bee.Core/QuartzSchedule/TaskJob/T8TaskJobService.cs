using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBook.Bee.Entity;
using Quartz;

namespace Openbook.Bee.Core.QuartzSchedule.TaskJob
{
    public class T8TaskJobService : JobService<T8TaskJob>
    {
        public override T8TaskEntity T8TaskEntity { get; set; }

        protected override string JobName
        {
            get
            {
                return T8TaskEntity.T8FileEntity.JobName;
            }
        }

        protected override string GroupName
        {
            get
            {
                return T8TaskEntity.T8FileEntity.GroupName;
            }
        }

        protected override ITrigger GetTrigger()
        {
            var trigger = TriggerBuilder.Create()
                .WithIdentity(JobName, "作业触发器")
                //.WithSimpleSchedule(x => x.WithIntervalInSeconds(40).RepeatForever()) //显式指定作业时间
                .WithCronSchedule(T8TaskEntity.T8FileEntity.QuartzCronExpression)
                .Build();
            return trigger;
        }
    }
}
