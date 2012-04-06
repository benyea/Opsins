using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opsins
{
    /// <summary>
    /// 封装TimeSpan
    /// </summary>
    public class TimeSpanWrapper
    {
        public TimeSpanWrapper(System.TimeSpan ts)
        {
            this.TimeSpan = ts;
        }

        public System.TimeSpan TimeSpan { get; private set; }

        public static implicit operator TimeSpanWrapper(System.TimeSpan ts)
        {
            return new TimeSpanWrapper(ts);
        }

        public static System.TimeSpan operator *(TimeSpanWrapper tsoe, int i)
        {
            var ts = tsoe.TimeSpan;
            var r = ts;
            for (var a = 1; a < i; a++)
            {
                r = r + ts;
            }
            return r;
        }
    }
}
