using System;
using System.Threading;

namespace Quartz.Net.Jobs
{
    /// <summary>
    /// 实现IJob接口
    /// </summary>
    public class Job1 : IJob
    {
        //使用Common.Logging.dll日志接口实现日志记录
        private static readonly Common.Logging.ILog logger = Common.Logging.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region IJob 成员

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                logger.Info("Job1 任务运行开始");

                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(100);
                    logger.InfoFormat("Job1 正在运行{0}", i);
                }

                logger.Info("Job1 任务运行结束");
            }
            catch (Exception ex)
            {
                logger.Error("Job1 运行异常", ex);
            }

        }

        #endregion
    }
}
