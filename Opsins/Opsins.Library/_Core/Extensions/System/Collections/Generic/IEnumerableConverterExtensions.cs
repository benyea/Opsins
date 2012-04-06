using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opsins
{
    /// <summary>
    /// IEnumerable转换扩展
    /// </summary>
    public static partial class IEnumerableExtensions
    {
        #region 转换所有数据

        /// <summary>
        /// 将集合中的每个元素转换为TResult类型
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source">集合源</param>
        /// <param name="converter">转换器</param>
        /// <returns></returns>

        public static IList<TResult> ConvertAll<TObject, TResult>(this IEnumerable<TObject> source, Func<TObject, TResult> converter)
        {
            return ConvertOnEach<TObject, TResult>(source, converter, null);
        }

        #endregion

        #region 转换规格

        /// <summary>
        /// 将集合中的符合predicate条件元素转换为TResult类型
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source">数据源</param>
        /// <param name="converter">数据结果</param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IList<TResult> ConvertOnEach<TObject, TResult>(this IEnumerable<TObject> source, Func<TObject, TResult> converter, Predicate<TObject> predicate)
        {
            IList<TResult> list = new List<TResult>();
            source.Each(ele => list.Add(converter(ele)), predicate);
            return list;
        }

        /// <summary>
        ///  将集合中的符合predicate条件的第一个元素转换为TResult类型
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="converter"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static TResult ConvertOnFirst<TObject, TResult>(this IEnumerable<TObject> source, Func<TObject, TResult> converter, Predicate<TObject> predicate) where TObject : class
        {
            TObject target = FindFirst<TObject>(source, predicate);

            if (target == null)
            {
                return default(TResult);
            }

            return converter(target);
        }

        #endregion

        #region 复制数据

        /// <summary>
        /// 将source所有规格元素复制到List
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IList<TObject> ToList<TObject>(this IEnumerable<TObject> source, Predicate<TObject> predicate)
        {
            IList<TObject> copy = new List<TObject>();
            Each<TObject>(source, copy.Add, predicate);
            return copy;
        }

        /// <summary>
        /// 复制到List
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IList<TObject> CopyAllToList<TObject>(IEnumerable<TObject> source)
        {
            IList<TObject> copy = new List<TObject>();
            source.Each<TObject>(copy.Add);
            return copy;
        }

        #endregion

        #region 转换为Dictionary

        /// <summary>
        /// 将集合中符合条件的对象添加到新的字典中。通过keySelector获取object对应的Key
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source">集合源</param>
        /// <param name="keySelector">键选择器</param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TElement> ToDictionary<TKey, TElement>(IEnumerable<TElement> source, Func<TElement, TKey> keySelector, Predicate<TElement> predicate)
        {
            return source.Where(element => predicate(element)).ToDictionary(keySelector);
        }

        #endregion
    }
}
