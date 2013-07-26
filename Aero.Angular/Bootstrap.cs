using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Aero.Angular.App_Start;

namespace Aero.Angular
{
    public class Bootstrap
    {
        public void Configure(HttpConfiguration config)
        {
            Register(GlobalConfiguration.Configuration);
        }

        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
              name: "BreezeApi",
              routeTemplate: "api/{controller}/{action}"
          );
        }
    }
}