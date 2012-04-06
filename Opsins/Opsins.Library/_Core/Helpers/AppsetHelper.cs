using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Opsins
{
    /// <summary>
    /// AppSettings辅助方法
    /// </summary>
    public static class AppsetHelper
    {
        /// <summary>
        /// 返回Int32值,未设置返回默认值
        /// </summary>
        /// <param name="key">配置键名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static int GetInt32(string key, int defaultValue=0)
        {
            int result = 0;
            return int.TryParse(GetString(key), out result) ? result : defaultValue;
        }

        /// <summary>
        /// 返回Bool值，未设置返回默认值
        /// </summary>
        /// <param name="key">配置键名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static bool GetBool(string key, bool defaultValue=false)
        {
            string value = GetString(key);
            if (value.IsNotEmpty())
            {
                return bool.Parse(GetString(key));
            }
            return defaultValue;
        }

        /// <summary>
        /// 返回TimeSpan，未设置返回默认值
        /// </summary>
        /// <param name="key">配置键名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static TimeSpan GetTimespan(string key, TimeSpan defaultValue=default(TimeSpan))
        {
            string val = GetString(key);

            if (val == null)
                return defaultValue;

            return TimeSpan.Parse(val);
        }

        /// <summary>
        /// 返回String值，未设置返回默认值
        /// </summary>
        /// <param name="key">配置键名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetString(string key, string defaultValue=null)
        {
            string value = defaultValue;
            string keyVal = ConfigurationManager.AppSettings[key];
            if (keyVal.IsNotEmpty())
            {
                value = keyVal;
            }

            return value;
        }

        /// <summary>
        /// 获取设置值
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="key">配置键名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static TValue Get<TValue>(string key, TValue defaultValue)
        {
            TValue value = defaultValue;
            object keyValue = ConfigurationManager.AppSettings[key];
            if (keyValue != null)
            {
                value = keyValue.ConvertTo(defaultValue);
            }
            return value;
        }
    }
}
