using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opsins
{
    /// <summary>
    /// Collection扩展
    /// </summary>
    public static class CollectionExtensions
    {
        #region 判断是否为NULL

        /// <summary>
        /// 判断集合是否为NULL
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source">集合源</param>
        /// <returns></returns>
        public static bool IsNull<TElement>(this ICollection<TElement> source)
        {
            return (source == null);
        }

        /// <summary>
        /// 判断集合是否为NULL或Empty
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source">集合源</param>
        /// <returns></returns>
        public static bool IsEmpty<TElement>(this ICollection<TElement> source)
        {
            return (source == null) || (source.Count == 0);
        }


        /// <summary>
        /// 判断集合是否为不NULL
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source">集合源</param>
        /// <returns></returns>
        public static bool IsNotNull<TElement>(this ICollection<TElement> source)
        {
            return (source != null);
        }

        /// <summary>
        /// 判断集合是否为不NULL或Empty
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source">集合源</param>
        /// <returns></returns>
        public static bool IsNotEmpty<TElement>(this ICollection<TElement> source)
        {
            return (source != null) && (source.Count > 0);
        }

        #endregion
    }
}
