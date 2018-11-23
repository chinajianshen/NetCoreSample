using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.Utils
{
    /// <summary>
    /// 程序路径
    /// </summary>
    public class AppPath
    {
        static AppPath()
        {
            //if (!Directory.Exists(PluginFolder))
            //{
            //    Directory.CreateDirectory(PluginFolder);
            //}

            if (!Directory.Exists(TempFolder))
            {
                Directory.CreateDirectory(TempFolder);
            }

            //if (!Directory.Exists(RawDataFolder))
            //{
            //    Directory.CreateDirectory(RawDataFolder);
            //}

        }


        public static string App_Root = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        /// <summary>
        /// 日志路径
        /// </summary>
        public static string LogFolder = Path.Combine(App_Root.Substring(0, App_Root.LastIndexOf(@"\") + 1), "Logs");
        public static string UserActionLogFolder = Path.Combine(App_Root.Substring(0, App_Root.LastIndexOf(@"\") + 1), "UserLogs");

        /// <summary>
        /// 日志路径
        /// </summary>  
        public static string TempFolder = Path.Combine(App_Root.Substring(0, App_Root.LastIndexOf(@"\") + 1), "Temp");



        public static string HostName = System.Net.Dns.GetHostName();
        public static string Attachment = ConfigurationManager.AppSettings["AttachmentUrl"];
        public static string AttachmentServerID = ConfigurationManager.AppSettings["AttachmentServerID"];
        public static List<string> FullMatchPublisherNames = string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["FullMatchPublishers"]) ? new List<string>() : new List<string>(System.Configuration.ConfigurationManager.AppSettings["FullMatchPublishers"].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries));
    }
}
