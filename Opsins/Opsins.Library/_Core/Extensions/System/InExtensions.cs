using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opsins
{
    /// <summary>
    /// 扩展In[对象是否存在的判断]
    /// </summary>
    public static class InExtensions
    {
        /// <summary>
        /// 判断对象是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool In<T>(this T t, params T[] obj)
        {
            if (obj != null)
            {
                return obj.Any(i => i.Equals(t));
            }
            return false;
        }
    }
}
