using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommLib
{
    public static class DateTimeExt
    {
        //日期/时间格式
        public const string DATETIME_FMT_SHORT = "yyyy-MM-dd";
        public const string DATETIME_FMT_DATE_HM = "yyyy-MM-dd HH:mm";
        public const string DATETIME_FMT_FULL = "yyyy-MM-dd HH:mm:ss";
        public const string DATETIME_FMT_TIME = "HH:mm:ss";
        public const string DATETIME_FMT_HM = "HH:mm";
        public const string DATETIME_FMT_YM = "yyyy-MM";
        public const string DATETIME_FMT_FULL_TIME = "yyyy-MM-dd HH:mm:ss.fff";

        static readonly DateTime MIN_DB_DATETIME = new DateTime(1753, 1, 1, 12, 0, 0);
        static readonly DateTime MAX_DB_DATETIME = new DateTime(2099, 12, 31, 23, 59, 59);

        public static DateTime DbDateTimeFromTicks(long ticks)
        {
            DateTime dt = new DateTime(ticks);
            if (dt < MIN_DB_DATETIME)
            {
                return MIN_DB_DATETIME;
            }
            if (dt > MAX_DB_DATETIME)
            {
                return MAX_DB_DATETIME;
            }
            return dt;
        }

        /// <summary>
        /// 返回 2012-06
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string SimpleDateYM(this DateTime datetime)
        {
            return datetime.ToString(DATETIME_FMT_YM);
        }

        /// <summary>
        /// 返回 2012-06-07
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string SimpleDate(this DateTime datetime)
        {
            return datetime.ToString(DATETIME_FMT_SHORT);
        }

        /// <summary>
        /// 返回 2012-06-07 13:34
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string SimpleDateHM(this DateTime datetime)
        {
            return datetime.ToString(DATETIME_FMT_DATE_HM);
        }

        /// <summary>
        /// 返回 2012-06-07 13:34:40
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string SimpleDateFull(this DateTime datetime)
        {
            return datetime.ToString(DATETIME_FMT_FULL);
        }

        /// <summary>
        /// 返回 2012-06-07 13:34:40.456
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string SimpleDateFullTime(this DateTime datetime)
        {
            return datetime.ToString(DATETIME_FMT_FULL_TIME);
        }

        /// <summary>
        /// 返回 13:34:40
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string SimpleDateTime(this DateTime datetime)
        {
            return datetime.ToString(DATETIME_FMT_TIME);
        }

        /// <summary>
        /// 返回 13:34
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string SimpleDateTimeHM(this DateTime datetime)
        {
            return datetime.ToString(DATETIME_FMT_HM);
        }

        public static string SimpleDateTimeYM(this DateTime datetime)
        {
            return datetime.ToString(DATETIME_FMT_YM);
        }

        public static string GetChinessDayOfWeek(DateTime datetime)
        {
            string str = string.Empty;
            string dayofweek = datetime.DayOfWeek.ToString().ToLower();
            switch (dayofweek)
            {
                case "monday":
                    str = "星期一";
                    break;
                case "tuesday":
                    str = "星期二";
                    break;
                case "wednesday":
                    str = "星期三";
                    break;
                case "thursday":
                    str = "星期四";
                    break;
                case "friday":
                    str = "星期五";
                    break;
                case "saturday":
                    str = "星期六";
                    break;
                case "sunday":
                    str = "星期日";
                    break;
            }
            return str;
        }
    }
}
