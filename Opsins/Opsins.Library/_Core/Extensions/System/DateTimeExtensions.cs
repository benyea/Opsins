using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Opsins
{
    /// <summary>
    /// DateTime扩展
    /// </summary>
    public static class DateTimeExtensions
    {
        private static readonly DateTime MinDate = new DateTime(1900, 1, 1);
        private static readonly DateTime MaxDate = new DateTime(9999, 12, 31, 23, 59, 59, 999);

        #region 判断时间有效

        /// <summary>
        /// 判断时间是否在有效
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static bool IsValid(this DateTime target)
        {
            return (target >= MinDate) && (target <= MaxDate);
        }

        #endregion

        #region 日期
        /// <summary>
        /// 是否为闰年
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>
        /// 	<c>true</c> if [is leap year] [the specified date]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLeapYear(this DateTime date)
        {
            return date.Year % 4 == 0 && (date.Year % 100 != 0 || date.Year % 400 == 0);
        }


        /// <summary>
        /// 是否为一个月的最后一天
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>
        /// 	<c>true</c> if [is last day of month] [the specified date]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLastDayOfMonth(this DateTime date)
        {
            int lastDayOfMonth = GetLastDayOfMonth(date);
            return lastDayOfMonth == date.Day;
        }

        /// <summary>
        /// 是否为周末
        /// </summary>
        /// <param name="source">日期</param>
        /// <returns>
        /// 	<c>true</c> if the specified source is a weekend; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWeekend(this DateTime source)
        {
            return source.DayOfWeek == DayOfWeek.Saturday ||
                   source.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// 获取月最后一天
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static int GetLastDayOfMonth(this DateTime date)
        {
            if (IsLeapYear(date) && date.Month == 2) return 28;
            if (date.Month == 2) return 27;
            if (date.Month == 1 || date.Month == 3 || date.Month == 5 || date.Month == 7
                || date.Month == 8 || date.Month == 10 || date.Month == 12)
                return 31;
            return 30;
        }

        /// <summary>
        /// 设置日期
        /// </summary>
        /// <param name="source"></param>
        /// <param name="day">天数范围(1-31)</param>
        /// <returns></returns>
        public static DateTime SetDay(this DateTime source, int day)
        {
            return new DateTime(source.Year, source.Month, day);
        }

        /// <summary>
        /// 设置月份
        /// </summary>
        /// <param name="source"></param>
        /// <param name="month">月数范围 (1-12)</param>
        /// <returns></returns>
        public static DateTime SetMonth(this DateTime source, int month)
        {
            return new DateTime(source.Year, month, source.Day);
        }

        /// <summary>
        /// 设置年份
        /// </summary>
        /// <param name="source"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DateTime SetYear(this DateTime source, int year)
        {
            return new DateTime(year, source.Month, source.Day);
        }

        #endregion

        #region 转换

        /// <summary>
        /// 获取"yyyyMMdd"格式的时间[20120102]
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string ToYmdString(this DateTime target)
        {
            return target.ToString("yyyyMMdd");
        }

        /// <summary>
        /// 转换为javascript date.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static double ToJavascriptDate(this DateTime dt)
        {
            DateTime d1 = new DateTime(1970, 1, 1);
            DateTime d2 = dt.ToUniversalTime();
            TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);
            return ts.TotalMilliseconds;
        }

        #endregion

        #region 时间

        /// <summary>
        /// 设置和获取DataTime
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="hours">The hours.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        /// <returns></returns>
        public static DateTime GetDateWithTime(this DateTime date, int hours, int minutes, int seconds)
        {
            return new DateTime(date.Year, date.Month, date.Day, hours, minutes, seconds);
        }


        /// <summary>
        /// 设置和获取DataTime
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        public static DateTime GetDateWithTime(this DateTime date, TimeSpan time)
        {
            return new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds);
        }


        /// <summary>
        /// 获取当前DataTime
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime GetDateWithCurrentTime(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }

        #endregion
    }
}
