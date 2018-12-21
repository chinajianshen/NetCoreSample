using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Transfer8Pro.Entity
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class SystemConfigEntity
    {
         public int SysConfigID { get; set; }

        public string SysConfigName { get; set; }

        /// <summary>
        ///  状态 0未开启 1开启
        ///  当前项开卷系统类型(70) 0未设置 1日数据采集系统 2以前传8系统
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 数据导出和FTP上传设置多久轮询一次  也可程序配置文件中设置（配置文件优先）
        /// </summary>
        public string Cron { get; set; }

        /// <summary>
        /// 存放其他信息 如存放系统版本
        /// </summary>
        public string ExSetting01 { get; set; }

        public string ExSetting02 { get; set; }
    }
}
