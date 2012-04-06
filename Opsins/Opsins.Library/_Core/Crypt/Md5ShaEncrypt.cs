using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace Opsins.Crypt
{
    /// <summary>
    /// MD5 SHA 加密 哈希算法
    /// </summary>
    public partial class Mse
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string Md5(string content)
        {
            if (!String.IsNullOrEmpty(content))
            {
                return FormsAuthentication.HashPasswordForStoringInConfigFile(content, "MD5");
            }
            return string.Empty;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string Md5(string content, Encoding encoding)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(encoding.GetBytes(content));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString().ToUpperInvariant();
        }

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string Sha(string content)
        {
            if (!String.IsNullOrEmpty(content))
            {
                var hashPasswordForStoringInConfigFile = FormsAuthentication.HashPasswordForStoringInConfigFile(content, "SHA1");
                if (hashPasswordForStoringInConfigFile != null)
                {
                    return hashPasswordForStoringInConfigFile.ToLower();
                }
            }
            return string.Empty;
        }
    }
}
