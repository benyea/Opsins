using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Opsins.CMS
{
    using Infra;

    public class RegisterControllerFactory : IBootstrapperTask
    {
        private readonly IControllerFactory _controllerFactory;

        public RegisterControllerFactory(IControllerFactory controllerFactory)
        {
            Checker.IsNotNull(controllerFactory,"controllerFactory");

            _controllerFactory = controllerFactory;
        }

        public void Execute()
        {
            ControllerBuilder.Current.SetControllerFactory(_controllerFactory);
        }
    }
}
