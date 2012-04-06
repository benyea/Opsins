using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Opsins
{
    /// <summary>
    /// object扩展
    /// </summary>
    public static class ObjectExtensions
    {
        #region 对象判断

        /// <summary>
        /// 对象是否为NULL
        /// </summary>
        /// <param name="instance">对象实例</param>
        /// <returns></returns>
        public static bool IsNull(this object instance)
        {
            return (instance == null);
        }

        /// <summary>
        /// 对象是否不为NULL
        /// </summary>
        /// <param name="instance">对象实例</param>
        /// <returns></returns>
        public static bool IsNotNull(this object instance)
        {
            return (instance != null);
        }

        /// <summary>
        /// 如果对象为null则调用函数委托并返回函数委托的返回值。否则返回对象本身
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="func">对象为null时用于调用的函数委托</param>
        /// <returns>如果对象不为null则返回对象本身，否则返回<paramref name="func"/>函数委托的返回值</returns>
        /// <example>
        /// <code>
        /// string v = null;
        /// string d = v.IfNull(()=>"v is null");  //d = "v is null";
        /// string t = d.IfNull(() => "d is null");              //t = "v is null";
        /// </code>
        /// </example>
        public static T IfNull<T>(this T obj, Func<T> func) where T : class
        {
            if (obj == null)
            {
                return func == null ? default(T) : func();
            }
            return obj;
        }

        #endregion

        #region 类型转换

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string TryToString(this object target)
        {
            if (target == null)
            {
                return string.Empty;
            }
            return target.ToString();
        }

        /// <summary>
        /// 转换值为某类型
        /// </summary>
        /// <param name="target"></param>
        /// <param name="pInfo"></param>
        /// <returns></returns>
        public static object ConvertTo(this string target, Type pInfo)
        {
            try
            {
                if (!string.IsNullOrEmpty(target))
                {
                    var obj = Convert.ChangeType(target, pInfo);
                    return obj;
                }
            }
            catch { }
            return null;
        }

        /// <summary>
        /// 转化值为某类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(this object o)
        {
            try
            {
                if (o == null || o == DBNull.Value)
                {
                    return (T)default(T);
                }
                return (T)Convert.ChangeType(o, typeof(T));
            }
            catch
            {
                return (T)default(T);
            }
        }

        /// <summary>
        /// 转化值为某类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(this object o, T defaultValue)
        {
            try
            {
                if (o == null || o == DBNull.Value)
                {
                    return (T)Convert.ChangeType(defaultValue, typeof(T));
                }
                return (T)Convert.ChangeType(o, typeof(T));
            }
            catch
            {
                return (T)Convert.ChangeType(defaultValue, typeof(T));
            }
        }

        #endregion

        #region Xml序列化

        /// <summary>
        /// 将对象序列化到XML文件
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="fileName">Name of the file.</param>
        public static void SerializeToXmlFile(this object o, string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(o.GetType());
            //DirectoryHelper.EnsureExists(Path.GetDirectoryName(fileName));
            string directory = Path.GetDirectoryName(fileName);
            if(directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                serializer.Serialize(stream, o);
            }
        }
        /// <summary>
        /// 从XML文件反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static T DeserializeFromXmlFile<T>(string fileName)
        {
            T o;
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                o = (T)serializer.Deserialize(stream);
            }
            return o;
        }

        /// <summary>
        /// XML序列化
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static string SerializeToXml(this object o)
        {
            XmlSerializer serializer = new XmlSerializer(o.GetType());
            StringBuilder stringBuilder = new StringBuilder();
            using (TextWriter textWriter = new StringWriter(stringBuilder))
            {
                serializer.Serialize(textWriter, o);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// XML反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        public static T DeserializeFromXml<T>(string xml)
        {
            return (T)DeserializeFromXml(xml, typeof(T));

        }

        /// <summary>
        /// XML反序列化
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static object DeserializeFromXml(string xml, Type type)
        {
            object o;
            XmlSerializer serializer = new XmlSerializer(type);
            using (TextReader textReader = new StringReader(xml))
            {
                o = serializer.Deserialize(textReader);
            }
            return o;
        }

        #endregion 
    }
}
