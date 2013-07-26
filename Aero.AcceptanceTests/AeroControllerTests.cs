using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.SelfHost;
using Aero.Model;
using Xunit;

namespace Aero.AcceptanceTests
{
    public class AeroControllerTests
    {
        private HttpSelfHostServer _server;

        public AeroControllerTests()
        {
            _server = HttpSelfHost.GetServer();
        }

        //[Fact]
        //[UseDatabase]
        //public void PartInsertGetTest()
        //{
        //    var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
        //    var vendor = TestData.CreateVendor("VendorName", contact);
        //    var part = TestData.CreatePart(Condition.New, "description", "model", "nsn", "partNumber", 1010, 12, "source", new DateTime(2010, 10, 10), vendor);

        //    using (var client = new HttpClient(_server))
        //    {
        //        client.BaseAddress = HttpSelfHost.BaseAddress;

        //        var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Part>(part);
        //        ////~/api/Todo/SaveChanges

        //        client.PostAsync("api/Aero/SaveChanges", requestMessage).Wait();

        //        var response = client.GetAsync("api/Aero/Parts");
        //        Part partResponse = (Part)((ObjectContent)(response.Result.Content)).Value;

        //        //var response2 = client.GetAsync(string.Format("api/Aero/Parts", partResponse.Id));

        //        //Assert.Equal(response2.Result.StatusCode, HttpStatusCode.OK);
        //        //Part partResponse2 = (Part)((ObjectContent)(response2.Result.Content)).Value;

        //        //Assert.Equal(partResponse2.Condition, part.Condition);
        //        //Assert.Equal(partResponse2.Description, part.Description);
        //        //Assert.Equal(partResponse2.Model, part.Model);
        //        //Assert.Equal(partResponse2.NSN, part.NSN);
        //        //Assert.Equal(partResponse2.PartNumber, part.PartNumber);
        //        //Assert.Equal(partResponse2.Price, part.Price);
        //        //Assert.Equal(partResponse2.Qty, part.Qty);
        //        //Assert.Equal(partResponse2.Source, part.Source);
        //        //Assert.Equal(partResponse2.UpdateDate, part.UpdateDate);
        //        //Assert.Equal(partResponse2.Vendor.Name, part.Vendor.Name);
        //    }
        //}


    }
}
