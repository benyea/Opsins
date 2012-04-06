using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opsins
{
    /// <summary>
    /// integers扩展
    /// </summary>
    public static class IntegerExtensions
    {
        #region 执行次数

        /// <summary>
        /// 执行指定的动作在指定的次数，在每次迭代值传递
        /// </summary>
        /// <param name="ndx">开始值(0)</param>
        /// <param name="action">Action</param>
        public static void Times(this int ndx, Action<int> action)
        {
            for (int i = 0; i < ndx; i++)
            {
                action(i);
            }
        }

        /// <summary>
        /// 执行指定的动作在指定的次数，结束次数为end值[不包括上限值]，在每次迭代值传递
        /// </summary>
        /// <param name="start">开始值</param>
        /// <param name="end">结束值(不包括)</param>
        /// <param name="action">Action</param>
        public static void Upto(this int start, int end, Action<int> action)
        {
            for (int i = start; i < end; i++)
            {
                action(i);
            }
        }

        /// <summary>
        /// 执行指定的动作在指定的次数，结束次数为end值，在每次迭代值传递
        /// </summary>
        /// <param name="start">开始值</param>
        /// <param name="end">结束值(包括)</param>
        /// <param name="action">Action</param>
        public static void UptoIncluding(this int start, int end, Action<int> action)
        {
            for (int i = start; i <= end; i++)
            {
                action(i);
            }
        }

        /// <summary>
        /// 执行指定的动作在指定的次数，开始次数为start值[不包括下限值]，在每次迭代值传递
        /// </summary>
        /// <param name="start">开始值(不包括)</param>
        /// <param name="end">结束值</param>
        /// <param name="action">Action</param>
        public static void Downto(this int end, int start, Action<int> action)
        {
            for (int i = end; i > start; i--)
            {
                action(i);
            }
        }

        /// <summary>
        /// 执行指定的动作在指定的次数，下限次数为start值，在每次迭代值传递
        /// </summary>
        /// <param name="start">开始值(包括)</param>
        /// <param name="end">结束值</param>
        /// <param name="action">Action</param>
        public static void DowntoIncluding(this int end, int start, Action<int> action)
        {
            for (int i = end; i >= start; i--)
            {
                action(i);
            }
        }

        #endregion
        
        #region 奇偶判断

        /// <summary>
        /// 是否为奇数
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static bool IsOdd(this int i)
        {
            return (i % 2) != 0;
        }

        /// <summary>
        /// 是否为偶数
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static bool IsEven(this int i)
        {
            return (i % 2) == 0;
        }

        #endregion

        #region 大小判断

        /// <summary>
        /// 是否在两数之间
        /// </summary>
        /// <param name="low"></param>
        /// <param name="high"></param>
        /// <returns></returns>
        public static bool Between(this int i, int low, int high)
        {
            return i > low && i < high;
        }

        #endregion

        #region Bytes
        /// <summary>
        /// 转换为兆(M)
        /// </summary>
        /// <param name="numberInBytes">Number in bytes</param>
        /// <returns></returns>
        public static int MegaBytes(this int numberInBytes)
        {
            return numberInBytes / 1000000;
        }


        /// <summary>
        /// 转换为字节(K)
        /// </summary>
        /// <param name="numberInBytes">Number in bytes</param>
        /// <returns></returns>
        public static int KiloBytes(this int numberInBytes)
        {
            return numberInBytes / 1000;
        }


        /// <summary>
        /// 转换为T兆
        /// </summary>
        /// <param name="numberInBytes">Number in bytes</param>
        /// <returns></returns>
        public static int TeraBytes(this int numberInBytes)
        {
            return numberInBytes / 1000000000;
        }
        #endregion

        #region 日期扩展

        /// <summary>
        /// 返回一个字符串值，指定工作日的名称
        /// </summary>
        /// <param name="i">指定为周一至周五的数字，从1到7 ; 1表示一周的第一天， 7表示一周的最后一天</param>
        /// <param name="abbreviation">指示工作日名称缩写。默认为false</param>
        /// <returns></returns>
        public static string WeekdayName(this int i, bool abbreviation = false, DayOfWeek firstDayOfWeek = DayOfWeek.Sunday)
        {
            if (i < 1 || i > 7)
            {
                throw new ArgumentOutOfRangeException("i", "Invalid value (" + i + "). Numeric weekday designation must be from 1 through 7.");
            }
            return Microsoft.VisualBasic.DateAndTime.WeekdayName(i, abbreviation, ConvertToFirstDayOfWeek(firstDayOfWeek));
        }

        private static Microsoft.VisualBasic.FirstDayOfWeek ConvertToFirstDayOfWeek(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    return Microsoft.VisualBasic.FirstDayOfWeek.Monday;
                case DayOfWeek.Tuesday:
                    return Microsoft.VisualBasic.FirstDayOfWeek.Tuesday;
                case DayOfWeek.Wednesday:
                    return Microsoft.VisualBasic.FirstDayOfWeek.Wednesday;
                case DayOfWeek.Thursday:
                    return Microsoft.VisualBasic.FirstDayOfWeek.Thursday;
                case DayOfWeek.Friday:
                    return Microsoft.VisualBasic.FirstDayOfWeek.Friday;
                case DayOfWeek.Saturday:
                    return Microsoft.VisualBasic.FirstDayOfWeek.Saturday;
                case DayOfWeek.Sunday:
                default:
                    return Microsoft.VisualBasic.FirstDayOfWeek.Sunday;
            }
        }

        /// <summary>
        /// 返回一个字符串值，其中包含指定的月份的名称
        /// </summary>
        /// <param name="i">本月数</param>
        /// <param name="abbreviation">指示月份名称缩写。默认为false</param>
        /// <returns></returns>
        public static string MonthName(this int i, bool abbreviation = false)
        {
            return Microsoft.VisualBasic.DateAndTime.MonthName(i, abbreviation);
        }

        #endregion

        #region Dates

        /// <summary>
        /// 是否为闰年
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static bool IsLeapYear(this int year)
        {
            return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
        }


        /// <summary>
        /// 指定的多少天前
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public static DateTime DaysAgo(this int days)
        {
            return DateTime.Now.AddDays(-days);
        }


        /// <summary>
        /// 指定的多少月前.
        /// </summary>
        /// <param name="months"></param>
        /// <returns></returns>
        public static DateTime MonthsAgo(this int months)
        {
            return DateTime.Now.AddMonths(-months);
        }


        /// <summary>
        /// 指定的多少年前
        /// </summary>
        /// <param name="years"></param>
        /// <returns></returns>
        public static DateTime YearsAgo(this int years)
        {
            return DateTime.Now.AddYears(-years);
        }


        /// <summary>
        /// 指定的多少时前
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static DateTime HoursAgo(this int hours)
        {
            return DateTime.Now.AddHours(-hours);
        }


        /// <summary>
        /// 指定的多少分前
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static DateTime MinutesAgo(this int minutes)
        {
            return DateTime.Now.AddMinutes(-minutes);
        }


        /// <summary>
        /// 从现在开始的多少天后
        /// </summary>
        /// <param name="days">now</param>
        /// <returns></returns>
        public static DateTime DaysFromNow(this int days)
        {
            return DateTime.Now.AddDays(days);
        }


        /// <summary>
        /// 从现在开始的多少月后
        /// </summary>
        /// <param name="months">Number of months from now</param>
        /// <returns></returns>
        public static DateTime MonthsFromNow(this int months)
        {
            return DateTime.Now.AddMonths(months);
        }


        /// <summary>
        /// 从现在开始的多少年后
        /// </summary>
        /// <param name="years">now</param>
        /// <returns></returns>
        public static DateTime YearsFromNow(this int years)
        {
            return DateTime.Now.AddYears(years);
        }


        /// <summary>
        /// 从现在开始的多少小时后
        /// </summary>
        /// <param name="hours">now</param>
        /// <returns></returns>
        public static DateTime HoursFromNow(this int hours)
        {
            return DateTime.Now.AddHours(hours);
        }


        /// <summary>
        /// 从现在开始的多少分钟后
        /// </summary>
        /// <param name="minutes">now</param>
        /// <returns></returns>
        public static DateTime MinutesFromNow(this int minutes)
        {
            return DateTime.Now.AddMinutes(minutes);
        }

        #endregion

        #region Time

        /// <summary>
        /// 返回一个TimeSpan，表示毫秒数
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static System.TimeSpan Milliseconds(this int i)
        {
            return TimeSpan.FromMilliseconds(i);
        }

        /// <summary>
        /// 返回一个TimeSpan，表示秒数
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static System.TimeSpan Seconds(this int i)
        {
            return TimeSpan.FromSeconds(i);
        }

        /// <summary>
        /// 返回一个TimeSpan，表示分钟数
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static System.TimeSpan Minutes(this int i)
        {
            return TimeSpan.FromMinutes(i);
        }

        /// <summary>
        /// 返回一个TimeSpan，表示小时数
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static System.TimeSpan Hours(this int i)
        {
            return TimeSpan.FromHours(i);
        }

        /// <summary>
        /// 返回一个TimeSpan，表示天数
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static System.TimeSpan Days(this int i)
        {
            return TimeSpan.FromDays(i);
        }

        /// <summary>
        /// 返回一个TimeSpan，表示星期数
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static System.TimeSpan Weeks(this int i)
        {
            return ((TimeSpanWrapper)i.Days() * 7);
        }

        /// <summary>
        /// 将数字转换为timespan
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static TimeSpan Time(this int num)
        {
            return Time(num, false);
        }

        /// <summary>
        /// 将数字转换为timespan
        /// </summary>
        /// <param name="num"></param>
        /// <param name="convertSingleDigitsToHours">指示是否将“9”为9小时，而不是分钟</param>
        /// <returns></returns>
        public static TimeSpan Time(this int num, bool convertSingleDigitsToHours)
        {
            TimeSpan time = TimeSpan.MinValue;
            if (convertSingleDigitsToHours)
            {
                if (num <= 24)
                    num *= 100;
            }
            int hours = num / 100;
            int hour = hours;
            int minutes = num % 100;

            time = new TimeSpan(hours, minutes, 0);
            return time;
        }

        /// <summary>
        /// 返回格式化的标准时间
        /// 如： 1am 9:30pm
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string TimeWithSuffix(this int num)
        {
            TimeSpan time = Time(num, true);
            return TimeHelper.Format(time);
        }

        #endregion

        #region 进制转换

        /// <summary>
        /// 转换为16进制
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToHex(this int number)
        {
            return Convert.ToString(number, 16);
        }


        /// <summary>
        /// 转换为二进制
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToBinary(this int number)
        {
            return Convert.ToString(number, 2);
        }

        #endregion
    }
}
