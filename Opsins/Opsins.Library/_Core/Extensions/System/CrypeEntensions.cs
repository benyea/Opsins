using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opsins
{
    using Crypt;

    /// <summary>
    /// 加密方式
    /// </summary>
    public enum CryptMode
    {
        /// <summary>
        /// MD5
        /// </summary>
        Md5,
        /// <summary>
        /// HMAC
        /// </summary>
        Hmac
    }

    /// <summary>
    /// 扩展String加解密
    /// </summary>
    public static class CryptExtensions
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string Encrypt(this string target)
        {
            return Aes.Encrypt(Des.Encrypt(Aes.Encrypt(target)));
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string Decrypt(this string target)
        {
            return Aes.Decrypt(Des.Decrypt(Aes.Decrypt(target)));
        }

        /// <summary>
        /// 单重加密
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string EncryptSingle(this string target)
        {
            Checker.IsNotEmpty(target, "crypt agrs");
            return Aes.Encrypt(target.Trim());
        }

        /// <summary>
        /// 单重解密
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string DecryptSingle(this string target)
        {
            Checker.IsNotEmpty(target, "crypt agrs");
            return Aes.Decrypt(target.Trim());
        }

        /// <summary>
        /// 密码加密
        /// </summary>
        /// <param name="target"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Password(this string target, string key)
        {
            Checker.IsNotEmpty(target, "password");
            Checker.IsNotEmpty(key, "key");
            key = key.Trim().ToUpper();
            return Mse.Hmac.Maker(Mse.Hmac.Maker(Mse.Hmac.Maker(target.Trim(), "PASSWORD"), "SYSTEM"), key);
        }

        /// <summary>
        /// 签名加密
        /// </summary>
        /// <param name="target"></param>
        /// <param name="cryptMode">加密方式</param>
        /// <returns></returns>
        public static string Sign(this string target, CryptMode cryptMode=CryptMode.Md5)
        {
            if (cryptMode == CryptMode.Md5)
            {
                return target.Md5();
            }
            return target.Hmac("SYSTEM.SIGNIN.SIGN.ENCRYPT");
        }

        /// <summary>
        /// 签名加密，需要secretKey
        /// </summary>
        /// <param name="target"></param>
        /// <param name="secretKey">安全Key</param>
        /// <returns></returns>
        public static string Sign(this string target, string secretKey)
        {
            return target.Hmac(secretKey);
        }

        /// <summary>
        /// HMAC加密：系统默认加密方式
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string Hmac(this string target)
        {
            return target.Hmac("ENCRYPTION").Hmac("DEFAULT");
        }

        /// <summary>
        /// HMAC加密
        /// </summary>
        /// <param name="target"></param>
        /// <param name="key"></param>
        /// <param name="ignoreCase">true:忽略Key大小写，false:区分Key大小写</param>
        /// <returns></returns>
        public static string Hmac(this string target, string key, bool ignoreCase=false)
        {
            Checker.IsNotEmpty(target, "crypt args");
            Checker.IsNotEmpty(key, "key");
            if (ignoreCase)
                key = key.Trim().ToUpper();
            return Mse.Hmac.Maker(target.Trim(), key);
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string Md5(this string target)
        {
            Checker.IsNotEmpty(target, "crypt agrs");
            return Mse.Md5(target.Trim());
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="target">内容</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string Md5(this string target, Encoding encoding)
        {
            Checker.IsNotEmpty(target, "crypt agrs");
            return Mse.Md5(target.Trim(), encoding);
        }

        /// <summary>
        /// SHA加密
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string Sha(this string target)
        {
            Checker.IsNotEmpty(target, "crypt agrs");
            return Mse.Sha(target.Trim());
        }

        /// <summary>
        /// 验证码加密
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string Authcode(this string target)
        {
            return target.ToUpperInvariant().Hmac("ENCRYPTION").Hmac("AUTHCODE");
        }
    }
}
