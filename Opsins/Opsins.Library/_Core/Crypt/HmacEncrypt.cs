using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Opsins.Crypt
{
    /// <summary>
    /// MD5 SHA 加密 哈希算法
    /// </summary>
    public partial class Mse
    {
        /// <summary>
        /// HMAC加密
        /// </summary>
        public class Hmac
        {
            /// <summary>
            /// 生成MD5
            /// </summary>
            /// <param name="str"></param>
            /// <returns></returns>
            private static string BuildMd5(string str)
            {
                byte[] b = Encoding.GetEncoding(1252).GetBytes(str);
                b = new MD5CryptoServiceProvider().ComputeHash(b);
                string result = string.Empty;
                foreach (int i in b)
                {
                    result += i.ToString("x").PadLeft(2, '0');
                }
                return result;
            }

            /// <summary>
            /// 十六进行转数组
            /// </summary>
            /// <param name="hexStr"></param>
            /// <returns></returns>
            private static byte[] HexStringToArray(string hexStr)
            {
                const string HEX = "0123456789ABCDEF";
                string str = hexStr.ToUpper();
                int len = str.Length;
                byte[] retByte = new byte[len / 2];
                for (int i = 0; i < len / 2; i++)
                {
                    int numHigh = HEX.IndexOf(str[i * 2]);
                    int numLow = HEX.IndexOf(str[i * 2 + 1]);
                    retByte[i] = Convert.ToByte(numHigh * 16 + numLow);
                }
                return retByte;
            }

            /// <summary>
            /// 字符串异或处理
            /// </summary>
            /// <param name="password"></param>
            /// <param name="pad"></param>
            /// <returns></returns>
            private static string StrXor(string password, string pad)
            {
                string iResult = string.Empty;
                int KLen = password.Length;

                for (int i = 0; i < 64; i++)
                {
                    if (i < KLen)
                        iResult += Convert.ToChar(pad[i] ^ password[i]);
                    else
                        iResult += Convert.ToChar(pad[i]);
                }
                return iResult;
            }

            /// <summary>
            /// 生成HMAC码
            /// </summary>
            /// <param name="data"></param>
            /// <param name="password"></param>
            /// <returns></returns>
            public static string Maker(string data, string password)
            {
                string k_ipad, k_opad, temp;
                string ipad = "CHINESE_ZHUHAI_WINDOWS_APPLICATION_PROGS_IS_A_VERY_GOOD_SOFTWARE";
                string opad = @"////////////////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\";
                k_ipad = BuildMd5(StrXor(password, ipad) + data);

                k_opad = StrXor(password, opad);

                byte[] Test = HexStringToArray(k_ipad);
                temp = string.Empty;

                char[] b = Encoding.GetEncoding(1252).GetChars(Test);
                for (int i = 0; i < b.Length; i++)
                {
                    temp += b[i];
                }
                temp = k_opad + temp;
                return BuildMd5(temp).ToUpper();
            }
        }
    }
}
