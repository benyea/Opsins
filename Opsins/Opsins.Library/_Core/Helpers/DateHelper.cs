using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opsins
{
    /// <summary>
    /// 时间辅助类
    /// </summary>
    public static class DateHelper
    {
        #region 获取月份

        /// <summary>
        /// 根据月份代码获取月份数字的方法
        /// </summary>
        /// <param name="monthCode">月份代码</param>
        /// <returns>返回月份数字</returns>
        public static int GetMonth(string monthCode)
        {
            int month = 1;
            switch (monthCode.ToUpper())
            {
                case "JAN": month = 01; break;
                case "FEB": month = 02; break;
                case "MAR": month = 03; break;
                case "APR": month = 04; break;
                case "MAY": month = 05; break;
                case "JUN": month = 06; break;
                case "JUL": month = 07; break;
                case "AUG": month = 08; break;
                case "SEP": month = 09; break;
                case "OCT": month = 10; break;
                case "NOV": month = 11; break;
                default: month = 12; break;
            }
            return month;
        }

        /// <summary>
        /// 根据月份数字获取月份英文缩写的方法
        /// </summary>
        /// <param name="monthNo">月份数字</param>
        /// <returns>返回月份代码</returns>
        public static string GetMonthEn(int monthNo)
        {
            string monthCode;
            switch (monthNo)
            {
                case 1: monthCode = "JAN"; break;
                case 2: monthCode = "FEB"; break;
                case 3: monthCode = "MAR"; break;
                case 4: monthCode = "APR"; break;
                case 5: monthCode = "MAY"; break;
                case 6: monthCode = "JUN"; break;
                case 7: monthCode = "JUL"; break;
                case 8: monthCode = "AUG"; break;
                case 9: monthCode = "SEP"; break;
                case 10: monthCode = "OCT"; break;
                case 11: monthCode = "NOV"; break;
                default: monthCode = "DEC"; break;
            }
            return monthCode;
        }

        /// <summary>
        /// 根据月份数字获取月份中文表示的方法
        /// </summary>
        /// <param name="monthNo">月份数字</param>
        /// <returns>返回月份文字</returns>
        public static string GetMonthChs(int monthNo)
        {
            string monthChina = null;
            switch (monthNo)
            {
                case 1: monthChina = "一月"; break;
                case 2: monthChina = "二月"; break;
                case 3: monthChina = "三月"; break;
                case 4: monthChina = "四月"; break;
                case 5: monthChina = "五月"; break;
                case 6: monthChina = "六月"; break;
                case 7: monthChina = "七月"; break;
                case 8: monthChina = "八月"; break;
                case 9: monthChina = "九月"; break;
                case 10: monthChina = "十月"; break;
                case 11: monthChina = "十一月"; break;

                default: monthChina = "十二月"; break;
            }
            return monthChina;
        }

        /// <summary>
        /// 根据月份代码获取月份中文表示的方法
        /// </summary>
        /// <param name="monthCode">月份代码</param>
        /// <returns>返回月份文字</returns>
        public static string GetMonthChs(string monthCode)
        {
            string monthChina = null;
            switch (monthCode.ToUpper())
            {
                case "JAN": monthChina = "一月"; break;
                case "FEB": monthChina = "二月"; break;
                case "MAR": monthChina = "三月"; break;
                case "APR": monthChina = "四月"; break;
                case "MAY": monthChina = "五月"; break;
                case "JUN": monthChina = "六月"; break;
                case "JUL": monthChina = "七月"; break;
                case "AUG": monthChina = "八月"; break;
                case "SEPT": monthChina = "九月"; break;
                case "OCT": monthChina = "十月"; break;
                case "NOV": monthChina = "十一月"; break;

                default: monthChina = "十二月"; break;
            }
            return monthChina;
        }

        #endregion

        #region 获取星期

        /// <summary>
        /// 根据星期几数字返回简写的方法
        /// </summary>
        /// <param name="weekNumber">星期几数字</param>
        /// <returns>返回相应的代码</returns>
        public static string GetWeekEn(int weekNumber)
        {
            string result = "";
            switch (weekNumber)
            {
                case 1: result = "MON"; break;
                case 2: result = "TUE"; break;
                case 3: result = "WED"; break;
                case 4: result = "THU"; break;
                case 5: result = "FRI"; break;
                case 6: result = "SAT"; break;
                default: result = "SUN"; break;
            }
            return result;
        }

        /// <summary>
        /// 根据星期数字返回星期几文字的方法
        /// </summary>
        /// <param name="weekNo">星期数字</param>
        /// <returns>返回星期几文字</returns>
        public static string GetWeekChs(int weekNo)
        {
            string result;
            switch (weekNo)
            {
                case 0: result = "星期天"; break;
                case 1: result = "星期一"; break;
                case 2: result = "星期二"; break;
                case 3: result = "星期三"; break;
                case 4: result = "星期四"; break;
                case 5: result = "星期五"; break;
                default: result = "星期六"; break;
            }
            return result;
        }

        /// <summary>
        /// 根据星期缩写返回星期几文字的方法
        /// </summary>
        /// <param name="weekCode">星期缩写</param>
        /// <returns>返回星期几文字</returns>
        public static string GetWeekChs(string weekCode)
        {
            string result;
            switch (weekCode.ToUpper())
            {
                case "SUN": result = "星期天"; break;
                case "MON": result = "星期一"; break;
                case "TUE": result = "星期二"; break;
                case "WED": result = "星期三"; break;
                case "THU": result = "星期四"; break;
                case "FRI": result = "星期五"; break;
                default: result = "星期六"; break;
            }
            return result;
        }

        /// <summary>
        /// 根据传入的日期获取中文星期字符串
        /// </summary>
        /// <param name="date">传入的日期</param>
        /// <returns>返回中文星期字符串</returns>
        public static string GetWeekChs(DateTime date)
        {
            string result = string.Empty;
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday: result = "星期一"; break;
                case DayOfWeek.Tuesday: result = "星期二"; break;
                case DayOfWeek.Wednesday: result = "星期三"; break;
                case DayOfWeek.Thursday: result = "星期四"; break;
                case DayOfWeek.Friday: result = "星期五"; break;
                case DayOfWeek.Saturday: result = "星期六"; break;
                case DayOfWeek.Sunday: result = "星期日"; break;
            }
            return result;
        }

        #endregion

        #region 两时间相差计算

        /// <summary>
        /// 返回相差的小时数
        /// </summary>
        /// <param name="time"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static int DivHours(string time, int hours)
        {
            if (string.IsNullOrEmpty(time))
                return 1;
            TimeSpan ts = DateTime.Now - DateTime.Parse(time).AddHours(hours);
            if (ts.TotalHours > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (ts.TotalHours < int.MinValue)
            {
                return int.MinValue;
            }
            return (int)ts.TotalHours;
        }

        /// <summary>
        /// 返回相差的分钟数
        /// </summary>
        /// <param name="time"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static int DivMinutes(string time, int minutes)
        {
            if (string.IsNullOrEmpty(time))
                return 1;
            TimeSpan ts = DateTime.Now - DateTime.Parse(time).AddMinutes(minutes);
            if (ts.TotalMinutes > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (ts.TotalMinutes < int.MinValue)
            {
                return int.MinValue;
            }
            return (int)ts.TotalMinutes;
        }

        /// <summary>
        /// 返回相差的秒数
        /// </summary>
        /// <param name="time"></param>
        /// <param name="sec"></param>
        /// <returns></returns>
        public static int DivSeconds(string time, int sec)
        {
            TimeSpan ts = DateTime.Now - DateTime.Parse(time).AddSeconds(sec);
            if (ts.TotalSeconds > int.MaxValue)
            {
                return int.MaxValue;
            }
            if (ts.TotalSeconds < int.MinValue)
            {
                return int.MinValue;
            }
            return (int)ts.TotalSeconds;
        }

        /// <summary>
        /// 返回两个时间间隔
        /// </summary>
        /// <param name="dateTime1"></param>
        /// <param name="dateTime2"></param>
        /// <returns></returns>
        public static TimeSpan DivTimeSpan(DateTime dateTime1, DateTime dateTime2)
        {
            TimeSpan ts1 = new TimeSpan(dateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(dateTime2.Ticks);

            TimeSpan ts = ts1.Subtract(ts2).Duration();

            return ts;
        }

        /// <summary>
        /// 求时间与现在的时数间隔
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static TimeSpan DivTimeSpan(DateTime datetime)
        {
            TimeSpan ts1 = new TimeSpan(datetime.Ticks);
            TimeSpan ts = ts1.Subtract(new TimeSpan(DateTime.Now.Ticks)).Duration();
            return ts;
        }

        #endregion

        #region 时间与现在的时数间隔

        /// <summary>
        /// 求时间与现在的时数间隔
        /// </summary>
        /// <param name="oldtime"></param>
        /// <returns></returns>
        public static double DivHours(DateTime oldtime)
        {
            TimeSpan ts = DateTime.Now - oldtime;
            return ts.TotalHours;
        }

        /// <summary>
        /// 求时间与现在的时数间隔
        /// </summary>
        /// <param name="oldtime"></param>
        /// <returns></returns>
        public static double DivHours(object oldtime)
        {
            DateTime dt;
            if (DateTime.TryParse(oldtime.ToString(), out dt))
            {
                return DivHours(dt);
            }
            return 1;
        }

        /// <summary>
        /// 求时间与现在的分数间隔
        /// </summary>
        /// <param name="oldtime"></param>
        /// <returns></returns>
        public static double DivMinutes(DateTime oldtime)
        {
            TimeSpan ts = DateTime.Now - oldtime;
            return ts.TotalMinutes;
        }

        /// <summary>
        /// 求时间与现在的分数间隔
        /// </summary>
        /// <param name="oldtime"></param>
        /// <returns></returns>
        public static double DivMinutes(object oldtime)
        {
            DateTime dt;
            if (DateTime.TryParse(oldtime.ToString(), out dt))
            {
                return DivMinutes(dt);
            }
            return 1000;
        }

        /// <summary>
        /// 求时间与现在的秒数间隔
        /// </summary>
        /// <param name="oldtime"></param>
        /// <returns></returns>
        public static double DivSeconds(DateTime oldtime)
        {
            TimeSpan ts = DateTime.Now - oldtime;
            return ts.TotalSeconds;
        }

        /// <summary>
        /// 求时间与现在的秒数间隔
        /// </summary>
        /// <param name="oldtime"></param>
        /// <returns></returns>
        public static double DivSeconds(object oldtime)
        {
            DateTime dt;
            if (DateTime.TryParse(oldtime.ToString(), out dt))
            {
                return DivSeconds(dt);
            }
            return 1000;
        }

        #endregion
    }
}
