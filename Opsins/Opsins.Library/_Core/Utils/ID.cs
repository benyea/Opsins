using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opsins
{
    /// <summary>
    /// ID标识
    /// </summary>
    public static class ID
    {
        #region 流水号

        /// <summary>
        /// 生成20位流水号
        /// </summary>
        /// <returns></returns>
        public static string SetLsh()
        {
            return SetLsh19().ToString() + RandHelper.Number(1).ToString();
        }

        /// <summary>
        /// 生成带一位字符前缀的20位流水号
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string SetLsh(char prefix)
        {
            return prefix + SetLsh19().ToString();
        }

        /// <summary>
        /// 获取16位流水号
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string SetLsh16(this string target)
        {
            Guid guid = Guid.NewGuid();
            long i = 1;
            foreach (byte b in guid.ToByteArray())
            {
                i *= ((int)b + 1);
            }
            target = string.Format("{0:x}", i - DateTime.Now.Ticks);
            return target;
        }

        /// <summary>
        /// 获取19位数字流水号
        /// </summary>
        /// <returns></returns>
        private static long SetLsh19()
        {
            Guid guid = Guid.NewGuid();
            byte[] buffer = guid.ToByteArray();
            long target = BitConverter.ToInt64(buffer, 0);
            return target;
        }

        /// <summary>
        /// 获取20位时间格式的流水号["yyyyMMdd hhmmss ffff" + 2位随机数]
        /// </summary>
        /// <returns></returns>
        public static string SetLsh20()
        {
            string target = DateTime.Now.ToString("yyyyMMddhhmmssffff") + RandHelper.Number(2).ToString();
            return target;
        }

        /// <summary>
        /// 获取32位流水号
        /// </summary>
        /// <returns></returns>
        public static string SetLsh32()
        {
            long l = 0;
            string target = DateTime.Now.ToString("yyyyMMddhhmms").Substring(0, 13) + SetLsh19().ToString();
            return target;
        }

        #endregion
    }
}
