using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Opsins
{
    /// <summary>
    /// XML系列化帮助
    /// </summary>
    public class SerializerHelper
    {
        #region 泛型方法
        /// <summary>
        /// 序列化到配置文件　
        /// </summary>
        /// <typeparam name="T">序列化此类型</typeparam>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="fn">键值</param>
        public static bool Save<T>(T obj, string fn) where T : class
        {
            bool success = false;

            var mySerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            string filepath = HttpContext.Current.Server.MapPath(fn);
            if (!File.Exists(filepath))
                File.Create(filepath);
            if ((File.GetAttributes(filepath)
                 & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                File.SetAttributes(filepath, FileAttributes.Archive);
            }
            using (var myWriter = new StreamWriter(filepath))
            {
                mySerializer.Serialize(myWriter, obj);
            }
            try
            {
                success = true;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("存储配置文件{0}时出错,编号:{1}->{2}", fn, 10359, e.Message));
            }

            return success;
        }

        /// <summary>
        /// 从配置文件反序列化
        /// </summary>
        /// <param name="fn">键</param>
        /// <returns></returns>
        public static T Load<T>(string fn) where T : class
        {
            string filepath = HttpContext.Current.Server.MapPath(fn);
            if (!File.Exists(filepath))
                File.Create(filepath);
            if ((File.GetAttributes(filepath)
                 & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                File.SetAttributes(filepath, FileAttributes.Archive);
            }
            var mySerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (var myFileStream = new StreamReader(HttpContext.Current.Server.MapPath(fn)).BaseStream)
            {
                return mySerializer.Deserialize(myFileStream) as T;
            }
        }
        #endregion

        #region 非泛型方法

        /// <summary>
        /// 序列化数组
        /// </summary>
        private static readonly Dictionary<int, XmlSerializer> SerializerDict = new Dictionary<int, XmlSerializer>();

        /// <summary>
        /// 获取序列化值
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static XmlSerializer GetSerializer(Type t)
        {
            int typeHash = t.GetHashCode();

            if (!SerializerDict.ContainsKey(typeHash))
                SerializerDict.Add(typeHash, new XmlSerializer(t));

            return SerializerDict[typeHash];
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="filename">文件路径</param>
        /// <returns></returns>
        public static object Load(Type type, string filename)
        {
            FileStream fs = null;
            try
            {
                // open the stream...
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                throw new XException("序列化失败！", ex);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }


        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="filename">文件路径</param>
        public static bool Save(object obj, string filename)
        {
            bool success = false;

            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(fs, obj);
                success = true;
            }
            catch (Exception ex)
            {
                throw new XException("序列化失败！", ex);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return success;
        }

        /// <summary>
        /// xml序列化成字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>xml字符串</returns>
        public static string Serialize(object obj)
        {
            string returnStr = "";
            XmlSerializer serializer = GetSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();
            XmlTextWriter xtw = null;
            StreamReader sr = null;
            try
            {
                xtw = new System.Xml.XmlTextWriter(ms, Encoding.UTF8);
                xtw.Formatting = System.Xml.Formatting.Indented;
                serializer.Serialize(xtw, obj);
                ms.Seek(0, SeekOrigin.Begin);
                sr = new StreamReader(ms);
                returnStr = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new XException("序列化失败！", ex);
            }
            finally
            {
                if (xtw != null)
                    xtw.Close();
                if (sr != null)
                    sr.Close();
                ms.Close();
            }
            return returnStr;
        }

        /// <summary>
        /// xml序列化成字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>xml字符串</returns>
        public static string Serialize(object obj, Encoding encoding)
        {
            string returnStr = "";
            XmlSerializer serializer = GetSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();
            XmlTextWriter xtw = null;
            StreamReader sr = null;
            try
            {
                xtw = new System.Xml.XmlTextWriter(ms, encoding);
                xtw.Formatting = System.Xml.Formatting.Indented;
                serializer.Serialize(xtw, obj);
                ms.Seek(0, SeekOrigin.Begin);
                sr = new StreamReader(ms);
                returnStr = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new XException("序列化失败！", ex);
            }
            finally
            {
                if (xtw != null)
                    xtw.Close();
                if (sr != null)
                    sr.Close();
                ms.Close();
            }
            return returnStr;

        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static object Deserialize(Type type, string xml)
        {
            byte[] b = System.Text.Encoding.UTF8.GetBytes(xml);
            try
            {
                XmlSerializer serializer = GetSerializer(type);
                return serializer.Deserialize(new MemoryStream(b));
            }
            catch (Exception ex)
            {
                throw new XException("序列化失败！", ex);
            }
        }

        #endregion

        #region 泛型序列化

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static object Deserialize<T>(string xml) where T : class
        {
            byte[] b = System.Text.Encoding.UTF8.GetBytes(xml);
            try
            {
                XmlSerializer serializer = GetSerializer(typeof(T));
                return serializer.Deserialize(new MemoryStream(b));
            }
            catch (Exception ex)
            {
                throw new XException("序列化失败！", ex);
            }
        }

        /// <summary>
        /// Function:将XML字符串返序列化成对象
        /// Author: Kenny
        /// CreateDate: 2010-06-10
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static List<T> Deserializes<T>(string xml)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            List<T> entities = new List<T>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            // 要被循环的节点名
            string nodeName = typeof(T).ToString().Split('.')[typeof(T).ToString().Split('.').Length - 1];

            XmlNodeList nodeList = xmlDoc.GetElementsByTagName(nodeName);

            foreach (XmlNode itemNodeP in nodeList)
            {
                T entity = Activator.CreateInstance<T>();

                foreach (XmlNode itemNodeS in itemNodeP.ChildNodes)
                {
                    foreach (PropertyInfo property in properties)
                    {
                        if (itemNodeS.Name == property.Name)
                        {
                            property.SetValue(entity, Convert.ChangeType(itemNodeS.InnerXml, property.PropertyType), null);
                            break;
                        }
                    }
                }

                entities.Add(entity);
            }

            return entities;
        }

        /// <summary>
        /// Function:将XML字符串返序列化成对象
        /// Author: Kenny
        /// CreateDate: 2010-06-10
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <param name="conditionName">筛选条件的节点名称</param>
        /// <param name="conditionValue">筛选条件的节点值</param>
        /// <returns></returns>
        public static List<T> Deserializes<T>(string xml, string conditionName, string conditionValue)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            List<T> entities = new List<T>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            // 要被循环的节点名
            string nodeName = typeof(T).ToString().Split('.')[typeof(T).ToString().Split('.').Length - 1];

            foreach (XmlNode itemNodeP in xmlDoc.GetElementsByTagName(nodeName))
            {
                T entity = Activator.CreateInstance<T>();

                // 是否添加
                bool isAdd = false;

                foreach (XmlNode itemNodeS in itemNodeP.ChildNodes)
                {
                    // 加以筛选条件
                    if (itemNodeS.Name == conditionName && itemNodeS.InnerXml == conditionValue)
                    {
                        isAdd = true;
                    }
                    foreach (PropertyInfo property in properties)
                    {
                        if (itemNodeS.Name == property.Name)
                        {
                            property.SetValue(entity, Convert.ChangeType(itemNodeS.InnerXml, property.PropertyType), null);
                            break;
                        }
                    }
                }
                if (isAdd)
                {
                    entities.Add(entity);
                }
            }
            return entities;
        }

        #endregion
    }
}
