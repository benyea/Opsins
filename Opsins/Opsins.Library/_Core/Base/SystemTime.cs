using System;
using System.Diagnostics;

namespace Opsins
{
    /// <summary>
    /// 系统时间
    /// </summary>
    public static class SystemTime
    {
        private static Func<DateTime> now = () => DateTime.UtcNow;

        /// <summary>
        /// 现在时间
        /// </summary>
        public static DateTime Now
        {
            [DebuggerStepThrough]
            get
            {
                return now();
            }

            [DebuggerStepThrough]
            set
            {
                now = () => value;
            }
        }
    }
}
