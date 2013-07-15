using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.SelfHost;
using Aero.Model;
using Microsoft.Data.OData;
using Xunit;

namespace Aero.AcceptanceTests
{
    public class PartTests
    {
      private HttpSelfHostServer _server;

        public PartTests()
        {
            _server = HttpSelfHost.GetServer();
        }

        [Fact]
        [UseDatabase]
        public void PartInsertGetTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);
            var part = TestData.CreatePart(Condition.New, "description", "model", "nsn", "partNumber", 1010, 12, "source", new DateTime(2010, 10, 10), vendor);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Part>(part);
                var response = client.PostAsync("odata/Parts", requestMessage);
                Part partResponse = (Part)((ObjectContent)(response.Result.Content)).Value;

                var response2 = client.GetAsync(string.Format("odata/Parts({0})", partResponse.Id));

                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.OK);
                Part partResponse2 = (Part)((ObjectContent)(response2.Result.Content)).Value;

                Assert.Equal(partResponse2.Condition, part.Condition);
                Assert.Equal(partResponse2.Description, part.Description);
                Assert.Equal(partResponse2.Model, part.Model);
                Assert.Equal(partResponse2.NSN, part.NSN);
                Assert.Equal(partResponse2.PartNumber, part.PartNumber);
                Assert.Equal(partResponse2.Price, part.Price);
                Assert.Equal(partResponse2.Qty, part.Qty);
                Assert.Equal(partResponse2.Source, part.Source);
                Assert.Equal(partResponse2.UpdateDate, part.UpdateDate);
                Assert.Equal(partResponse2.Vendor.Name, part.Vendor.Name);
            }
        }

        [Fact]
        [UseDatabase]
        public void PartInsertUpdateGetTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);
            var part = TestData.CreatePart(Condition.New, "description", "model", "nsn", "partNumber", 1010, 12, "source", new DateTime(2010, 10, 10), vendor);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Part>(part);
                var response = client.PostAsync("odata/Parts", requestMessage);
                Part partResponse = (Part)((ObjectContent)(response.Result.Content)).Value;

                const Condition condition = Condition.Used;
                partResponse.Condition = condition;

                const string description = "descriptionUpdated";
                partResponse.Description = description;

                const string model = "modelUpdated";
                partResponse.Model = model;

                const string nsn = "nsnUpdated";
                partResponse.NSN = nsn;

                const string partNumber = "partNumberUpdated";
                partResponse.PartNumber = partNumber;

                const decimal price = 1111.05M;
                partResponse.Price = price;

                const short qty = 22;
                partResponse.Qty = qty;

                const string source = "sourceUpdated";
                partResponse.Source = source;

                DateTime updateDate = new DateTime(2001, 02, 02);
                partResponse.UpdateDate = updateDate;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<Part>(partResponse);
                var response2 = client.PutAsync(string.Format("odata/Parts({0})", partResponse.Id), requestMessage2);
                var response3 = client.GetAsync(string.Format("odata/Parts({0})", partResponse.Id));
                Assert.Equal(response3.Result.StatusCode, HttpStatusCode.OK);
                Part partResponse3 = (Part)((ObjectContent)(response3.Result.Content)).Value;

                Assert.Equal(partResponse3.Condition, condition);
                Assert.Equal(partResponse3.Description, description);
                Assert.Equal(partResponse3.Model, model);
                Assert.Equal(partResponse3.NSN, nsn);
                Assert.Equal(partResponse3.PartNumber, partNumber);
                Assert.Equal(partResponse3.Price, price);
                Assert.Equal(partResponse3.Qty, qty);
                Assert.Equal(partResponse3.Source, source);
                Assert.Equal(partResponse3.UpdateDate, updateDate);
                Assert.Equal(partResponse3.Vendor.Name, part.Vendor.Name);
            }
        }

        [Fact]
        [UseDatabase]
        public void PartInsertPatchGetTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);
            var part = TestData.CreatePart(Condition.New, "description", "model", "nsn", "partNumber", 1010, 12, "source", new DateTime(2010, 10, 10), vendor);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Part>(part);
                var response = client.PostAsync("odata/Parts", requestMessage);
                Part partResponse = (Part)((ObjectContent)(response.Result.Content)).Value;

                const string description = "descriptionUpdated";
                const string model = "modelUpdated";
                const string nsn = "nsnUpdated";
                const string partNumber = "partNumberUpdated";
                const short qty = 22;
                const string source = "sourceUpdated";
                DateTime updateDate = new DateTime(2001, 02, 02);

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<dynamic>(new { Description = description, Model = model, NSN = nsn, PartNumber = partNumber, Qty = qty, Source = source, UpdateDate = updateDate});
                var response2 = client.PatchAsync(string.Format("odata/Parts({0})", partResponse.Id), requestMessage2);
                var response3 = client.GetAsync(string.Format("odata/Parts({0})", partResponse.Id));
                Assert.Equal(response3.Result.StatusCode, HttpStatusCode.OK);
                Part partResponse3 = (Part)((ObjectContent)(response3.Result.Content)).Value;

                Assert.Equal(partResponse3.Description, description);
                Assert.Equal(partResponse3.Model, model);
                Assert.Equal(partResponse3.NSN, nsn);
                Assert.Equal(partResponse3.PartNumber, partNumber);
                Assert.Equal(partResponse3.Qty, qty);
                Assert.Equal(partResponse3.Source, source);
                Assert.Equal(partResponse3.UpdateDate, updateDate);
                Assert.Equal(partResponse3.Vendor.Name, part.Vendor.Name);
            }
        }

        [Fact]
        [UseDatabase]
        public void PartInsertGetDeleteTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);
            var part = TestData.CreatePart(Condition.New, "description", "model", "nsn", "partNumber", 1010, 12, "source", new DateTime(2010, 10, 10), vendor);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Part>(part);
                var response = client.PostAsync("odata/Parts", requestMessage);
                Part partResponse = (Part)((ObjectContent)(response.Result.Content)).Value;

                var response2 = client.GetAsync(string.Format("odata/Parts({0})", partResponse.Id));
                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.OK);

                var response3 = client.DeleteAsync(string.Format("odata/Parts({0})", partResponse.Id));
                var response4 = client.GetAsync(string.Format("odata/Parts({0})", partResponse.Id));
                Assert.Equal(response4.Result.StatusCode, HttpStatusCode.NotFound);
            }
        }

        [Fact]
        [UseDatabase]
        public void PartExceptionDuringInsertTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);
            var part = TestData.CreatePart(Condition.New, null, null, "nsn", "partNumber", 1010, 12, "source", new DateTime(2010, 10, 10), vendor);
            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Part>(part);
                var response = client.PostAsync("odata/Parts", requestMessage);
                Assert.Equal(response.Result.StatusCode, HttpStatusCode.InternalServerError);
                Assert.IsType<ObjectContent<ODataError>>(response.Result.Content);
            }
        }


        [Fact]
        [UseDatabase]
        public void PartExceptionDuringUpdateMissingDataTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);
            var part = TestData.CreatePart(Condition.New, "description", "model", "nsn", "partNumber", 1010, 12, "source", new DateTime(2010, 10, 10), vendor);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Part>(part);
                var response = client.PostAsync("odata/Parts", requestMessage);
                Part partResponse = (Part)((ObjectContent)(response.Result.Content)).Value;

                partResponse.Description = null;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<Part>(partResponse);
                var response2 = client.PutAsync(string.Format("odata/Parts({0})", partResponse.Id), requestMessage2);
                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.InternalServerError);
                Assert.IsType<ObjectContent<ODataError>>(response2.Result.Content);
            }
        }


        [Fact]
        [UseDatabase]
        public void PartExceptionDuringUpdateMissingIdTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);
            var part = TestData.CreatePart(Condition.New, "description", "model", "nsn", "partNumber", 1010, 12, "source", new DateTime(2010, 10, 10), vendor);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Part>(part);
                var response = client.PostAsync("odata/Parts", requestMessage);
                Part partResponse = (Part)((ObjectContent)(response.Result.Content)).Value;

                partResponse.Description = "partDescUpdated";
                partResponse.Id = 1000;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<Part>(partResponse);
                var response2 = client.PutAsync(string.Format("odata/Parts({0})", partResponse.Id), requestMessage2);
                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.NotFound);
                Assert.IsType<ObjectContent<ODataError>>(response2.Result.Content);
            }
        }

        [Fact]
        [UseDatabase]
        public void PartExceptionDuringPatchMissingIdTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);
            var part = TestData.CreatePart(Condition.New, "description", "model", "nsn", "partNumber", 1010, 12, "source", new DateTime(2010, 10, 10), vendor);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Part>(part);
                var response = client.PostAsync("odata/Parts", requestMessage);
                Part partResponse = (Part)((ObjectContent)(response.Result.Content)).Value;

                const string description = "partDescUpdated";
                partResponse.Description = description;
                partResponse.Id = 1000;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<dynamic>(new { Id = 1000, Description = description });
                var response2 = client.PatchAsync(string.Format("odata/Customers({0})", partResponse.Id), requestMessage2);
                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.NotFound);
                Assert.IsType<ObjectContent<ODataError>>(response2.Result.Content);
            }
        }
 
    }
}
