using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Transfer8Pro.Entity
{
    /// <summary>
    /// 任务日志实体
    /// </summary>
    public class TaskLogEntity
    {
        public int TaskLogID { get; set; }

        public string TaskID { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        /// <summary>
        /// 耗时
        /// </summary>
        public string ElapsedTime { get; set; }

        public TaskExecutedStatus TaskExecutedStatus { get; set; }

        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 任务日志明细实体
    /// </summary>
    public class TaskLogDetailEntity
    {
        public int TaskLogDetailID { get; set; }

        public string TaskID { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        /// <summary>
        /// 耗时
        /// </summary>
        public string ElapsedTime { get; set; }

        public TaskExecutedStatus TaskExecutedStatus { get; set; }

        public DateTime CreateTime { get; set; }

        public string ErrorContent { get; set; }       
    }
}
