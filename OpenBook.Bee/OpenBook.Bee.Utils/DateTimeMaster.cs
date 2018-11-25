using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.Utils
{
    /// <summary>
    /// DateTimes 的摘要说明。
    /// </summary>
    public class DateTimeMaster
    {
        public static string DateTimeFormat = "yyyy-MM-dd HH\\:mm\\:ss";
        public static string DateFormat = "yyyy-MM-dd";
        public static string OracleDateTimeFormat = "yyyy-MM-dd hh24:nn:ss";
        public static string OracleDateFormat = "yyyy-mm-dd";
        public static string ShortDateFormatWithNoDayFormat = "MMM,yyyy";
        public static string ShortDateFormatWithNoDayFormat2 = "yyyy-MM";

        public static string YearFormat = "yyyy";

        public static int DiffMonthByINT(int small_yyyyMM, int big_yyyyMM)
        {
            return (big_yyyyMM / 100) * 12 + big_yyyyMM % 100 - (small_yyyyMM / 100) * 12 - small_yyyyMM % 100;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="yyyyMM"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static int AddMonthByINT(int yyyyMM, int m)
        {
            return yyyyMM > 100000 ? AddMonthByINT(new DateTime(yyyyMM / 100, yyyyMM % 100, 1), m) : 0;
        }

        public static int AddMonthByINT(DateTime dt, int m)
        {
            return int.Parse(dt.AddMonths(m).ToString("yyyyMM"));
        }


        #region Date Calculate

        /// <summary>
        /// 本周是本年第几周
        /// </summary>
        /// <param name="Date"></param>
        /// <returns></returns>
        public static int DatePart(System.DateTime Date)
        {
            //当天所在的年份
            int year = Date.Year;
            //当年的第一天
            DateTime firstDay = new DateTime(year, 1, 1);
            //当年的第一天是星期几
            int firstOfWeek = Convert.ToInt32(firstDay.DayOfWeek);
            if (firstOfWeek == 0)
            {
                firstOfWeek = 7;
            }
            //当年第一周的天数
            int firstWeekDayNum = 8 - firstOfWeek;

            //传入日期在当年的天数与第一周天数的差
            int otherDays = Date.DayOfYear - firstWeekDayNum;
            //传入日期不在第一周内
            if (otherDays > 0)
            {
                int weekNumOfOtherDays;
                if (otherDays % 7 == 0)
                {
                    weekNumOfOtherDays = otherDays / 7;
                }
                else
                {
                    weekNumOfOtherDays = otherDays / 7 + 1;
                }

                return weekNumOfOtherDays + 1;
            }
            //传入日期在第一周内
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// 今天起始时间  精确到秒
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime DayBegin(DateTime dt)
        {
            DateTime day = dt.Date;
            return day;
        }

        /// <summary>
        /// 今天结束时间  精确到秒
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime DayEnd(DateTime dt)
        {
            DateTime day = dt.Date.AddDays(1).AddSeconds(-1);
            return day;
        }

        /// <summary>
        /// 昨天起始时间  精确到秒
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime LastDayBegin(DateTime dt)
        {
            DateTime lastDay = dt.Date.AddDays(-1);
            return lastDay;
        }

        /// <summary>
        /// 昨天结束时间  精确到秒
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime LastDayEnd(DateTime dt)
        {
            DateTime lastDay = dt.Date.AddSeconds(-1);
            return lastDay;
        }

        /// <summary>
        /// 本周起始日期 精确到秒
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime WeekBegin(DateTime dt)
        {
            int weeknow = Convert.ToInt32(dt.DayOfWeek);
            if (weeknow == 0)
            {
                weeknow = 7;
            }
            int daydiff = (-1) * (weeknow - 1);
            DateTime dateBegin = dt.AddDays(daydiff);
            return dateBegin.Date;
        }

        /// <summary>
        /// 本周结束日期
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime WeekEnd(DateTime dt)
        {
            int weeknow = Convert.ToInt32(dt.DayOfWeek);
            if (weeknow == 0)
            {
                weeknow = 7;
            }
            int dayadd = 7 - weeknow;
            DateTime dateEnd = dt.AddDays(dayadd).AddDays(1).Date.AddSeconds(-1);
            return dateEnd;
        }

        /// <summary>
        /// 上周起始日期 精确到秒
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime LastWeekBegin(DateTime dt)
        {
            DateTime weekBegin = WeekBegin(dt).AddDays(-7);
            return new DateTime(weekBegin.Year,weekBegin.Month,weekBegin.Day,0,0,0);
        }

        /// <summary>
        /// 上周结束日期 精确到秒
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime LastWeekEnd(DateTime dt)
        {
            DateTime weekBegin = WeekBegin(dt).AddDays(-1);
            return new DateTime(weekBegin.Year, weekBegin.Month, weekBegin.Day, 23, 59, 59);
        }

        /// <summary>
        /// 返回参数时间上月开始日期 精确到秒
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime LastMonthBegin(DateTime dt)
        {
            DateTime lastMonth = dt.AddMonths(-1);
            return new DateTime(lastMonth.Year, lastMonth.Month,1, 0, 0, 0);
        }

        /// <summary>
        /// 返回参数时间上月结束日期 精确到秒
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime LastMonthEnd(DateTime dt)
        {
            DateTime lastMonth = dt.AddMonths(-1);
            int days = DaysInMonth(lastMonth.Year, lastMonth.Month);
            return new DateTime(lastMonth.Year, lastMonth.Month, days, 23, 59, 59);
        }

        /// <summary>
        /// 返回一个月内的天数
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="Month"></param>
        public static int DaysInMonth(int Year, int Month)
        {
            return DateTime.DaysInMonth(Year, Month);
        }

        /// <summary>
        /// 返回一个季度内的天数
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="Quarter"></param>
        public static int DaysInQuarter(int Year, int Quarter)
        {
            int i, d = 0;
            for (i = 3 * (Quarter - 1) + 1; i <= 3 * Quarter; i++)
            {
                d += DateTime.DaysInMonth(Year, i);
            }
            return d;
        }

        /// <summary>
        /// 返回一年的天数
        /// </summary>
        /// <param name="Year"></param>
        public static int DaysInYear(int Year)
        {
            if (DateTime.IsLeapYear(Year))
                return 366;
            else
                return 365;
        }

        /// <summary>
        /// 返回当日的最后一个时间点，精确到秒
        /// </summary>
        /// <param name="dtValue"></param>
        public static DateTime EndOfDay(DateTime dtValue)
        {
            return new DateTime(dtValue.Year, dtValue.Month, dtValue.Day, 23, 59, 59);
        }

        /// <summary>
        /// 返回当月的最后一个时间点，精确到秒
        /// </summary>
        /// <param name="dtValue"></param>
        public static DateTime EndOfMonth(DateTime dtValue)
        {
            return new DateTime(dtValue.Year, dtValue.Month, DaysInMonth(dtValue.Year, dtValue.Month), 23, 59, 59);
        }

        /// <summary>
        /// 返回本季度的最后一个时间点，精确到秒
        /// </summary>
        /// <param name="dtValue"></param>
        public static DateTime EndOfQuarter(DateTime dtValue)
        {
            int m;
            m = 3 * (((int)(dtValue.Month - 1) / 3) + 1);
            return new DateTime(dtValue.Year, m, DaysInQuarter(dtValue.Year, m), 23, 59, 59);
        }

        /// <summary>
        /// 返回本年的最后一个时间点，精确到秒
        /// </summary>
        /// <param name="dtValue"></param>
        public static DateTime EndOfYear(DateTime dtValue)
        {
            return new DateTime(dtValue.Year, 12, 31, 23, 59, 59);
        }

        /// <summary>
        /// 计算两个日期间的间隔秒数
        /// </summary>
        /// <param name="dtMinu">被减数</param>
        /// <param name="dtSust">减数</param>
        public static int DiffSecond(DateTime dtMinu, DateTime dtSust)
        {
            return (dtMinu - dtSust).Seconds;
        }

        /// <summary>
        /// 计算两个日期间的间隔天数
        /// </summary>
        /// <param name="dtMinu">被减数</param>
        /// <param name="dtSust">减数</param>
        public static int DiffDay(DateTime dtMinu, DateTime dtSust)
        {
            return (dtMinu - dtSust).Days;
        }

        /// <summary>
        /// 获取上周五的日期,小时，分秒部分为0
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime FirdayOfLastWeek()
        {
            DateTime dt = DateTime.Now;
            DateTime tmp;
            tmp = dt.AddDays(-2 - Convert.ToInt32(dt.DayOfWeek.ToString("d")));

            return new DateTime(tmp.Year, tmp.Month, tmp.Day, 0, 0, 0, 0);
        }

        /// <summary>
        /// 获取本周五的日期,小时，分秒部分为0
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime FirdayOfThisWeek()
        {
            DateTime dt = DateTime.Now;
            DateTime tmp;
            tmp = dt.AddDays(5 - Convert.ToInt32(dt.DayOfWeek.ToString("d")));

            return new DateTime(tmp.Year, tmp.Month, tmp.Day, 0, 0, 0, 0);
        }

        /// <summary>
        /// 获取本周三的日期,小时，分秒部分为0
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime WednesdayOfThisWeek()
        {
            DateTime dt = DateTime.Now;
            DateTime tmp;
            tmp = dt.AddDays(3 - Convert.ToInt32(dt.DayOfWeek.ToString("d")));

            return new DateTime(tmp.Year, tmp.Month, tmp.Day, 0, 0, 0, 0);
        }
        #endregion

        #region Date Convert
        /// <summary>
        /// 将日期转换为字符串
        /// </summary>
        /// <param name="dt"></param>
        public static string DateToStr(DateTime dt)
        {
            return dt.ToString(DateFormat);
        }
        /// <summary>
        /// 将日期转换为字符串(MMM,yyyy)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateToStrNoDay(DateTime dt)
        {
            return dt.ToString(ShortDateFormatWithNoDayFormat);
        }
        /// <summary>
        /// 将日期转换为字符串(yyyy-MM)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateToStrNoDay2(DateTime dt)
        {
            return dt.ToString(ShortDateFormatWithNoDayFormat2);
        }
        /// <summary>
        /// 将日期转换为年字符串(yyyy)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateToYearStr(DateTime dt)
        {
            return dt.ToString(YearFormat);
        }
        /// <summary>
        /// 将日期和时间转换为字符串
        /// </summary>
        /// <param name="dt"></param>
        public static string DateTimeToStr(DateTime dt)
        {
            return dt.ToString(DateTimeFormat);
        }
        #endregion

        #region Date ReOrganization
        /// <summary>
        /// 去掉日期时间中的毫秒数据
        /// </summary>
        /// <param name="dt"></param>
        public static DateTime ResetMilliseconds(DateTime dt)
        {
            DateTime dtRet = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
            return dtRet;
        }

        /// <summary>
        /// 去掉日期时间中的毫秒数据，对数组操作
        /// </summary>
        /// <param name="dt[]"></param>
        public static DateTime[] ResetMilliseconds(DateTime[] dt)
        {
            int index = 0;
            for (index = dt.GetLowerBound(0); index <= dt.GetUpperBound(0); index++)
            {
                dt[index] = ResetMilliseconds(dt[index]);
            }
            return dt;
        }

        /// <summary>
        /// 去掉日期时间中的时间数据
        /// </summary>
        /// <param name="dt"></param>
        public static DateTime ResetTime(DateTime dt)
        {
            DateTime dtRet = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
            return dtRet;
        }

        /// <summary>
        /// 去掉日期时间中的时间数据，对数组操作
        /// </summary>
        /// <param name="dt[]"></param>
        public static DateTime[] ResetTime(DateTime[] dt)
        {
            int index = 0;
            for (index = dt.GetLowerBound(0); index <= dt.GetUpperBound(0); index++)
            {
                dt[index] = ResetTime(dt[index]);
            }
            return dt;
        }

        /// <summary>
        /// 去掉日期时间中的日期数据
        /// </summary>
        /// <param name="dt"></param>
        public static DateTime ResetDate(DateTime dt)
        {
            DateTime dtRet = new DateTime(1, 1, 1, dt.Hour, dt.Minute, dt.Second);
            return dtRet;
        }

        /// <summary>
        /// 去掉日期时间中的日期数据，对数组操作
        /// </summary>
        /// <param name="dt[]"></param>
        public static DateTime[] ResetDate(DateTime[] dt)
        {
            int index = 0;
            for (index = dt.GetLowerBound(0); index <= dt.GetUpperBound(0); index++)
            {
                dt[index] = ResetDate(dt[index]);
            }
            return dt;
        }
        #endregion

        #region Date Check
        public static bool IsDate(string dt)
        {
            try
            {
                System.DateTime.Parse(dt);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsWeekDay(DateTime dt)
        {
            if ((dt.DayOfWeek == DayOfWeek.Saturday) || (dt.DayOfWeek == DayOfWeek.Sunday))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsWeekEnd(DateTime dt)
        {
            if ((dt.DayOfWeek == DayOfWeek.Saturday) || (dt.DayOfWeek == DayOfWeek.Sunday))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        #endregion Date Check

        /// <summary>
        /// 获取当前时间纬度的起始时间
        /// </summary>
        /// <param name="cycle">1-周，2-月，3-季，4-年</param>
        /// <returns></returns>
        public static DateTime[] GetCycleDateTime(int cycle)
        {
            DateTime dtnow = DateTime.Now;
            DateTime[] dts = new DateTime[2];
            switch (cycle)
            {
                case 1:
                    dts[0] = WeekBegin(dtnow);
                    dts[1] = WeekEnd2(dtnow);
                    break;
                case 2:
                    dts[0] = MonthBegin(dtnow);
                    dts[1] = MonthEnd(dtnow);
                    break;
                case 3:
                    dts[0] = QuarterStart(dtnow);
                    dts[1] = QuarterEnd(dtnow);
                    break;
                case 4:
                    dts[0] = YearStart(dtnow);
                    dts[1] = YearEnd(dtnow);
                    break;
            }
            return dts;
        }

        private static DateTime WeekEnd2(DateTime dtnow)
        {
            DateTime dt = WeekEnd(dtnow).AddDays(1);
            return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
            //return new DateTime(dt.Year, dt.Month, dt.Day + 1, 0, 0, 0);
        }

        /// <summary>
        ///  当月开始时间 精确到秒
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime MonthBegin(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1, 0, 0, 0);
        }

        /// <summary>
        /// 当月结束时间 精确到秒
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime MonthEnd(DateTime dt)
        {
            //int year = dt.Month + 1 > 12 ? dt.Year + 1 : dt.Year;
            //int month = dt.Month + 1 > 12 ? 1 : dt.Month + 1;
            int days = DaysInMonth(dt.Year, dt.Month);
            return new DateTime(dt.Year, dt.Month, days, 23, 59, 59);
        }
        public static DateTime QuarterStart(DateTime dt)
        {
            int smonth = ((dt.Month - 1) / 3) * 3 + 1;

            return new DateTime(dt.Year, smonth, 1, 0, 0, 0);
        }
        public static DateTime QuarterEnd(DateTime dt)
        {
            int smonth = ((dt.Month - 1) / 3) * 3 + 4;
            int year = smonth > 12 ? dt.Year + 1 : dt.Year;
            smonth = smonth > 12 ? 1 : smonth;
            return new DateTime(year, smonth, 1, 0, 0, 0);
        }
        public static DateTime YearStart(DateTime dt)
        {
            return new DateTime(dt.Year, 1, 1, 0, 0, 0);
        }
        public static DateTime YearEnd(DateTime dt)
        {
            return new DateTime(dt.Year + 1, 1, 1, 0, 0, 0);
        }
    }
}
