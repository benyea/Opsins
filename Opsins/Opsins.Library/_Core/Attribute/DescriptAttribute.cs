using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Opsins
{
    /// <summary>
    /// 用于描述枚举备注信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    public class DescriptAttribute : Attribute
    {
        private static readonly IDictionary<string, IList<EnumItem>> _decriptCache = new Dictionary<string, IList<EnumItem>>();

        public DescriptAttribute(string descript)
            : this(descript, 0, null, true, 0)
        { }

        public DescriptAttribute(string descript, object tag)
            : this(descript, 0, tag, true, 0)
        { }

        public DescriptAttribute(string descript, int value)
            : this(descript, value, null, true, 0)
        { }

        public DescriptAttribute(string descript, int value, byte sort)
            : this(descript, value, null, true, sort)
        { }

        public DescriptAttribute(string descript, int value, bool show)
            : this(descript, value, null, show, 0)
        { }

        public DescriptAttribute(string descript, int value, object tag)
            : this(descript, value, tag, true, 0)
        { }

        public DescriptAttribute(string descript, int value, bool show, byte sort)
            : this(descript, value, null, show, sort)
        { }

        public DescriptAttribute(string descript, int value, object tag, bool show)
            : this(descript, value, tag, show, 0)
        { }

        public DescriptAttribute(string descript, int value, object tag, bool show, byte sort)
        {
            EnumItem = new EnumItem()
            {
                Descript = descript,
                Value = value,
                Tag = tag,
                Show = show,
                Sort = sort
            };
        }

        #region 属性

        /// <summary>
        /// 枚举项值
        /// </summary>
        public EnumItem EnumItem { get; set; }

        #endregion

        #region 方法重写

        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return EnumItem.Descript;
        }

        #endregion

        #region 公有方法

        /// <summary>
        /// 得到枚举类型定义的所有枚举值的描述文本
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static IList<EnumItem> GetFieldTexts(Type enumType)
        {
            string fullName = enumType.FullName;
            if (string.IsNullOrEmpty(fullName)) return null;

            if (!_decriptCache.ContainsKey(fullName))
            {
                FieldInfo[] fields = enumType.GetFields();
                IList<EnumItem> list = new List<EnumItem>();
                foreach (FieldInfo fi in fields)
                {
                    object[] remarks = fi.GetCustomAttributes(typeof(DescriptAttribute), false);
                    if (remarks.Length == 1)
                    {
                        DescriptAttribute remark = (DescriptAttribute)remarks[0];
                        remark.EnumItem.Name = fi.GetValue(null);

                        list.Add(remark.EnumItem);
                    }
                }

                _decriptCache.Add(fullName, list);
            }
            return _decriptCache[fullName];
        }

        /// <summary>
        /// 获取枚举类型的描述文本
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static string GetEnumDescript(Type enumType)
        {
            DescriptAttribute[] remarkAttributes = (DescriptAttribute[])enumType.GetCustomAttributes(typeof(DescriptAttribute), false);
            if (remarkAttributes.Length < 1)
            {
                return string.Empty;
            }

            return remarkAttributes[0].EnumItem.Descript;
        }

        /// <summary>
        /// 获取枚举类型携带的Tag
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static object GetEnumTag(Type enumType)
        {
            var remarkAttributes = (DescriptAttribute[])enumType.GetCustomAttributes(typeof(DescriptAttribute), false);
            if (remarkAttributes.Length != 1)
            {
                return string.Empty;
            }

            return remarkAttributes[0].EnumItem.Tag;
        }

        /// <summary>
        /// 获得指定枚举值的描述文本
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetFieldDescript(object enumValue)
        {
            var list = GetFieldTexts(enumValue.GetType());
            if (list == null) return null;

            return list.ConvertOnFirst<EnumItem, string>(e => e.Descript,
                                                                    e => e.Name.ToString() == enumValue.ToString());
        }

        /// <summary>
        /// 获得指定枚举值的Tag
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static object GetFieldTag(object enumValue)
        {
            var list = GetFieldTexts(enumValue.GetType());
            if (list == null) return null;

            return list.ConvertOnFirst<EnumItem, object>(e => e.Tag,
                                                                    e => e.Name.ToString() == enumValue.ToString());
        }

        #endregion
    }
}
