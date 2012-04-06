using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Opsins
{
    /// <summary>
    /// NameValueCollection扩展
    /// </summary>
    public static class NameValueCollectionExtensions
    {
        #region 集合判断

        /// <summary>
        /// 是否为可空
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsNullable(this NameValueCollection source, string key)
        {
            bool isTrue;

            var strings = source.GetValues(key);
            return strings != null && (source != null
                                                     && strings != null
                                                     && bool.TryParse(strings[0], out isTrue)
                                                     && isTrue);
        }

        /// <summary>
        /// 是否为真可空
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool? IsTrueNullable(this NameValueCollection source, string key)
        {
            bool? isTrue = null;

            if (source != null && source.GetValues(key) != null)
            {
                bool isTrueValue;

                var strings = source.GetValues(key);
                if (strings != null && bool.TryParse(strings[0], out isTrueValue))
                    isTrue = isTrueValue; 
            }

            return isTrue;
        }

        #endregion

        #region 基本设置

        /// <summary>
        /// 设为只读
        /// </summary>
        /// <param name="source">集合</param>
        /// <param name="readOnly">设置<c>true</c> [只读].</param>
        public static void SetReadOnly(this NameObjectCollectionBase source, bool readOnly)
        {
            typeof(NameValueCollection).SetPropertyValue("IsReadOnly", source, readOnly);
        }

        #endregion

        #region 序列化操作

        /// <summary>
        /// 序列化键值集合为XML
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string SerializeToXml(this NameValueCollection source)
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            foreach (var key in source.AllKeys)
            {
                list.Add(new KeyValuePair<string, string>(key, source[key]));
            }
            return list.SerializeToXml();
        }

        /// <summary>
        /// 从XML反序列化键值集合
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static NameValueCollection DeserializeFromXml(string xml)
        {
            List<KeyValuePair<string, string>> list = ObjectExtensions.DeserializeFromXml<List<KeyValuePair<string, string>>>(xml);
            NameValueCollection nameValues = new NameValueCollection();
            foreach (var item in list)
            {
                nameValues.Add(item.Key, item.Value);
            }
            return nameValues;
        }

        #endregion

        #region 转换操作

        /// <summary>
        /// 转换为Dictionary
        /// </summary>
        /// <param name="source">集合</param>
        /// <returns></returns>
        public static IDictionary<string, object> ToDictionary(this NameValueCollection source)
        {
            if (source == null)
            {
                return null;
            }
            IDictionary<string, object> dic = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
            foreach (string key in source.Keys)
            {
                dic[key] = source[key];
            }
            return dic;
        }

        /// <summary>
        /// 转化为字符串
        /// </summary>
        /// <param name="source">集合</param>
        /// <returns></returns>
        public static string ToKeyValueString(this NameValueCollection source)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (string key in source.Keys)
            {
                stringBuilder.AppendFormat("{0}:{1},", key, source[key]);
            }
            stringBuilder.RemoveLastChar(',');
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 转换为GET提交参数字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToQueryString(this NameValueCollection source)
        {
            if (source.Count > 0)
            {
                StringBuilder qs = new StringBuilder();

                qs.Append("?");

                for (int i = 0; i < source.Count; i++)
                {
                    if (i > 0)
                        qs.Append("&");

                    qs.AppendFormat("{0}={1}", AntiXss.HtmlEncode(source.Keys[i]), AntiXss.HtmlEncode(source[i]));
                }

                return qs.ToString();
            }

            return string.Empty;
        }

        #endregion

        /// <summary>
        /// 获取键值或默认值
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetOrDefault(NameValueCollection collection, string key, string defaultValue)
        {
            if (collection == null) return defaultValue;

            string val = collection[key];
            if (string.IsNullOrEmpty(val))
                return defaultValue;

            return val;
        }


        /// <summary>
        /// 获取键值或默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static T GetOrDefault<T>(NameValueCollection collection, string key, T defaultValue)
        {
            if (collection == null) return defaultValue;

            string val = collection[key];
            if (string.IsNullOrEmpty(val))
                return defaultValue;

            return val.ConvertTo<T>();
        }
    }
}
