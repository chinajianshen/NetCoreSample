using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Transfer8Pro.Entity;

namespace Transfer8Pro.Client.Core
{
   public  interface ITaskPlanBase
    {
        /// <summary>
        /// 获取或设置Cron实体
        /// </summary>
        CronExpressionEntity CronExpressionEntity { get; set; }

        /// <summary>
        /// 获取Cron表达式值
        /// </summary>
        /// <returns></returns>
        string GetCronExpressionString();

        /// <summary>
        /// 验证数据
        /// </summary>
        /// <returns></returns>
        bool ValidateData();      
       
    }
}
