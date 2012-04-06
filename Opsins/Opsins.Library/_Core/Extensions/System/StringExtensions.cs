using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Opsins
{
    /// <summary>
    /// String扩展
    /// </summary>
    public static class StringExtensions
    {
        #region 空判断

        /// <summary>
        /// 判断字符串是否为NULL或Empty
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string target)
        {
            return string.IsNullOrEmpty(target);
        }

        /// <summary>
        /// 判断字符是否不为NULL或Empty
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsNotEmpty(this string target)
        {
            return !string.IsNullOrEmpty(target);
        }

        /// <summary>
        /// 判断字符串是否为NULL、空或由空白字符组成
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsWhiteSpace(this string target)
        {
            return string.IsNullOrWhiteSpace(target);
        }

        /// <summary>
        /// 判断字符串是否不为NULL、空或由空白字符组成
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsNotWhiteSpace(this string target)
        {
            return !string.IsNullOrWhiteSpace(target);
        }

        /// <summary>
        /// 如果字符串值为null或空字符串，则返回<paramref name="defaultValue"/>默认值
        /// </summary>
        /// <param name="text">字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string IfEmpty(this string text, string defaultValue)
        {
            return string.IsNullOrEmpty(text) ? defaultValue : text;
        }
        /// <summary>
        /// 如果字符串值为null或空字符串，则调用<paramref name="func"/>函数并返回<paramref name="func"/>函数返回值
        /// </summary>
        /// <param name="text">字符串</param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static string IfEmpty(this string text, Func<string> func)
        {
            return string.IsNullOrEmpty(text) ? func.Invoke() : text;
        }

        #endregion

        #region 字符串比较

        /// <summary>
        /// 字符串比较
        /// </summary>
        /// <param name="target"></param>
        /// <param name="s"></param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public static int Compare(this string target, string s, bool ignoreCase=false)
        {
            return string.Compare(target, s, ignoreCase);
        }

        #endregion

        #region 字符格式化

        /// <summary>
        /// 字符格式化
        /// </summary>
        /// <param name="target"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatWith(this string target, params object[] args)
        {
            return string.Format(target, args);
        }

        /// <summary>
        /// 字符格式化
        /// </summary>
        /// <param name="target"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static string FormatWith(this string target, object arg)
        {
            return string.Format(target, arg);
        }

        /// <summary>
        /// 字符格式化
        /// </summary>
        /// <param name="target"></param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public static string FormatWith(this string target, object arg0, object arg1)
        {
            return string.Format(target, arg0, arg1);
        }

        #endregion

        #region SQL字符处理

        /// <summary> 
        /// 防SQL注入 
        /// </summary> 
        /// <param name="target">要过滤的字符串 </param> 
        /// <returns>如果参数存在不安全字符，则返回true </returns> 
        [DebuggerStepThrough]
        public static bool AntiSql(this string target)
        {
            const string word = "%|]|[|'|;|--|(|)|*|{|}|!| alter | exec | execute | insert | select | delete | update | master| truncate | declare | xp_cmdshell";
            //const string word = "%|]|[|'|;|--|/|\\|(|)|*|{|}|!|and |alter |exec |execute |insert |select |delete |update |chr |mid |master |or |truncate |char |declare |join |cmd |xp_cmdshell";
            if (target == null)
                return false;

            string[] strArr = word.Split('|');
            return strArr.Any(s => (target.ToUpperInvariant().IndexOf(s, System.StringComparison.Ordinal) > -1) || (target.ToUpperInvariant().IndexOf(s, System.StringComparison.Ordinal) > -1));
        }

        /// <summary>
        /// 防SQL注入 
        /// </summary>
        /// <param name="strArr">字符串数组</param>
        /// <returns></returns>
        public static bool AntiSql(string[] strArr)
        {
            return strArr.Any(AntiSql);
        }

        #endregion

        #region 字符串操作

        /// <summary>
        /// 在字符串上追加字符
        /// </summary>
        /// <param name="target"></param>
        /// <param name="appendString">追加的字符[默认为","]</param>
        /// <param name="appendAtBack">在字符串后追加</param>
        /// <param name="appendAtFront">在字符串前追加</param>
        /// <returns></returns>
        public static string AppendTo(this string target, string appendString = ",", bool appendAtBack = true, bool appendAtFront = false)
        {
            var result = string.Empty;

            if (appendAtFront) result += appendString;
            result += target;
            if (appendAtBack) result += appendString;

            return result;
        }

        /// <summary>
        /// 字符串颠倒输出
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string Reverse(this string target)
        {
            char[] charstr = target.ToCharArray();
            Array.Reverse(charstr);
            return new string(charstr);
        }

        /// <summary>
        /// 字符串切片：返回字符串指定两个字符串中间的信息
        /// </summary>
        /// <param name="target">字符串</param>
        /// <param name="startString">开始段</param>
        /// <param name="endString">尾段</param>
        /// <returns></returns>
        public static string Slice(this string target, string startString, string endString)
        {
            if (target.IndexOf(startString, StringComparison.Ordinal) == -1) return "";
            if (target.IndexOf(endString, StringComparison.Ordinal) == -1) return "";

            string lastPart = target.Substring(target.IndexOf(startString, StringComparison.Ordinal) + startString.Length);

            if (lastPart.IndexOf(endString, StringComparison.Ordinal) == -1) return "";

            return lastPart.Substring(0, lastPart.IndexOf(endString, StringComparison.Ordinal));
        }

        /// <summary>
        /// 仅保留字符串中的数字字符
        /// </summary>
        /// <param name="target">字符串</param>
        /// <returns></returns>
        public static string RetainNumber(this string target)
        {
            string stroutput = target;

            Regex r = new Regex(@"\D*", RegexOptions.IgnoreCase);
            Match mc = r.Match(target);
            return target.Replace(mc.Groups[0].Value, "");
        }

        /// <summary>
        /// 清除中文
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string CleanChinese(this string target)
        {
            Regex regex = new Regex(@"[\u4e00-\u9fa5]");
            target = regex.Replace(target, "");
            return target.Trim();
        }

        /// <summary>
        /// 清除空格,换行字符
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string CleanSpace(this string target)
        {
            return target.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("  ", "");
        }

        /// <summary>
        /// 清除文本
        /// </summary>
        /// <param name="target">字符串</param>
        /// <returns></returns>
        public static string CleanText(this string target)
        {
            if (target == null) return null;

            return AntiXss.HtmlEncode(target);
        }

        /// <summary>
        /// 清除HTML
        /// </summary>
        /// <param name="target">字符串</param>
        /// <returns></returns>
        public static string CleanHtml(this string target)
        {
            string encodedText = AntiXss.HtmlEncode(target);
            return encodedText.Replace("&#13;&#10;", "<br />");
        }

        /// <summary>
        /// 清除UBB标记
        /// </summary>
        /// <param name="target">字符串</param>
        /// <returns></returns>
        public static string CleanUbb(this string target)
        {
            Regex regex = new Regex(@"\[[^[\[\]]*]");
            target = regex.Replace(target, "");
            return target.Trim();
        }

        /// <summary>
        /// 清除UBB和HTML
        /// </summary>
        /// <param name="target">字符串</param>
        /// <returns></returns>
        public static string CleanUbbHtml(this string target)
        {
            return target.CleanUbb().CleanHtml();
        }

        /// <summary>
        /// 清除QueryString
        /// </summary>
        /// <param name="target">字符串</param>
        /// <returns></returns>
        public static string CleanForQueryString(this string target)
        {
            return AntiXss.UrlEncode(target);
        }

        /// <summary>
        /// 清除Attribute
        /// </summary>
        /// <param name="target">字符串</param>
        /// <returns></returns>
        public static string CleanAttribute(this string target)
        {
            return AntiXss.HtmlAttributeEncode(target);
        }

        #endregion

        #region 截取字符串

        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替
        /// </summary>
        /// <param name="target"></param>
        /// <param name="sLength">指定长度</param>
        /// <param name="tailString">用于替换的字符串[默认为"..."]</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubstring(this string target, int sLength, string tailString = "...")
        {
            return GetSubstring(target, 0, sLength, tailString);
        }


        /// <summary>
        /// 取指定长度的字符串
        /// </summary>
        /// <param name="target">字符串</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="sLength">指定长度</param>
        /// <param name="tailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubstring(this string target, int startIndex, int sLength, string tailString)
        {
            #region 取指定长度的字符串

            string myResult = target;

            //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
            if (Regex.IsMatch(target, "[\u0800-\u4e00]+") || Regex.IsMatch(target, "[\xAC00-\xD7A3]+"))
            {
                //当截取的起始位置超出字段串长度时
                if (startIndex >= target.Length)
                {
                    return "";
                }
                return target.Substring(startIndex,
                                        ((sLength + startIndex) > target.Length) ? (target.Length - startIndex) : sLength);
            }


            if (sLength >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(target);

                //当字符串长度大于起始位置
                if (bsSrcString.Length > startIndex)
                {
                    int pEndIndex = bsSrcString.Length;

                    //当要截取的长度在字符串的有效长度范围内
                    if (bsSrcString.Length > (startIndex + sLength))
                    {
                        pEndIndex = sLength + startIndex;
                    }
                    else
                    {   //当不在有效范围内时,只取到字符串的结尾

                        sLength = bsSrcString.Length - startIndex;
                        tailString = "";
                    }

                    int nRealLength = sLength;
                    int[] anResultFlag = new int[sLength];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = startIndex; i < pEndIndex; i++)
                    {
                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                            {
                                nFlag = 1;
                            }
                        }
                        else
                        {
                            nFlag = 0;
                        }

                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[pEndIndex - 1] > 127) && (anResultFlag[sLength - 1] == 1))
                    {
                        nRealLength = sLength + 1;
                    }

                    bsResult = new byte[nRealLength];

                    Array.Copy(bsSrcString, startIndex, bsResult, 0, nRealLength);

                    myResult = Encoding.Default.GetString(bsResult);
                    myResult = myResult + tailString;
                }
            }
            return myResult;

            #endregion
        }

        /// <summary>
        /// 获取字符串标识符中的第几片段[默认为第一个片段]
        /// </summary>
        /// <param name="target">字符串</param>
        /// <param name="c">标识符</param>
        /// <param name="sliceNo">片段编号，从0开始计算</param>
        /// <returns></returns>
        public static string GetSubstring(this string target, char c, int sliceNo = 0)
        {
            if (target.IsEmpty()) return string.Empty;
            string[] array = target.Split(c);

            if (sliceNo < array.Length)
            {
                return array[sliceNo];
            }

            //确定最小为第一个片段
            if (sliceNo < 0) return array[0];
            //确定最大为最后一个片段
            return array[array.Length - 1];
        }

        /// <summary>
        /// 获取最后一个片段
        /// </summary>
        /// <param name="target"></param>
        /// <param name="c"></param>
        /// <param name="lastSlice"></param>
        /// <returns></returns>
        public static string GetSubstring(this string target, char c, bool lastSlice)
        {
            if (target.IsEmpty()) return string.Empty;
            string[] array = target.Split(c);

            return array[array.Length - 1];
        }

        /// <summary>
        /// 获取开始到结束的片段
        /// </summary>
        /// <param name="target"></param>
        /// <param name="c"></param>
        /// <param name="startSlice">开始片段</param>
        /// <param name="endSlice">结束片段</param>
        /// <returns></returns>
        public static string GetSubstring(this string target, char c, int startSlice, int endSlice)
        {
            if (target.IsEmpty()) return string.Empty;
            string[] array = target.Split(c);
            int arrlen = array.Length;

            if (startSlice >= 0 && endSlice < arrlen)
            {
                string result = string.Empty;
                for (int i = startSlice; i < endSlice; i++)
                {
                    result += array[i].ToString();
                }
                return result;
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取除最后片段的字符
        /// </summary>
        /// <param name="target"></param>
        /// <param name="c"></param>
        /// <param name="includeSeparateSign">包括分隔符</param>
        /// <returns></returns>
        public static string GetSubstringExcludeLastSlice(this string target, char c, bool includeSeparateSign = true)
        {
            if (target.IsEmpty()) return string.Empty;
            string result;

            string lastSlice = target.GetSubstring(c, true);
            if (!includeSeparateSign)
            {
                result = target.Replace(lastSlice, "");
            }
            else
            {
                result = target.Replace(string.Format("{0}{1}", c.ToString(), lastSlice), "");
            }
            return result;
        }

        #endregion

        #region 正则表达

        /// <summary>
        /// 调用Regex中IsMatch函数实现一般的正则表达式匹配
        /// </summary>
        /// <param name="target"> </param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <returns>如果正则表达式找到匹配项，则为返回true；否则返回false</returns>
        public static bool IsMatch(this string target, string pattern)
        {
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return !string.IsNullOrEmpty(pattern) && regex.IsMatch(target);
        }

        /// <summary>
        /// Regex.Match扩展
        /// </summary>
        /// <param name="target"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string Match(this string target, string pattern)
        {
            if (target == null) return "";
            return Regex.Match(target, pattern).Value;
        }

        #endregion

        #region 字符转换

        /// <summary>
        /// 转换为枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string target,T defaultValue=default(T)) where T : struct
        {
            T convertedValue = defaultValue;
            if(target.IsNotEmpty())
            {
                try
                {
                    bool result = Enum.TryParse(target, true, out convertedValue);
                    if(result)
                    {
                        return convertedValue;
                    }
                    convertedValue = defaultValue;
                }
                catch (ArgumentException)
                {
                    throw;
                }
            }

            return convertedValue;
        }

        #endregion
    }
}
