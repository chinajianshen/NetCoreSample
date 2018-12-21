using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Transfer8Pro.Entity;
using Transfer8Pro.Utils;

namespace Transfer8Pro.Core.Infrastructure
{
    public class QuartzHelper
    {
        /// <summary>
        ///  解析cronExpression
        /// </summary>
        /// <param name="cronExpression"></param>
        /// <returns></returns>
        public static CronExpressionEntity ResolveCronExpression(string cronExpression)
        {
            #region Cron表达式样例
            // 每天12: 15:00执行一次     0 15 13 * * ? *
            // 每天每隔10分钟执行一次    0 0/10 * * * ? *  每天每隔2小时执行一次 0 0 0/2 * * ? *
            // 每周一到周五 13：15分执行 0 15 13 ? * 2,3,4,5,6 *
            // 每月 1 2号 13：15分执行   0 15 13 1,2 * ? *
            #endregion

            CronExpressionEntity cronExpEntity = new CronExpressionEntity();
            if (!ValidExpression(cronExpression))
            {
                throw new Exception("参数cronExpression值校验失败");
            }

            List<string> cronExpList = cronExpression.Split(new char[] { ' '}, StringSplitOptions.RemoveEmptyEntries).ToList();

            if (cronExpList.Count != 7)
            {
                throw new Exception("参数cronExpression数据格式没有按照传8所要求的设置");
            }

            //获取后4位
            string lastFourChar = "";
            for (int i = 3; i <= 6; i++)
            {
                lastFourChar += cronExpList[i];
            }

            cronExpEntity.Second = cronExpList[0];
            cronExpEntity.Minute = cronExpList[1];
            cronExpEntity.Hour = cronExpList[2];

            if (lastFourChar == "**?*") //执行计划 每天
            {
                cronExpEntity.CycleTypes = CycleTypes.D;

                if (!cronExpList[2].Contains("/") && cronExpList[2] != "*")//每天执行一次
                {
                    cronExpEntity.ExecutingOnce = true;
                }
                else //每天执行多次
                {
                    cronExpEntity.ExecutingOnce = false;
                }
            }
            else if ((cronExpList[5].Split(','))[0].ToInt() > 0) //执行计划 每周
            {
                cronExpEntity.CycleTypes = CycleTypes.W;
                cronExpEntity.SelectedTimestamp = cronExpList[5];
                if (cronExpEntity.SelectedTimestamp.Split(',').Length == 1)
                {
                    cronExpEntity.ExecutingOnce = true;
                }
                else
                {
                    cronExpEntity.ExecutingOnce = false;
                }

            }
            else if ((cronExpList[3].Split(',')[0]).ToInt() > 0) //执行计划 每月
            {
                cronExpEntity.CycleTypes = CycleTypes.M;
                cronExpEntity.SelectedTimestamp = cronExpList[3];
                if (cronExpEntity.SelectedTimestamp.Split(',').Length == 1)
                {
                    cronExpEntity.ExecutingOnce = true;
                }
                else
                {
                    cronExpEntity.ExecutingOnce = false;
                }
            }
            else
            {
                throw new Exception("参数cronExpression数据格式没有按照传8所要求的设置");
            }
            return cronExpEntity;
        }

        /// <summary>
        ///  生成cronExpression表达式字符串
        /// </summary>
        /// <param name="cronExpEntity"></param>
        /// <returns></returns>
        public static string GenerateCronExpression(CronExpressionEntity cronExpEntity)
        {
            #region Cron表达式样例
            // 每天12: 15:00执行一次     0 15 13 * * ? *
            // 每天每隔10分钟执行一次    0 0/10 * * * ? *  每天每隔2小时执行一次 0 0 0/2 * * ? *
            // 每周一到周五 13：15分执行 0 15 13 ? * 2,3,4,5,6 *
            // 每月 1 2号 13：15分执行   0 15 13 1,2 * ? *
            #endregion
            string cronStr = "";
            string cronFormatter = "{0} ";

            cronStr += string.Format(cronFormatter, cronExpEntity.Second);
            cronStr += string.Format(cronFormatter, cronExpEntity.Minute);

            if (cronExpEntity.CycleTypes == CycleTypes.D)
            {
                cronStr += string.Format(cronFormatter, cronExpEntity.Hour);
                cronStr += "* * ? *";
            }
            else if (cronExpEntity.CycleTypes == CycleTypes.W)
            {
                cronStr += string.Format(cronFormatter, cronExpEntity.Hour);
                cronStr += "? * ";
                cronStr += string.Format(cronFormatter, cronExpEntity.SelectedTimestamp);
                cronStr += "*";
            }
            else
            {
                cronStr += string.Format(cronFormatter, cronExpEntity.Hour);
                cronStr += string.Format(cronFormatter, cronExpEntity.SelectedTimestamp);
                cronStr += "* ? *";
            }

            if (!ValidExpression(cronStr))
            {
                throw new Exception("生成cronExpression表达式字符串失败");
            }
            return cronStr;
        }

        /// <summary>
        /// 校验字符串是否为正确的Cron表达式
        /// </summary>
        /// <param name="cronExpression">带校验表达式</param>
        /// <returns></returns>
        public static bool ValidExpression(string cronExpression)
        {
            return CronExpression.IsValidExpression(cronExpression);
        }

        /// <summary>
        /// 获取任务在未来周期内哪些时间会运行
        /// </summary>
        /// <param name="CronExpressionString">Cron表达式</param>
        /// <param name="numTimes">运行次数</param>
        /// <returns>运行时间段</returns>
        public static List<DateTime> GetNextFireTime(string CronExpressionString, int numTimes)
        {
            if (numTimes < 0)
            {
                throw new Exception("参数numTimes值大于等于0");
            }
            //时间表达式
            ITrigger trigger = TriggerBuilder.Create().WithCronSchedule(CronExpressionString).Build();
            IList<DateTimeOffset> dates = TriggerUtils.ComputeFireTimes(trigger as IOperableTrigger, null, numTimes);
            List<DateTime> list = new List<DateTime>();
            foreach (DateTimeOffset dtf in dates)
            {
                list.Add(TimeZoneInfo.ConvertTimeFromUtc(dtf.DateTime, TimeZoneInfo.Local));
            }
            return list;
        }
    }
}
