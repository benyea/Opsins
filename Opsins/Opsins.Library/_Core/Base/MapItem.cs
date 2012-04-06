using System;

namespace Opsins
{
    /// <summary>
    /// 映射项
    /// </summary>
    /// <typeparam name="TSource">映射源</typeparam>
    /// <typeparam name="TTarget">映射目标</typeparam>
    public class MapItem<TSource, TTarget>
    {
        //构造函数
        public MapItem() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public MapItem(TSource source, TTarget target)
        {
            Source = source;
            Target = target;
        }

        /// <summary>
        /// 映射源
        /// </summary>
        public TSource Source { get; set; }
        /// <summary>
        /// 映射目标
        /// </summary>
        public TTarget Target { get; set; }
    }
}
