using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opsins
{
    /// <summary>
    /// List扩展
    /// </summary>
    public static class IListExtensions
    {
        #region 二分查找

        /// <summary>
        /// BinarySearch 从已排序的列表中，采用二分查找找到目标在列表中的位置。
        /// 如果刚好有个元素与目标相等，则返回true，且minIndex会被赋予该元素的位置；否则，返回false，且minIndex会被赋予比目标小且最接近目标的元素的位置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sortedList"></param>
        /// <param name="target"></param>
        /// <param name="minIndex"></param>
        /// <returns></returns>
        public static bool BinarySearch<T>(IList<T> sortedList, T target, out int minIndex) where T : IComparable
        {
            if (target.CompareTo(sortedList[0]) == 0)
            {
                minIndex = 0;
                return true;
            }

            if (target.CompareTo(sortedList[0]) < 0)
            {
                minIndex = -1;
                return false;
            }

            if (target.CompareTo(sortedList[sortedList.Count - 1]) == 0)
            {
                minIndex = sortedList.Count - 1;
                return true;
            }

            if (target.CompareTo(sortedList[sortedList.Count - 1]) > 0)
            {
                minIndex = sortedList.Count - 1;
                return false;
            }

            //int targetPosIndex = -1;
            int left = 0;
            int right = sortedList.Count - 1;

            while (right - left > 1)
            {
                int middle = (left + right) / 2;

                if (target.CompareTo(sortedList[middle]) == 0)
                {
                    minIndex = middle;
                    return true;
                }

                if (target.CompareTo(sortedList[middle]) < 0)
                {
                    right = middle;
                }
                else
                {
                    left = middle;
                }
            }

            minIndex = left;
            return false;
        }

        #endregion
    }
}
