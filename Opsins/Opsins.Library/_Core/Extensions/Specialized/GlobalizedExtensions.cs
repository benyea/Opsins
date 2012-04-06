using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Opsins.Specialized
{
    /// <summary>
    /// Globalized扩展
    /// </summary>
    public static class GlobalizedExtensions
    {
        #region 对象转换字符串

        /// <summary>
        /// 转为全球化的字符串
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns></returns>
        public static string ToGlobalizedString(this object o)
        {
            if (o == null)
            {
                return string.Empty;
            }
            if (o is DateTime)
            {
                return ((DateTime)o).ToGlobalizedDateTimeString();
            }
            if (o.GetType().IsNumericType())
            {
                var enusCulture = new CultureInfo("en-us", false);
                return ((IFormattable)o).ToString("", enusCulture.NumberFormat);
            }
            return o.ToString();
        }

        #endregion

        #region 日期时间全球化

        /// <summary>
        /// 转为全球化的日期时间字符串
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        /// <returns></returns>
        public static string ToGlobalizedDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("s");
        }

        /// <summary>
        /// 转为全球化的日期字符串
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        /// <returns></returns>
        public static string ToGlobalizedDateString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 转为全球化的时间字符串
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        /// <returns></returns>
        public static string ToGlobalizedTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("HH:mm:ss");
        }

        #endregion
    }
}
