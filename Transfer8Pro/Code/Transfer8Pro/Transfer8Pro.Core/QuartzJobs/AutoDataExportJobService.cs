using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Quartz;
using Transfer8Pro.Core.Infrastructure;
using Transfer8Pro.Utils;

namespace Transfer8Pro.Core.QuartzJobs
{
    /// <summary>
    /// 自动数据导出服务
    /// </summary>
    public class AutoDataExportJobService : JobService<AutoDataExportJob>
    {
        protected override string JobName => "自动添加传8任务作业";

        protected override string GroupName => "自动添加传8任务作业组";

        protected override CancellationToken CancelToken { get;}

        public override string JobKey
        {
            get
            {
                return "T8_AutoDataExportJob";
            }
        }

        public AutoDataExportJobService(CancellationToken ct)
        {
            CancelToken = ct;
        }

        protected override ITrigger GetTrigger()
        {
            try
            {
                ITrigger trigger = null;
                string cronExp = ConfigHelper.GetConfig("AutoJobCronExpression", "0 0/10 * * * ? *");
                if (QuartzHelper.ValidExpression(cronExp))
                {
                    trigger = TriggerBuilder.Create().WithIdentity(JobName, "自动传8任务作业触发器")
                              .WithCronSchedule(cronExp).Build();
                }
                else
                {
                    throw new Exception($"执行自动作业处理服务AutoAddJobService.GetTrigger()，配置AutoJobCronExpression的Cron表达式[{cronExp}]语法错误");
                }
                return trigger;
            }
            catch (Exception ex)
            {
                throw new Exception($"自动作业处理服务AutoAddJobService.GetTrigger()异常，异常信息[{ex.Message}]");
            }
        }
    }
}
