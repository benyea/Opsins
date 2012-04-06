using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opsins
{
    /// <summary>
    /// 版本辅助方法
    /// </summary>
    public class VersionHelper
    {
        /// <summary>
        /// 是否匹配版本规格
        /// </summary>
        /// <param name="version">版本字符串</param>
        /// <returns>返回：true/false</returns>
        public static bool Match(string version)
        {
            const string pattern = @"[0-9.]";
            return version.IsMatch(pattern);
        }

        /// <summary>
        /// 验证是否为有效版本规格
        /// </summary>
        /// <param name="verNo">版本字符串</param>
        /// <returns>返回0表示为有效版本规格</returns>
        public static int Valid(string verNo)
        {
            if (verNo.IsEmpty()) return -1;
            if (!Match(verNo)) return 1;
            verNo = verNo.Trim('.');
            var verArr = verNo.Split('.');

            var arrlen = verArr.Length;
            if (arrlen < 2 || arrlen > 4) return 2;
            for (int i = 0; i < arrlen; i++)
            {
                var strlen = verArr[i].Length;
                switch (i)
                {
                    case 0:
                    case 1:
                        if (strlen > 4) return 4;
                        if (strlen > 1)
                        {
                            if (verArr[i].StartsWith("0")) return 5;
                        }
                        break;
                    case 2:
                        if (strlen > 6) return 4;
                        if (strlen > 1)
                        {
                            if (verArr[i].StartsWith("0")) return 5;
                        }
                        break;
                    case 3:
                        if (strlen > 6) return 5;
                        break;
                }
            }
            return 0;
        }

        /// <summary>
        /// 判断是否需要升级
        /// </summary>
        /// <param name="newVer">新版本</param>
        /// <param name="oldVer">旧版本</param>
        /// <returns>返回TRUE：升级</returns>
        public static bool Upgrade(string newVer, string oldVer)
        {
            return Compare(newVer, oldVer) > 0;
        }

        /// <summary>
        /// 版本比较大小[返回值：-1：前者小于后者，0：两者相等，1：前者大于后者]
        /// </summary>
        /// <param name="newVer">新版本</param>
        /// <param name="oldVer">旧版本</param>
        /// <returns>返回值：-1：前者小于后者，0：两者相等，1：前者大于后者</returns>
        public static int Compare(string newVer, string oldVer)
        {
            //没有旧版本号时，当然新版本大于旧版本
            if (oldVer.IsEmpty()) return 1;
            //没有新版本号，当然新版本小于旧版本
            if (newVer.IsEmpty()) return -1;

            int[] newVers = AsVersion(newVer);
            int[] oldVers = AsVersion(oldVer);

            for (int i = 0; i < newVers.Length; i++)
            {
                if (newVers[i] > oldVers[i])
                {
                    return 1;
                }
                if (newVers[i] < oldVers[i])
                {
                    return -1;
                }
            }

            return 0;
        }

        /// <summary>
        /// 将版本字符串转换为整数数组
        /// </summary>
        /// <param name="version">版本字符串</param>
        /// <returns>返回版本整数数组</returns>
        public static int[] AsVersion(string version)
        {
            Checker.IsNotEmpty(version, "version");

            int[] vers = new int[4];
            var verArr = version.Split('.');
            for (int i = 0; i < verArr.Length; i++)
            {
                int.TryParse(verArr[i], out vers[i]);
            }
            return vers;
        }
    }
}
