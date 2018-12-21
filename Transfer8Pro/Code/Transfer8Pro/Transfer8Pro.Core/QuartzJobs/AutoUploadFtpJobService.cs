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
    /// 文件上传FTP服务
    /// </summary>
    public class AutoUploadFtpJobService : JobService<AutoUploadFtpJob>
    {
        protected override string JobName { get { return "自动上传FTP服务器作业"; } }

        protected override string GroupName
        {
            get { return "自动上传FTP服务器作业组"; }
        }

        protected override CancellationToken CancelToken { get; }
        public override string JobKey
        {
            get
            {
                return "T8_AutoUploadFtpJob";
            }
        }

        public AutoUploadFtpJobService(CancellationToken ct)
        {
            CancelToken = ct;
        }

        protected override ITrigger GetTrigger()
        {
            try
            {
                ITrigger trigger = null;
                string cronExp = ConfigHelper.GetConfig("AutoFtpJobCronExpression", "0 0/10 * * * ? *");
                if (QuartzHelper.ValidExpression(cronExp))
                {
                    trigger = TriggerBuilder.Create().WithIdentity(JobName, "自动上传FTP服务器作业触发器")
                              .WithCronSchedule(cronExp).Build();
                }
                else
                {
                    throw new Exception($"执行自动Ftp作业处理服务AutoUploadFtpJobService.GetTrigger()，配置AutoFtpJobCronExpression的Cron表达式[{cronExp}]语法错误");
                }
                return trigger;
            }
            catch (Exception ex)
            {
                throw new Exception($"自动Ftp作业处理服务AutoUploadFtpJobService.GetTrigger()异常，异常信息[{ex.Message}]");
            }
        }
    }
}
