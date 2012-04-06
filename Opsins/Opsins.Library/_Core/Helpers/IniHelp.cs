using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Opsins
{
    /// <summary>
    /// ini文件帮助
    /// </summary>
    public class IniHelp
    {
        public string FileName; //INI文件名 
        //声明读写INI文件的API函数 
        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);

        /// <summary>
        /// 类的构造函数，传递INI文件名 
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="dir">目录名，null为当前目录</param>
        public IniHelp(string fileName, string dir)
        {
            string path = "";
            if (!String.IsNullOrEmpty(dir))
            {

                System.IO.Directory.CreateDirectory(dir);
                path = dir + "\\";

            }
            string FileNamePath = path + fileName;
            // 判断文件是否存在 
            FileInfo fileInfo = new FileInfo(FileNamePath);
            //搞清枚举的用法 
            if ((!fileInfo.Exists))
            { //|| (FileAttributes.Directory in fileInfo.Attributes)) 
                //文件不存在，建立文件 

                System.IO.StreamWriter sw = new System.IO.StreamWriter(FileNamePath, false, System.Text.Encoding.Default);
                try
                {
                    sw.Write("#Application Configuration");
                    sw.Close();
                }
                catch
                {
                    throw (new ApplicationException("Ini文件不能创建"));
                }
            }
            //必须是完全路径，不能是相对路径 
            FileName = fileInfo.FullName;
        }
        /// <summary>
        /// 写INI文件
        /// </summary>
        /// <param name="section">主键</param>
        /// <param name="ident">属性</param>
        /// <param name="value">值</param>
        public void WriteString(string section, string ident, string value)
        {
            if (!WritePrivateProfileString(section, ident, value, FileName))
            {
                //抛出自定义的异常 
                throw (new ApplicationException("写Ini文件出错"));
            }
        }
        /// <summary>
        /// 读取INI文件指定 
        /// </summary>
        /// <param name="section">主键</param>
        /// <param name="ident">属性</param>
        /// <param name="defaultVal">如果错时返回指定值</param>
        /// <returns></returns>
        public string ReadString(string section, string ident, string defaultVal)
        {
            Byte[] Buffer = new Byte[65535];
            int bufLen = GetPrivateProfileString(section, ident, defaultVal, Buffer, Buffer.GetUpperBound(0), FileName);
            //必须设定0（系统默认的代码页）的编码方式，否则无法支持中文 
            string s = Encoding.GetEncoding(0).GetString(Buffer);
            s = s.Substring(0, bufLen);
            return s.Trim();
        }

        /// <summary>
        /// 读整数 
        /// </summary>
        /// <param name="section">主键</param>
        /// <param name="ident">属性</param>
        /// <param name="defaultVal">如果错时返回指定值</param>
        /// <returns></returns>
        public int ReadInteger(string section, string ident, int defaultVal)
        {
            string intStr = ReadString(section, ident, Convert.ToString(defaultVal));
            try
            {
                return Convert.ToInt32(intStr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return defaultVal;
            }
        }

        /// <summary>
        /// 写整数
        /// </summary>
        /// <param name="section">主键</param>
        /// <param name="ident">属性</param>
        /// <param name="value">值</param>
        public void WriteInteger(string section, string ident, int value)
        {
            WriteString(section, ident, value.ToString());
        }

        /// <summary>
        /// 读布尔
        /// </summary>
        /// <param name="section">主键</param>
        /// <param name="ident">属性</param>
        /// <param name="defaultVal">如果错时返回指定值</param>
        /// <returns></returns>
        public bool ReadBool(string section, string ident, bool defaultVal)
        {
            try
            {
                return Convert.ToBoolean(ReadString(section, ident, Convert.ToString(defaultVal)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return defaultVal;
            }
        }

        /// <summary>
        /// 写Bool 
        /// </summary>
        /// <param name="section">主键</param>
        /// <param name="ident">属性</param>
        /// <param name="value">值</param>
        public void WriteBool(string section, string ident, bool value)
        {
            WriteString(section, ident, Convert.ToString(value));
        }

        /// <summary>
        /// 从Ini文件中，将指定的Section名称中的所有Ident添加到列表中 
        /// </summary>
        /// <param name="section">主键</param>
        /// <param name="idents">属性</param>
        public void ReadSection(string section, StringCollection idents)
        {
            Byte[] Buffer = new Byte[16384];
            //Idents.Clear(); 

            int bufLen = GetPrivateProfileString(section, null, null, Buffer, Buffer.GetUpperBound(0),
              FileName);
            //对Section进行解析 
            GetStringsFromBuffer(Buffer, bufLen, idents);
        }

        private void GetStringsFromBuffer(Byte[] buffer, int bufLen, StringCollection strings)
        {
            strings.Clear();
            if (bufLen != 0)
            {
                int start = 0;
                for (int i = 0; i < bufLen; i++)
                {
                    if ((buffer[i] == 0) && ((i - start) > 0))
                    {
                        String s = Encoding.GetEncoding(0).GetString(buffer, start, i - start);
                        strings.Add(s);
                        start = i + 1;
                    }
                }
            }
        }
        /// <summary>
        /// 从Ini文件中，读取所有的Sections的名称 
        /// </summary>
        /// <param name="sectionList"></param>
        public void ReadSections(StringCollection sectionList)
        {
            //必须得用Bytes来实现，StringBuilder只能取到第一个Section 
            byte[] Buffer = new byte[65535];
            int bufLen = 0;
            bufLen = GetPrivateProfileString(null, null, null, Buffer,
              Buffer.GetUpperBound(0), FileName);
            GetStringsFromBuffer(Buffer, bufLen, sectionList);
        }
        /// <summary>
        /// 读取指定的Section的所有Value到列表中 
        /// </summary>
        /// <param name="section">主键</param>
        /// <param name="values">值</param>
        public void ReadSectionValues(string section, NameValueCollection values)
        {
            StringCollection KeyList = new StringCollection();
            ReadSection(section, KeyList);
            values.Clear();
            foreach (string key in KeyList)
            {
                values.Add(key, ReadString(section, key, ""));
            }
        }
        /// <summary>
        /// 清除某个Section 
        /// </summary>
        /// <param name="section">主键</param>
        public void EraseSection(string section)
        {
            if (!WritePrivateProfileString(section, null, null, FileName))
            {
                throw (new ApplicationException("无法清除Ini文件中的Section"));
            }
        }
        /// <summary>
        /// 删除某个Section下的键 
        /// </summary>
        /// <param name="section">主键</param>
        /// <param name="ident">属性</param>
        public void DeleteKey(string section, string ident)
        {
            WritePrivateProfileString(section, ident, null, FileName);
        }


        /// <summary>
        /// 对于Win9X，来说需要实现UpdateFile方法将缓冲中的数据写入文件 
        ///在Win NT, 2000和XP上，都是直接写文件，没有缓冲，所以，无须实现UpdateFile 
        ///执行完对Ini文件的修改之后，应该调用本方法更新缓冲区。 
        /// </summary>
        public void UpdateFile()
        {
            WritePrivateProfileString(null, null, null, FileName);
        }

        /// <summary>
        /// 检查某个Section下的某个键值是否存在 
        /// </summary>
        /// <param name="section">主键</param>
        /// <param name="ident">属性</param>
        /// <returns></returns>
        public bool ExistsValue(string section, string ident)
        {
            // 
            StringCollection Idents = new StringCollection();
            ReadSection(section, Idents);
            return Idents.IndexOf(ident) > -1;
        }

        //确保资源的释放 
        ~IniHelp()
        {
            UpdateFile();
        }
    }
}
