using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opsins
{
    /// <summary>
    /// IEnumerable扩展
    /// </summary>
    public static partial class IEnumerableExtensions
    {
        #region 判断是否为NULL

        /// <summary>
        /// 判断集合是否为NULL
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source">集合源</param>
        /// <returns></returns>
        public static bool IsNull<TElement>(this IEnumerable<TElement> source)
        {
            return (source == null);
        }

        /// <summary>
        /// 判断集合是否为NULL或Empty
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source">集合源</param>
        /// <returns></returns>
        public static bool IsEmpty<TElement>(this IEnumerable<TElement> source)
        {
            return (source == null) || (!source.GetEnumerator().MoveNext());
        }


        /// <summary>
        /// 判断集合是否为不NULL
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source">集合源</param>
        /// <returns></returns>
        public static bool IsNotNull<TElement>(this IEnumerable<TElement> source)
        {
            return (source != null);
        }

        /// <summary>
        /// 判断集合是否为不NULL或Empty
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source">集合源</param>
        /// <returns></returns>
        public static bool IsNotEmpty<TElement>(this IEnumerable<TElement> source)
        {
            return (source != null) && (source.GetEnumerator().MoveNext());
        }

        #endregion

        #region 选取元素

        /// <summary>
        /// 从集合中选取符合条件的元素
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source">集合源</param>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        public static IList<TElement> Find<TElement>(this IEnumerable<TElement> source, Predicate<TElement> predicate)
        {
            IList<TElement> list = new List<TElement>();
            Each(source, list.Add, predicate);
            return list;
        }

        #endregion

        #region 返回符合条件的第一个元素

        /// <summary>
        /// 返回符合条件的第一个元素
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source">数据源</param>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        public static TElement FindFirst<TElement>(this IEnumerable<TElement> source, Predicate<TElement> predicate)
        {
            foreach (TElement element in source)
            {
                if (predicate(element))
                {
                    return element;
                }
            }

            return default(TElement);
        }

        #endregion

        #region 是否包含满足条件的元素

        /// <summary>
        /// 集合中是否包含满足predicate条件的元素
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source">集合源</param>
        /// <param name="predicate">条件</param>
        /// <param name="specification"></param>
        /// <returns></returns>
        public static bool Contains<TElement>(this IEnumerable<TElement> source, Predicate<TElement> predicate, out TElement specification)
        {
            specification = default(TElement);
            foreach (TElement element in source)
            {
                if (predicate(element))
                {
                    specification = element;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 集合中是否包含满足predicate条件的元素
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source">集合源</param>
        /// <param name="predicate">条件</param>
        /// <returns></returns>

        public static bool Contains<TElement>(this IEnumerable<TElement> source, Predicate<TElement> predicate)
        {
            TElement specification;
            return Contains<TElement>(source, predicate, out specification);
        }

        #endregion

        #region 集合元素执行操作

        /// <summary>
        /// 执行集合中的每个元素
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void Each<TElement>(this IEnumerable<TElement> source, Action<TElement> action)
        {
            if (source == null) return;

            foreach (TElement obj in source)
            {
                action(obj);
            }
        }

        /// <summary>
        /// 对集合中满足predicate条件的元素执行action。如果没有条件，predicate传入null。
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source">集合源</param>
        /// <param name="action">执行方法</param>
        /// <param name="predicate">条件</param>
        public static void Each<TElement>(this IEnumerable<TElement> source, Action<TElement> action, Predicate<TElement> predicate)
        {
            if (source == null) { return; }

            if (predicate == null)
            {
                foreach (TElement obj in source)
                {
                    action(obj);
                }

                return;
            }

            foreach (TElement obj in source)
            {
                if (predicate(obj))
                {
                    action(obj);
                }
            }
        }

        #endregion

        #region 转换为字符串

        /// <summary>
        /// 转为以separator分隔符链接的一个字符串
        /// </summary>
        /// <param name="source"></param>
        /// <param name="separator">分隔符,默认为","</param>
        /// <returns></returns>
        public static string Join(this IEnumerable<string> source,string separator=",")
        {
            return string.Join(separator, source);
        }

        #endregion
    }
}
