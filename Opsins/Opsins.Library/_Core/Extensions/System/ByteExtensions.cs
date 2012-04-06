using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Opsins
{
    /// <summary>
    /// Byte扩展
    /// </summary>
    public static class ByteExtensions
    {
        /// <summary>
        /// 转换为十六进制字符串
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string ToHex(this byte target)
        {
            return target.ToString("X2");
        }
        /// <summary>
        /// 转换为十六进制字符串
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string ToHex(this IEnumerable<byte> target)
        {
            var sb = new StringBuilder();
            foreach (byte b in target)
                sb.Append(b.ToHex());

            return sb.ToString();
        }
        /// <summary>
        /// 转换为Base64字符串
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string ToBase64(byte[] target)
        {
            return Convert.ToBase64String(target);
        }
        /// <summary>
        /// 转换为整形
        /// </summary>
        /// <param name="target"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static int ToInt(this byte[] target, int startIndex)
        {
            return BitConverter.ToInt32(target, startIndex);
        }
        /// <summary>
        /// 转换为长整型
        /// </summary>
        /// <param name="target"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static long ToInt64(this byte[] target, int startIndex)
        {
            return BitConverter.ToInt64(target, startIndex);
        }

        #region Hash

        /// <summary>
        /// 使用默认算法Hash
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static byte[] Hash(this byte[] target)
        {
            return Hash(target, null);
        }

        /// <summary>
        /// 使用指定算法Hash
        /// </summary>
        /// <param name="target"></param>
        /// <param name="hashName"></param>
        /// <returns></returns>
        public static byte[] Hash(this byte[] target, string hashName)
        {
            HashAlgorithm algorithm;
            if (hashName.IsEmpty()) algorithm = HashAlgorithm.Create();
            else algorithm = HashAlgorithm.Create(hashName);
            return algorithm.ComputeHash(target);
        }
        #endregion

        #region 位运算

        /// <summary>
        /// 获取第index是否为1
        /// index从0开始
        /// </summary>
        /// <param name="target"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool GetBit(this byte target, int index)
        {
            return (target & (1 << index)) > 0;
        }
        /// <summary>
        /// 将第index位设为1
        /// </summary>
        /// <param name="target"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static byte SetBit(this byte target, int index)
        {
            target |= (byte)(1 << index);
            return target;
        }
        /// <summary>
        /// 将第index位设为0
        /// </summary>
        /// <param name="target"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static byte ClearBit(this byte target, int index)
        {
            target &= (byte)((1 << 8) - 1 - (1 << index));
            return target;
        }
        /// <summary>
        /// 将第index位取反
        /// </summary>
        /// <param name="target"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static byte ReverseBit(this byte target, int index)
        {
            target ^= (byte)(1 << index);
            return target;
        }

        #endregion

        /// <summary>
        /// 保存为文件
        /// </summary>
        /// <param name="target"></param>
        /// <param name="path">保存路径</param>
        public static void SaveAs(this byte[] target, string path)
        {
            File.WriteAllBytes(path, target);
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="isOverwrite">如果设置为<c>true</c> [覆盖写入].</param>
        /// <returns>文件绝对路径</returns>
        public static string SaveAs(this byte[] data, string filePath, bool isOverwrite)
        {
            string directory = Path.GetDirectoryName(filePath);
            if (string.IsNullOrEmpty(directory)) return string.Empty;

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            if (!isOverwrite && File.Exists(filePath))
            {
                string fileNameWithoutEx = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);

                int i = 1;
                do
                {
                    filePath = Path.Combine(directory, string.Format("{0}-{1}{2}", fileNameWithoutEx, i, extension));
                    i++;
                } while (File.Exists(filePath));
            }
            using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fileStream.Write(data, 0, data.Length);
            }
            return filePath;
        }

        /// <summary>
        /// 转换为内存流
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static MemoryStream ToMemoryStream(this byte[] target)
        {
            return new MemoryStream(target);
        }
    }
}
