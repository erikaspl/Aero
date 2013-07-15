using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using System.Web.Http.Dispatcher;
using System.Web.Http.Metadata;
using System.Web.Http.ModelBinding;
using System.Web.Http.SelfHost;
using System.Web.Http.Tracing;
using System.Web.Http.Validation;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;


namespace Aero.AcceptanceTests
{
    public static class HttpSelfHost
    {
        private static Uri _baseAddress = new Uri("http://localhost:5432");
        public static Uri BaseAddress
        {
            get
            {
                return _baseAddress;
            }
        }

        public static  HttpSelfHostServer GetServer()
        {
            var config = new HttpSelfHostConfiguration(_baseAddress);
            var container = new WindsorContainer();
            container.Install(FromAssembly.Named("Aero.Controllers"));

            //config.Services.Replace(typeof(ITraceWriter), new Tracer());
   
            new Bootstrap().Configure(config);
            var selfHostServer = new HttpSelfHostServer(config);           
            return selfHostServer;
        }

        public static HttpContent CreateHttpRequestMessage<T>(T obj)
        {
            MediaTypeFormatter formatter = new JsonMediaTypeFormatter();
            HttpContent content = new ObjectContent<T>(obj, formatter);
            return content;
        }
    }
}
