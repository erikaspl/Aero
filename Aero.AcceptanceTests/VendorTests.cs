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
    public class VendorTests
    {
       private HttpSelfHostServer _server;

       public VendorTests()
        {
            _server = HttpSelfHost.GetServer();
        }

        [Fact]
        [UseDatabase]
       public void VendorInsertGetTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);


            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Vendor>(vendor);
                var response = client.PostAsync("odata/Vendors", requestMessage);
                Vendor vendorResponse = (Vendor)((ObjectContent)(response.Result.Content)).Value;

                var response2 = client.GetAsync(string.Format("odata/Vendors({0})", vendorResponse.Id));

                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.OK);
                Vendor vendorResponse2 = (Vendor)((ObjectContent)(response2.Result.Content)).Value;

                Assert.Equal(vendorResponse2.Name, vendor.Name);
                Assert.Equal(vendorResponse2.Contact.Address1, vendor.Contact.Address1);
                Assert.Equal(vendorResponse2.Contact.Address2, vendor.Contact.Address2);
                Assert.Equal(vendorResponse2.Contact.Address3, vendor.Contact.Address3);
                Assert.Equal(vendorResponse2.Contact.City, vendor.Contact.City);
                Assert.Equal(vendorResponse2.Contact.Country, vendor.Contact.Country);
                Assert.Equal(vendorResponse2.Contact.County, vendor.Contact.County);
                Assert.Equal(vendorResponse2.Contact.Email, vendor.Contact.Email);
                Assert.Equal(vendorResponse2.Contact.Fax, vendor.Contact.Fax);
                Assert.Equal(vendorResponse2.Contact.Name, vendor.Contact.Name);
                Assert.Equal(vendorResponse2.Contact.Phone, vendor.Contact.Phone);
                Assert.Equal(vendorResponse2.Contact.PostCode, vendor.Contact.PostCode);
                Assert.Equal(vendorResponse2.Contact.Type, vendor.Contact.Type);
            }
        }

        [Fact]
        [UseDatabase]
        public void VendorUpdateGetTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Vendor>(vendor);
                var response = client.PostAsync("odata/Vendors", requestMessage);
                Vendor vendorResponse = (Vendor)((ObjectContent)(response.Result.Content)).Value;

                const string name = "vendorNameUpdated";
                vendorResponse.Name = name;

                const string addressLine1 = "addressLine1Updated";
                vendorResponse.Contact.Address1 = addressLine1;

                const string addressLine2 = "addressLine2Updated";
                vendorResponse.Contact.Address2 = addressLine2;

                const string addressLine3 = "addressLine3Updated";
                vendorResponse.Contact.Address3 = addressLine3;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<Vendor>(vendorResponse);
                var response2 = client.PutAsync(string.Format("odata/Vendors({0})", vendorResponse.Id), requestMessage2);
                var response3 = client.GetAsync(string.Format("odata/Vendors({0})", vendorResponse.Id));

                Assert.Equal(response3.Result.StatusCode, HttpStatusCode.OK);
                Vendor vendorResponse3 = (Vendor)((ObjectContent)(response3.Result.Content)).Value;

                Assert.Equal(vendorResponse3.Name, name);
                Assert.Equal(vendorResponse3.Contact.Address1, addressLine1);
                Assert.Equal(vendorResponse3.Contact.Address2, addressLine2);
                Assert.Equal(vendorResponse3.Contact.Address3, addressLine3);
            }
        }

        [Fact]
        [UseDatabase]
        public void VendorPatchGetTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Vendor>(vendor);
                var response = client.PostAsync("odata/Vendors", requestMessage);
                Vendor vendorResponse = (Vendor)((ObjectContent)(response.Result.Content)).Value;

                const string name = "vendorNameUpdated";
                vendorResponse.Name = name;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<dynamic>(new { Name = name });
                var response2 = client.PatchAsync(string.Format("odata/Vendors({0})", vendorResponse.Id), requestMessage2);
                var response3 = client.GetAsync(string.Format("odata/Vendors({0})", vendorResponse.Id));

                Assert.Equal(response3.Result.StatusCode, HttpStatusCode.OK);
                Vendor customerResponse3 = (Vendor)((ObjectContent)(response3.Result.Content)).Value;

                Assert.Equal(customerResponse3.Name, name);
            }
        }

        [Fact]
        [UseDatabase]
        public void VendorInsertGetDeleteTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Vendor>(vendor);
                var response = client.PostAsync("odata/Vendors", requestMessage);
                Vendor vendorResponse = (Vendor)((ObjectContent)(response.Result.Content)).Value;

                var response2 = client.GetAsync(string.Format("odata/Vendors({0})", vendorResponse.Id));
                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.OK);

                var response3 = client.DeleteAsync(string.Format("odata/Vendors({0})", vendorResponse.Id));
                var response4 = client.GetAsync(string.Format("odata/Vendors({0})", vendorResponse.Id));
                Assert.Equal(response4.Result.StatusCode, HttpStatusCode.NotFound);
            }
        }

        [Fact]
        [UseDatabase]
        public void VendorExceptionDuringInsertTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor(null, contact);
            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Vendor>(vendor);
                var response = client.PostAsync("odata/Vendors", requestMessage);
                Assert.Equal(response.Result.StatusCode, HttpStatusCode.InternalServerError);
                Assert.IsType<ObjectContent<ODataError>>(response.Result.Content);
            }
        }


        [Fact]
        [UseDatabase]
        public void VendorExceptionDuringUpdateMissingDataTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Vendor>(vendor);
                var response = client.PostAsync("odata/Vendors", requestMessage);
                Vendor vendorResponse = (Vendor)((ObjectContent)(response.Result.Content)).Value;

                vendorResponse.Name = null;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<Vendor>(vendorResponse);
                var response2 = client.PutAsync(string.Format("odata/Vendors({0})", vendorResponse.Id), requestMessage2);
                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.InternalServerError);
                Assert.IsType<ObjectContent<ODataError>>(response2.Result.Content);
            }
        }


        [Fact]
        [UseDatabase]
        public void VendorExceptionDuringUpdateMissingIdTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Vendor>(vendor);
                var response = client.PostAsync("odata/Vendors", requestMessage);
                Vendor vendorResponse = (Vendor)((ObjectContent)(response.Result.Content)).Value;

                vendorResponse.Name = "vendorNameUpdated";
                vendorResponse.Id = 1000;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<Vendor>(vendorResponse);
                var response2 = client.PutAsync(string.Format("odata/Vendors({0})", vendorResponse.Id), requestMessage2);
                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.NotFound);
                Assert.IsType<ObjectContent<ODataError>>(response2.Result.Content);
            }
        }

        [Fact]
        [UseDatabase]
        public void VendorExceptionDuringPatchMissingIdTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Vendor>(vendor);
                var response = client.PostAsync("odata/Vendors", requestMessage);
                Vendor customerResponse = (Vendor)((ObjectContent)(response.Result.Content)).Value;

                const string name = "vendorNameUpdated";
                customerResponse.Name = name;
                customerResponse.Id = 1000;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<dynamic>(new { Id = 1000, Name = name });
                var response2 = client.PatchAsync(string.Format("odata/Vendors({0})", customerResponse.Id), requestMessage2);
                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.NotFound);
                Assert.IsType<ObjectContent<ODataError>>(response2.Result.Content);
            }
        }
    }
}
