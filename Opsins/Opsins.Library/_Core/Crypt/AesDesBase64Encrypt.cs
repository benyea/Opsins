using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Opsins.Crypt
{
    /// <summary>
    /// 加密键
    /// </summary>
    public class ADBKey
    {
        //private static string _key = @")O[NB]6,YF}+efcaj{+oESb9d8>Z'e9M";
        private static string _key = @")O[NB]6,YF}+efcaj{+oESb9d8>Z'e9M";
        public static string Key
        {
            get
            {
                string key = AppsetHelper.GetString("ADBKey");
                if (key.IsNotEmpty())
                    _key = key;
                return _key;
            }
            set { _key = value; }
        }
    }

    /// <summary>
    /// AES加密
    /// </summary>
    public class Aes
    {
        /// <summary>
        /// 获取向量
        /// </summary>
        private static string Iv
        {
            get { return @"L+\~f4,Ir)b$=pkf"; }
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainStr">明文字符串</param>
        /// <returns>密文</returns>
        public static string Encrypt(string plainStr)
        {
            byte[] bKey = Encoding.UTF8.GetBytes(ADBKey.Key);
            byte[] bIV = Encoding.UTF8.GetBytes(Iv);
            byte[] byteArray = Encoding.UTF8.GetBytes(plainStr);

            string encrypt = null;
            Rijndael aes = Rijndael.Create();
            try
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        encrypt = Convert.ToBase64String(mStream.ToArray());
                    }
                }
            }
            catch { }
            aes.Clear();

            return encrypt;
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainStr">明文字符串</param>
        /// <param name="returnNull">加密失败时是否返回 null，false 返回 String.Empty</param>
        /// <returns>密文</returns>
        public static string Encrypt(string plainStr, bool returnNull)
        {
            string encrypt = Encrypt(plainStr);
            return returnNull ? encrypt : (encrypt == null ? String.Empty : encrypt);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encryptStr">密文字符串</param>
        /// <returns>明文</returns>
        public static string Decrypt(string encryptStr)
        {
            byte[] bKey = Encoding.UTF8.GetBytes(ADBKey.Key);
            byte[] bIV = Encoding.UTF8.GetBytes(Iv);
            byte[] byteArray = Convert.FromBase64String(encryptStr);

            string decrypt = null;
            Rijndael aes = Rijndael.Create();
            try
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        decrypt = Encoding.UTF8.GetString(mStream.ToArray());
                    }
                }
            }
            catch { }
            aes.Clear();

            return decrypt;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encryptStr">密文字符串</param>
        /// <param name="returnNull">解密失败时是否返回 null，false 返回 String.Empty</param>
        /// <returns>明文</returns>
        public static string Decrypt(string encryptStr, bool returnNull)
        {
            string decrypt = Decrypt(encryptStr);
            return returnNull ? decrypt : (decrypt == null ? String.Empty : decrypt);
        }
    }

    /// <summary> 
    /// DES加密
    /// </summary> 
    public static class Des
    {
        #region ========加密========

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Encrypt(string Text)
        {
            return Encrypt(Text, ADBKey.Key);
        }
        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Encrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        #endregion

        #region ========解密========


        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Decrypt(string Text)
        {
            return Decrypt(Text, ADBKey.Key);
        }
        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Decrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = Text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        #endregion
    }

    /// <summary>
    /// base64算法加密
    /// </summary>
    public static class Base64
    {
        /// <summary>
        /// 默认为UTF-8加密
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string Encrypt(string code)
        {
            return Encrypt("utf-8", code);
        }

        /// <summary> 
        /// 将字符串使用base64算法加密 
        /// </summary> 
        /// <param name="code_type">编码类型（编码名称） 
        /// * 代码页 名称 
        /// * 1200 "UTF-16LE"、"utf-16"、"ucs-2"、"unicode"或"ISO-10646-UCS-2" 
        /// * 1201 "UTF-16BE"或"unicodeFFFE" 
        /// * 1252 "windows-1252" 
        /// * 65000 "utf-7"、"csUnicode11UTF7"、"unicode-1-1-utf-7"、"unicode-2-0-utf-7"、"x-unicode-1-1-utf-7"或"x-unicode-2-0-utf-7" 
        /// * 65001 "utf-8"、"unicode-1-1-utf-8"、"unicode-2-0-utf-8"、"x-unicode-1-1-utf-8"或"x-unicode-2-0-utf-8" 
        /// * 20127 "us-ascii"、"us"、"ascii"、"ANSI_X3.4-1968"、"ANSI_X3.4-1986"、"cp367"、"csASCII"、"IBM367"、"iso-ir-6"、"ISO646-US"或"ISO_646.irv:1991" 
        /// * 54936 "GB18030"    
        /// </param> 
        /// <param name="code">待加密的字符串</param> 
        /// <returns>加密后的字符串</returns> 
        public static string Encrypt(string code_type, string code)
        {
            string encode = "";
            byte[] bytes = Encoding.GetEncoding(code_type).GetBytes(code);  //将一组字符编码为一个字节序列. 
            try
            {
                encode = Convert.ToBase64String(bytes);  //将8位无符号整数数组的子集转换为其等效的,以64为基的数字编码的字符串形式. 
            }
            catch
            {
                encode = code;
            }
            return encode;
        }

        /// <summary>
        /// 默认为UTF-8解密
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string Decrypt(string code)
        {
            return Decrypt("utf-8", code);
        }

        /// <summary> 
        /// 将字符串使用base64算法解密 
        /// </summary> 
        /// <param name="code_type">编码类型</param> 
        /// <param name="code">已用base64算法加密的字符串</param> 
        /// <returns>解密后的字符串</returns> 
        public static string Decrypt(string code_type, string code)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(code);  //将2进制编码转换为8位无符号整数数组. 
            try
            {
                decode = Encoding.GetEncoding(code_type).GetString(bytes);  //将指定字节数组中的一个字节序列解码为一个字符串。 
            }
            catch
            {
                decode = code;
            }
            return decode;
        }
    }
}
