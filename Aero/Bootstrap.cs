using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Aero.App_Start;

namespace Aero
{
    public class Bootstrap
    {
        public void Configure(HttpConfiguration config)
        {
            BreezeWebApiConfig.Register(config);
        }
    }
}