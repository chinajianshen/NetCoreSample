using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Transfer8Pro.Core.Infrastructure;
using Transfer8Pro.Entity;
using System.Threading;
using Transfer8Pro.Core.Service;
using Transfer8Pro.Utils;

namespace Transfer8Pro.Core.QuartzJobs
{
    /// <summary>
    /// 传吧作业
    /// </summary>
    public class TaskJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {           
            try
            {
                TaskEntity orgTaskEntity = context.JobDetail.JobDataMap["TaskParam"] as TaskEntity;
                TaskEntity taskEntity = QuartzBase.GetCurrentTaskList().Find(item => item.TaskID == orgTaskEntity.TaskID);
                CancellationToken ct = (CancellationToken)context.JobDetail.JobDataMap["CanellationTokenParam"];
               
                //构造数据文件产品并执行
                DbFileProductDirector director = new DbFileProductDirector();
                ADbFileProductBuilder productBuilder = Common.GetDbFileProductBuilder();
                director.ConstructProduct(productBuilder);
                DbFileProduct product = productBuilder.GetDbFileProduct();
                product.Execute(taskEntity, ct);
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog($"TaskJob作业类出现异常，异常信息：[{ex.Message}][{ex.StackTrace}]");

                JobExecutionException jex = new JobExecutionException(ex);
                //立即重新执行任务 
                jex.RefireImmediately = true;
            }
        }
    }
}
