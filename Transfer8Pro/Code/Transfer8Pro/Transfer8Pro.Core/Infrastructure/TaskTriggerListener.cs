using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Transfer8Pro.Core.Service;

namespace Transfer8Pro.Core.Infrastructure
{
    /// <summary>
    /// 任务触发器监听
    /// </summary>
    public class TaskTriggerListener : ITriggerListener
    {
        public string Name { get { return "All_TriggerListener"; } }

        /// <summary>
        /// Job完成时调用
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="context"></param>
        /// <param name="triggerInstructionCode"></param>
        public void TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode)
        {
            Guid guid;
            if (Guid.TryParse(trigger.JobKey.Name, out guid))
            {
                new TaskService().UpdateNextFireTime(trigger.JobKey.Name, TimeZoneInfo.ConvertTimeFromUtc(context.NextFireTimeUtc.Value.DateTime, TimeZoneInfo.Local));
            }

        }

        /// <summary>
        /// Job执行时调用
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="context"></param>
        public void TriggerFired(ITrigger trigger, IJobExecutionContext context)
        {

        }

        /// <summary>
        /// 错过触发时调用
        /// </summary>
        /// <param name="trigger"></param>
        public void TriggerMisfired(ITrigger trigger)
        {

        }

        /// <summary>
        /// Trigger触发后，job执行时调用本方法。true即否决，job后面不执行。
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool VetoJobExecution(ITrigger trigger, IJobExecutionContext context)
        {
            Guid guid;
            if (Guid.TryParse(trigger.JobKey.Name, out guid))
            {
                new TaskService().UpdateRecentRunTime(trigger.JobKey.Name, TimeZoneInfo.ConvertTimeFromUtc(context.FireTimeUtc.Value.DateTime, TimeZoneInfo.Local));
            }
            return false;
        }
    }
}
