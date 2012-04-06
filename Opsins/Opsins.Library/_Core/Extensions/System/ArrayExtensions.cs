using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Opsins
{
    /// <summary>
    /// Array扩展
    /// </summary>
    public static class ArrayExtensions
    {
        #region 是否为NULL或Empty

        /// <summary>
        /// 判断数组是否为NULL或Empty
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static bool IsEmpty(this string[] array)
        {
            return (array == null) || (array.Length == 0) || (array[0] == null);
        }

        /// <summary>
        /// 判断二维数组是否为NULL或Empty
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static bool IsEmpty(this string[,] array)
        {
            return (array == null) || (array.Length == 0) || (array[0, 0] == null);
        }

        /// <summary>
        /// 判断数组是否为NULL或Empty
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static bool IsEmpty(this string[][] array)
        {
            return (array == null) || (array.Length == 0) || (array[0] == null);
        }

        /// <summary>
        /// 判断数组是否为不NULL或Empty
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static bool IsNotEmpty(this string[] array)
        {
            return (array != null) && (array.Length > 0);
        }

        /// <summary>
        /// 判断二维数组是否不为NULL或Empty
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static bool IsNotEmpty(this string[,] array)
        {
            return (array != null) && (array.Length > 0);
        }

        /// <summary>
        /// 判断数组是否不为NULL或Empty
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static bool IsNotEmpty(this string[][] array)
        {
            return (array != null) && (array.Length > 0);
        }

        #endregion

        #region 获取部分数据

        /// <summary>
        /// 获取数组中指定的部分数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">数组</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="count">个数</param>
        /// <param name="reverse">是否反转[默认为false]</param>
        /// <returns></returns>
        public static T[] Part<T>(this T[] array, int startIndex, int count, bool reverse = false)
        {
            if (startIndex >= array.Length)
            {
                return null;
            }

            if (array.Length < startIndex + count)
            {
                count = array.Length - startIndex;
            }

            T[] result = new T[count];

            if (!reverse)
            {
                for (int i = 0; i < count; i++)
                {
                    result[i] = array[startIndex + i];
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    result[i] = array[array.Length - startIndex - 1 - i];
                }
            }

            return result;
        }

        #endregion

        #region 转换为IList



        #endregion

        #region 排序

        /// <summary>
        /// 数据冒泡排序
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string[] BubbleSort(this string[] array)
        {
            int i, j; //交换标志 
            string temp;

            bool exchange;

            for (i = 0; i < array.Length; i++) //最多做array.Length-1趟排序 
            {
                exchange = false; //本趟排序开始前，交换标志应为假

                for (j = array.Length - 2; j >= i; j--)
                {
                    if (String.CompareOrdinal(array[j + 1], array[j]) < 0)　//交换条件
                    {
                        temp = array[j + 1];
                        array[j + 1] = array[j];
                        array[j] = temp;

                        exchange = true; //发生了交换，故将交换标志置为真 
                    }
                }

                if (!exchange) //本趟排序未发生交换，提前终止算法 
                {
                    break;
                }
            }
            return array;
        }

        #endregion

        #region 其他操作


        #endregion
    }
}
