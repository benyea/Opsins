using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Opsins.CMS.Web.Controllers
{
    using Services;

    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController()
        {
            _userService = Infra.Ioc.Resolve<IUserService>();
        }

        public ActionResult Index()
        {
            
            string name = _userService.GetUserName("benyea");
            Response.Write(name);

            return View();
        }

    }
}
