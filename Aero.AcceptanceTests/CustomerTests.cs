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
    public class CustomerTests
    {
        private HttpSelfHostServer _server;

        public CustomerTests()
        {
            _server = HttpSelfHost.GetServer();
        }

        [Fact]
        [UseDatabase]
        public void CustomerInsertGetTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var customer = TestData.CreateCustomer("customerName", "userName", contact);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Customer>(customer);
                var response = client.PostAsync("odata/Customers", requestMessage);
                Customer customerResponse = (Customer)((ObjectContent)(response.Result.Content)).Value;

                var response2 = client.GetAsync(string.Format("odata/Customers({0})", customerResponse.Id));

                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.OK);
                Customer customerResponse2 = (Customer)((ObjectContent)(response2.Result.Content)).Value;

                Assert.Equal(customerResponse2.Name, customer.Name);
                Assert.Equal(customerResponse2.Contact.Address1, customer.Contact.Address1);
                Assert.Equal(customerResponse2.Contact.Address2, customer.Contact.Address2);
                Assert.Equal(customerResponse2.Contact.Address3, customer.Contact.Address3);
                Assert.Equal(customerResponse2.Contact.City, customer.Contact.City);
                Assert.Equal(customerResponse2.Contact.Country, customer.Contact.Country);
                Assert.Equal(customerResponse2.Contact.County, customer.Contact.County);
                Assert.Equal(customerResponse2.Contact.Email, customer.Contact.Email);
                Assert.Equal(customerResponse2.Contact.Fax, customer.Contact.Fax);
                Assert.Equal(customerResponse2.Contact.Name, customer.Contact.Name);
                Assert.Equal(customerResponse2.Contact.Phone, customer.Contact.Phone);
                Assert.Equal(customerResponse2.Contact.PostCode, customer.Contact.PostCode);
                Assert.Equal(customerResponse2.Contact.Type, customer.Contact.Type);
            }
        }

        [Fact]
        [UseDatabase]
        public void CustomerUpdateGetTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var customer = TestData.CreateCustomer("customerName", "userName", contact);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Customer>(customer);
                var response = client.PostAsync("odata/Customers", requestMessage);
                Customer customerResponse = (Customer)((ObjectContent)(response.Result.Content)).Value;

                const string name = "customerNameUpdated";
                customerResponse.Name = name;

                const string addressLine1 = "addressLine1Updated";
                customerResponse.Contact.Address1 = addressLine1;

                const string addressLine2 = "addressLine2Updated";
                customerResponse.Contact.Address2 = addressLine2;

                const string addressLine3 = "addressLine3Updated";
                customerResponse.Contact.Address3 = addressLine3;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<Customer>(customerResponse);
                var response2 = client.PutAsync(string.Format("odata/Customers({0})", customerResponse.Id), requestMessage2);
                var response3 = client.GetAsync(string.Format("odata/Customers({0})", customerResponse.Id));

                Assert.Equal(response3.Result.StatusCode, HttpStatusCode.OK);
                Customer customerResponse3 = (Customer)((ObjectContent)(response3.Result.Content)).Value;

                Assert.Equal(customerResponse3.Name, name);
                Assert.Equal(customerResponse3.Contact.Address1, addressLine1);
                Assert.Equal(customerResponse3.Contact.Address2, addressLine2);
                Assert.Equal(customerResponse3.Contact.Address3, addressLine3);
            }
        }

        [Fact]
        [UseDatabase]
        public void CustomerPatchGetTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var customer = TestData.CreateCustomer("customerName", "userName", contact);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Customer>(customer);
                var response = client.PostAsync("odata/Customers", requestMessage);
                Customer customerResponse = (Customer)((ObjectContent)(response.Result.Content)).Value;

                const string name = "customerNameUpdated";
                customerResponse.Name = name;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<dynamic>(new { Name = name });
                var response2 = client.PatchAsync(string.Format("odata/Customers({0})", customerResponse.Id), requestMessage2);
                var response3 = client.GetAsync(string.Format("odata/Customers({0})", customerResponse.Id));

                Assert.Equal(response3.Result.StatusCode, HttpStatusCode.OK);
                Customer customerResponse3 = (Customer)((ObjectContent)(response3.Result.Content)).Value;

                Assert.Equal(customerResponse3.Name, name);
            }
        }

        [Fact]
        [UseDatabase]
        public void CustomerInsertGetDeleteTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var customer = TestData.CreateCustomer("customerName", "userName", contact);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Customer>(customer);
                var response = client.PostAsync("odata/Customers", requestMessage);
                Customer customerResponse = (Customer)((ObjectContent)(response.Result.Content)).Value;

                var response2 = client.GetAsync(string.Format("odata/Customers({0})", customerResponse.Id));
                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.OK);

                var response3 = client.DeleteAsync(string.Format("odata/Customers({0})", customerResponse.Id));
                var response4 = client.GetAsync(string.Format("odata/Customers({0})", customerResponse.Id));
                Assert.Equal(response4.Result.StatusCode, HttpStatusCode.NotFound);
            }
        }

        [Fact]
        [UseDatabase]
        public void CustomerExceptionDuringInsertTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var customer = TestData.CreateCustomer(null, "userName", contact);
            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Customer>(customer);
                var response = client.PostAsync("odata/Customers", requestMessage);
                Assert.Equal(response.Result.StatusCode, HttpStatusCode.InternalServerError);
                Assert.IsType<ObjectContent<ODataError>>(response.Result.Content);
            }
        }


        [Fact]
        [UseDatabase]
        public void CustomerExceptionDuringUpdateMissingDataTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var customer = TestData.CreateCustomer("customerName", "userName", contact);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Customer>(customer);
                var response = client.PostAsync("odata/Customers", requestMessage);
                Customer customerResponse = (Customer)((ObjectContent)(response.Result.Content)).Value;

                customerResponse.Name = null;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<Customer>(customerResponse);
                var response2 = client.PutAsync(string.Format("odata/Customers({0})", customerResponse.Id), requestMessage2);
                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.InternalServerError);
                Assert.IsType<ObjectContent<ODataError>>(response2.Result.Content);
            }
        }


        [Fact]
        [UseDatabase]
        public void CustomerExceptionDuringUpdateMissingIdTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var customer = TestData.CreateCustomer("customerName", "userName", contact);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Customer>(customer);
                var response = client.PostAsync("odata/Customers", requestMessage);
                Customer customerResponse = (Customer)((ObjectContent)(response.Result.Content)).Value;

                customerResponse.Name = "customerNameUpdated";
                customerResponse.Id = 1000;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<Customer>(customerResponse);
                var response2 = client.PutAsync(string.Format("odata/Customers({0})", customerResponse.Id), requestMessage2);
                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.NotFound);
                Assert.IsType<ObjectContent<ODataError>>(response2.Result.Content);
            }
        }

        [Fact]
        [UseDatabase]
        public void CustomerExceptionDuringPatchMissingIdTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var customer = TestData.CreateCustomer("customerName", "userName", contact);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Customer>(customer);
                var response = client.PostAsync("odata/Customers", requestMessage);
                Customer customerResponse = (Customer)((ObjectContent)(response.Result.Content)).Value;

                const string name = "customerNameUpdated";
                customerResponse.Name = name;
                customerResponse.Id = 1000;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<dynamic>(new { Id = 1000, Name = name });
                var response2 = client.PatchAsync(string.Format("odata/Customers({0})", customerResponse.Id), requestMessage2);
                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.NotFound);
                Assert.IsType<ObjectContent<ODataError>>(response2.Result.Content);
            }
        }
    }
}
