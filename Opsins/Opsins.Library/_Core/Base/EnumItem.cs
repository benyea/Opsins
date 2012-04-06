using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opsins
{
    /// <summary>
    /// 枚举项
    /// </summary>
    public class EnumItem
    {
        public EnumItem()
        {
            Value = 0;
            Tag = null;
            Show = true;
            Sort = 0;
        }

        /// <summary>
        /// 枚举描述
        /// </summary>
        public string Descript { get; set; }
        /// <summary>
        /// 枚举名
        /// </summary>
        public object Name { get; set; }
        /// <summary>
        /// 枚举值
        /// </summary>
        public int Value { get; set; }
        /// <summary>
        /// 枚举标签
        /// </summary>
        public object Tag { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Show { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public byte Sort { get; set; }
    }
}
