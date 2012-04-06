using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opsins.CMS.Services.Imple
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class UserService : IUserService
    {
        public string GetUserName(string name)
        {
            return "my name is " + name;
        }
    }
}
