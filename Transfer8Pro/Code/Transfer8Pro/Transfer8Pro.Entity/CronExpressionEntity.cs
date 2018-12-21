using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Transfer8Pro.Entity
{
    /// <summary>
    /// Cron表达式实体
    /// </summary>
    public class CronExpressionEntity
    {
        public CronExpressionEntity()
        {
            Second = "0";
            Hour = "*";
            Minute = "0";
        }

        /// <summary>
        /// 日期类型
        /// </summary>
       public CycleTypes CycleTypes { get; set; }

        /// <summary>
        /// 是否执行一次
        /// </summary>
        public bool ExecutingOnce { get; set; }

        /// <summary>
        /// 选中时间戳
        /// 如每周 2，3 4表示周一周二周三 （注意1表示周日 7表示周六）
        /// 如每月 1，2，3,999表示 1号 2号 3号 最后一天
        /// </summary>
        public string SelectedTimestamp { get; set; }
      
        /// <summary>
        /// 时
        /// </summary>
        public string Hour { get; set; }       

        /// <summary>
        /// 分
        /// </summary>
        public string Minute { get; set; }
       
        /// <summary>
        /// 秒
        /// </summary>
        public string Second { get; set; }

    }
}
