using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opsins.CMS.Services
{
    /// <summary>
    /// 用户接口
    /// </summary>
    public interface IUserService
    {
        string GetUserName(string name);
    }
}
