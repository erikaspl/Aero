using System.Web.Http;
using System.Web.Http.OData.Builder;
using Aero.Model;
using Microsoft.Data.Edm;
using Newtonsoft.Json.Serialization;

[assembly: WebActivator.PreApplicationStartMethod(
    typeof(Aero.App_Start.BreezeWebApiConfig), "RegisterBreezePreStart")]
namespace Aero.App_Start
{
    ///<summary>
    /// Inserts the Breeze Web API controller route at the front of all Web API routes
    ///</summary>
    ///<remarks>
    /// This class is discovered and run during startup; see
    /// http://blogs.msdn.com/b/davidebb/archive/2010/10/11/light-up-your-nupacks-with-startup-code-and-webactivator.aspx
    ///</remarks>
    public static class BreezeWebApiConfig
    {

        public static void RegisterBreezePreStart()
        {
            Register(GlobalConfiguration.Configuration);
        }

        public static void Register(HttpConfiguration config)
        {
            //config.Routes.MapODataRoute("Aero", "odata", GetImplicitEdm());
            config.Routes.MapHttpRoute(
              name: "BreezeApi",
              routeTemplate: "api/{controller}/{action}",
              defaults: new
                {
                    controller = "Aero",
                    action = "Parts"
                }
          );

            config.Formatters.XmlFormatter.UseXmlSerializer = true;

            //config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
            //    new CamelCasePropertyNamesContractResolver();
        }

        private static IEdmModel GetImplicitEdm()
        {
            var builder = new ODataConventionModelBuilder();
            builder.Namespace = "";
            builder.EntitySet<Contact>("Contacts");
            builder.EntitySet<Customer>("Customers");
            builder.EntitySet<Part>("Parts");
            builder.EntitySet<Vendor>("Vendors");
            builder.EntitySet<RFQ>("RFQ");
            builder.EntitySet<PO>("PO");
            builder.EntitySet<Priority>("Priorities");
            builder.EntitySet<RFQState>("RFQState");
            return builder.GetEdmModel();
        }
    }
}