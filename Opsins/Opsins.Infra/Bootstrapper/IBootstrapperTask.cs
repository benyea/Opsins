using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opsins.Infra
{
    /// <summary>
    /// 启动接口
    /// </summary>
    public interface IBootstrapperTask
    {
        void Execute();
    }
}
