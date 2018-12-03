using System;
using System.Threading;

namespace Quartz.Net.Jobs   
{
    /// <summary>
    /// 实现IJob接口
    /// </summary>
    public class Job2 : IJob
    {
        //使用log4net.dll日志接口实现日志记录
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region IJob 成员

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                logger.Info("Job2 任务运行开始");

                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(100);
                    logger.InfoFormat("Job2 正在运行{0}", i);
                }

                logger.Info("Job2 任务运行结束");
            }
            catch (Exception ex)
            {
                logger.Error("Job2 运行异常", ex);
            }

        }

        #endregion
    }
}
