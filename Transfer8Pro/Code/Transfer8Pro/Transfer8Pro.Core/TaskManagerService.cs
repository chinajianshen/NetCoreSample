using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Transfer8Pro.Core
{
    /// <summary>
    /// 任务管理服务
    /// </summary>
    public class TaskManagerService
    {      
        public void Start()
        {           
            QuartzBase.StartScheduler();
        }

        public void Stop()
        {
            QuartzBase.StopSchedule();           
        }
    }
}
