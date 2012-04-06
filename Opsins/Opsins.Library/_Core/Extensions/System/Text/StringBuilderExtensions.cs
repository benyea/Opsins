using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opsins
{
    /// <summary>
    /// StringBuilder扩展
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// 移除最后字符
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="c">指定移除的字符</param>
        public static StringBuilder RemoveLastChar(this StringBuilder stringBuilder, char c)
        {
            if (stringBuilder.Length == 0)
                return stringBuilder;
            if (stringBuilder[stringBuilder.Length - 1] == c)
            {
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
            }
            return stringBuilder;
        }
    }
}
