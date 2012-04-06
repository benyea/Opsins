using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Opsins
{
    using Specialized;

    /// <summary>
    /// Dictionary扩展
    /// </summary>
    public static class DictionaryExtensions
    {
        #region 获取字典值

        /// <summary>
        /// 根据键名获取字典值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="d"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(this IDictionary d, string key)
        {
            object result = d[key];
            if (result == null) return default(T);
            T converted = result.ConvertTo<T>();
            return converted;
        }


        /// <summary>
        /// 根据键名获取字典值或默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="d"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetOrDefault<T>(this IDictionary d, string key, T defaultValue)
        {
            if (!d.Contains(key)) return defaultValue;

            return Get<T>(d, key);
        }


        /// <summary>
        /// 获取部分关键值
        /// </summary>
        /// <param name="d"></param>
        /// <param name="sectionName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(this IDictionary d, string sectionName, string key)
        {
            if (!d.Contains(sectionName)) return null;
            IDictionary section = d[sectionName] as IDictionary;
            if (!section.Contains(key)) return null;
            return section[key];
        }


        /// <summary>
        /// 获取部分关键值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="d"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(this IDictionary d, string section, string key)
        {
            object result = Get(d, section, key);
            if (result == null) return default(T);

            T converted = result.ConvertTo<T>();
            return converted;
        }


        /// <summary>
        /// 获取部分关键值或默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="d"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetOrDefault<T>(this IDictionary d, string section, string key, T defaultValue)
        {
            if (string.IsNullOrEmpty(section)) return defaultValue;

            // Validate and return default value.
            if (!d.Contains(section, key)) return defaultValue;
            return Get<T>(d, section, key);
        }


        /// <summary>
        /// 获取一个切片字典
        /// </summary>
        /// <param name="d"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public static IDictionary Section(this IDictionary d, string section)
        {
            if (d == null || d.Count == 0) return null;

            if (d.Contains(section))
                return d[section] as IDictionary;

            return null;
        }


        /// <summary>
        /// 是否包含键值
        /// </summary>
        /// <param name="d"></param>
        /// <param name="sectionName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Contains(this IDictionary d, string sectionName, string key)
        {
            IDictionary section = Section(d, sectionName);
            if (section == null) return false;

            return section.Contains(key);
        }

        #endregion

        #region 获取值

        /// <summary>
        /// 获取字符型键值
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="source">字典源</param>
        /// <param name="key">键名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetString<TK, TV>(this IDictionary<TK, TV> source, TK key, string defaultValue = null)
        {
            if (source.ContainsKey(key) && source[key] != null)
            {
                return source[key].ToString();
            }
            return defaultValue;
        }

        /// <summary>
        /// 获取布尔型键值
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="source">字典源</param>
        /// <param name="key">键名</param>
        /// <param name="defaultValue">设置<c>true</c> [默认值].</param>
        /// <returns></returns>
        public static bool GetBoolean<TK, TV>(this IDictionary<TK, TV> source, TK key, bool defaultValue = false)
        {
            bool value = defaultValue;
            if (source.ContainsKey(key) && source[key] != null)
            {
                bool.TryParse(source[key].ToString(), out value);
            }
            return value;
        }

        /// <summary>
        /// 获取布尔型键值
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="source">字典源</param>
        /// <param name="key">键名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static bool? GetBoolean<TK, TV>(this IDictionary<TK, TV> source, TK key, bool? defaultValue = false)
        {
            if (source.ContainsKey(key) && source[key] != null)
            {
                return bool.Parse(source[key].ToString());
            }
            return defaultValue;
        }

        /// <summary>
        /// 获取整数型键值
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="source">字典源</param>
        /// <param name="key">键名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static int GetInt<TK, TV>(this IDictionary<TK, TV> source, TK key, int defaultValue = -1)
        {
            int value = defaultValue;
            if (source.ContainsKey(key) && source[key] != null)
            {
                bool parse = int.TryParse(source[key].ToString(), out value);
                if (parse) return value;

                return defaultValue;
            }
            return value;
        }

        /// <summary>
        /// 获取键值
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">字典源</param>
        /// <param name="key">键名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static T Get<TK, TV, T>(this IDictionary<TK, TV> source, TK key, T defaultValue = null)
            where T : class
        {
            if (source.ContainsKey(key) && source[key] is T)
            {
                return source[key] as T;
            }
            return defaultValue;
        }

        #endregion

        #region 合并字典

        /// <summary>
        /// 合并字典
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
        {
            source[key] = value;
            return source;
        }

        /// <summary>
        /// 合并字典
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="mergedDictionary"></param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> source, IDictionary<TKey, TValue> mergedDictionary)
        {
            if (mergedDictionary != null)
            {
                foreach (var item in mergedDictionary)
                {
                    if (!source.ContainsKey(item.Key))
                    {
                        source.Add(item);
                    }
                }
            }
            return source;
        }

        #endregion

        #region 添加值

        /// <summary>
        /// 尝试将键和值添加到字典中：如果不存在，才添加；存在，不添加也不抛导常
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source">字典源</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
        {
            if (source.ContainsKey(key) == false) source.Add(key, value);
            return source;
        }

        /// <summary>
        /// 将键和值添加或替换到字典中：如果不存在，则添加；存在，则替换
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source">字典源</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> AddOrReplace<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value, TValue defaultValue = default(TValue))
        {
            source[key] = !value.IsNull() ? value : defaultValue;
            return source;
        }

        /// <summary>
        /// 向字典中批量添加键值对
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source">字典源</param>
        /// <param name="values"></param>
        /// <param name="replaceExisted">如果已存在，是否替换[默认替换]</param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> AddRange<TKey, TValue>(this IDictionary<TKey, TValue> source, IEnumerable<KeyValuePair<TKey, TValue>> values, bool replaceExisted = true)
        {
            foreach (KeyValuePair<TKey, TValue> item in values)
            {
                if (source.ContainsKey(item.Key) == false || replaceExisted)
                {
                    source[item.Key] = item.Value;
                }
            }
            return source;
        }

        #endregion

        #region 序列化

        /// <summary>
        /// 序列化为XML.
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="source">字典源</param>
        /// <returns></returns>
        public static string SerializeToXml<TK, TV>(this IDictionary<TK, TV> source)
        {
            List<KeyValuePair<TK, TV>> list = new List<KeyValuePair<TK, TV>>();
            foreach (var item in source)
            {
                list.Add(item);
            }
            return null;// list.SerializeToXml();
        }

        /// <summary>
        /// 反序列化XML字符串
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static IDictionary<TK, TV> DeserializeFromXml<TK, TV>(string xml)
        {
            List<KeyValuePair<TK, TV>> list = null; // ObjectExtensions.DeserializeFromXmlString<List<KeyValuePair<K, V>>>(xml);
            Dictionary<TK, TV> dic = new Dictionary<TK, TV>();
            foreach (var item in list)
            {
                dic.Add(item.Key, item.Value);
            }
            return dic;
        }

        #endregion

        #region 复制

        /// <summary>
        /// 复制
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="source">字典源</param>
        /// <param name="target">目标字典</param>
        public static void CopyTo<TK, TV>(this IDictionary<TK, TV> source, IDictionary<TK, TV> target)
        {
            foreach (var key in source.Keys)
            {
                if (!target.ContainsKey(key))
                {
                    target.Add(key, source[key]);
                }
            }
        }

        #endregion

        #region 空值判断

        /// <summary>
        /// 是否包含空值
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        /// 	<c>true</c> if [is value empty] [the specified source]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsContainValueEmpty(this IDictionary<string, string> source)
        {
            bool isBlank = true;
            foreach (var value in source.Values)
            {
                if (value.IsNotEmpty())
                {
                    isBlank = true;
                }
                else
                {
                    isBlank = false;
                    break;
                }
            }
            return isBlank;
        }

        #endregion

        #region 字典转换

        /// <summary>
        /// 转换为NameValueCollection
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="source">字典源</param>
        public static NameValueCollection ToNameValueCollection<TK, TV>(this IDictionary<TK, TV> source)
        {
            if (source == null)
            {
                return null;
            }
            NameValueCollection nameValues = new NameValueCollection();
            foreach (var value in source)
            {
                nameValues.Add(value.Key.ToString(), (value.Value == null || value.Value is DBNull) ? "" : value.Value.ToGlobalizedString());
            }
            return nameValues;
        }

        /// <summary>
        /// 转换为键值字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string ToKeyValueString<T, TV>(this IDictionary<T, TV> dic)
        {
            if (dic == null)
            {
                return null;
            }
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var key in dic.Keys)
            {
                stringBuilder.AppendFormat("{0}:{1},", key, dic[key]);
            }
            stringBuilder.RemoveLastChar(',');
            return stringBuilder.ToString();
        }

        #endregion

        #region 排序

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static SortedDictionary<TKey, TValue> Sort<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            return new SortedDictionary<TKey, TValue>(dict);
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static SortedDictionary<TKey, TValue> Sort<TKey, TValue>(this Dictionary<TKey, TValue> dict, IComparer<TKey> comparer)
        {
            return new SortedDictionary<TKey, TValue>(dict, comparer);
        }

        #endregion
    }
}
