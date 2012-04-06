using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opsins
{
    /// <summary>
    /// bool扩展
    /// </summary>
    public static class BooleanExtensions
    {
        /// <summary>
        /// 将true转为1，false转为0
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int ToBinary(this bool b)
        {
            return b ? 1 : 0;
        }

        /// <summary>
        /// 执行true
        /// </summary>
        /// <param name="b"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool IfTrue(this bool b, Action action)
        {
            if (b)
            {
                action();
            }
            return b;
        }

        /// <summary>
        /// 执行false
        /// </summary>
        /// <param name="b"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool IfFalse(this bool b, Action action)
        {
            if (!b)
            {
                action();
            }
            return b;
        }
    }
}
