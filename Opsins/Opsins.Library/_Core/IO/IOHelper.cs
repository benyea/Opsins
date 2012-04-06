using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Opsins
{
    /// <summary>
    /// IO辅助
    /// </summary>
    public static partial class IOHelper
    {
        #region 文件夹操作

        /// <summary>
        /// 确保文件夹存在
        /// </summary>
        /// <param name="path"></param>
        public static void EnsureDirectoryExists(string path)
        {
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 判断是否存在文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool ExistsDirectory(string path)
        {
            return Directory.Exists(path);
        }

        #endregion
    }
}
