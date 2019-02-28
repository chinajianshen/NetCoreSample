using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommLib
{
    internal static class LogHelper
    {
        public static string GetLogSubFolder(string strLogPath, DateTime dt)
        {
            return string.Format("{0}{1:yyyy}-{1:MM}\\", strLogPath, dt);
        }

        public static string GetLogFileName(string strLogPath, string strLogType, DateTime dt)
        {
            return string.Format("{0}{1:yyyy}-{1:MM}\\{2}_{1:dd}.log", strLogPath, dt, strLogType);
        }

        public static string DtToString(DateTime dtIn)
        {
            return string.Format("{0:HH:mm:ss.fff}", dtIn);
        }

        public static DateTime StringToDt(string strDateTime, DateTime dtIn)
        {
            DateTime dt = DateTime.Parse(strDateTime);
            return new DateTime(dtIn.Year, dtIn.Month, dtIn.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
        }
    }
}
