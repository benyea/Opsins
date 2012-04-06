using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Opsins.IO
{
    /// <summary>
    /// Stream扩展
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// 将文件流转为字节数组
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this Stream stream)
        {
            byte[] buffer = new byte[1000];
            int readLength = 1;
            using (MemoryStream ms = new MemoryStream())
            {
                while (readLength > 0)
                {
                    readLength = stream.Read(buffer, 0, buffer.Length);
                    if (readLength > 0)
                        ms.Write(buffer, 0, readLength);
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 复制副本
        /// </summary>
        /// <param name="src">源</param>
        /// <param name="dest">目标</param>
        public static void CopyTo(this Stream src, Stream dest)
        {
            byte[] buffer = new byte[0x10000];
            int bytes;
            try
            {
                while ((bytes = src.Read(buffer, 0, buffer.Length)) > 0)
                {
                    dest.Write(buffer, 0, bytes);
                }
            }
            finally
            {
                dest.Flush();
            }
        }

        /// <summary>
        /// 读取为字符数组
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static byte[] ReadData(this Stream src)
        {
            byte[] data = new byte[src.Length];
            src.Read(data, 0, data.Length);

            return data;
        }

        /// <summary>
        /// 读取字符串,默认为UTF8编码
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string ReadString(this Stream src)
        {
            return src.ReadString(Encoding.UTF8);
        }

        /// <summary>
        /// 读取字符串
        /// </summary>
        /// <param name="src">文件流</param>
        /// <param name="encoding"> </param>
        /// <returns></returns>
        public static string ReadString(this Stream src,Encoding encoding)
        {
            src.Seek(0, SeekOrigin.Begin);
            TextReader reader = new StreamReader(src, encoding);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// 写入字符串
        /// </summary>
        /// <param name="src">文件流</param>
        /// <param name="s">字符串</param>
        public static void WriteString(this Stream src, string s)
        {
            TextWriter writer = new StreamWriter(src, Encoding.UTF8);
            writer.Write(s);
            writer.Flush();
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="filePath">文件路径</param>
        public static string SaveAs(this Stream stream, string filePath)
        {
            return SaveAs(stream, filePath, true);
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="isOverwrite">如果设置为<c>true</c> [覆盖写入].</param>
        public static string SaveAs(this Stream stream, string filePath, bool isOverwrite)
        {
            var data = new byte[stream.Length];
            var length = stream.Read(data, 0, (int)stream.Length);
            return data.SaveAs(filePath, isOverwrite);
        }
    }
}
