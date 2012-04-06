using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Opsins
{
    /// <summary>
    /// 自定义系统异常
    /// </summary>
    public class XException : Exception
    {
        public XException()
        { }

        public XException(string message)
            : base(message)
        { }

        public XException(string message, Exception innerException)
            : base(message, innerException)
        { }

        protected XException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
