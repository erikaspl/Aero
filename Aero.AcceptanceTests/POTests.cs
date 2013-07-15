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
    public class POTests
    {
        private HttpSelfHostServer _server;

        public POTests()
        {
            _server = HttpSelfHost.GetServer();
        }

        [Fact]
        [UseDatabase]
        public void POInsertGetTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);
            var part = TestData.CreatePart(Condition.New, "description", "model", "nsn", "partNumber", 1010, 12, "source", new DateTime(2010, 10, 10), vendor);
            var customerContact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var customer = TestData.CreateCustomer("customerName", "userName", customerContact);
            var po = TestData.CreatePO("comment", customer, new DateTime(2010, 10, 10), "1234", part, 11, 100.05M);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<PO>(po);
                var response = client.PostAsync("odata/PO", requestMessage);
                PO poResponse = (PO)((ObjectContent)(response.Result.Content)).Value;

                var response2 = client.GetAsync(string.Format("odata/PO({0})", poResponse.Id));

                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.OK);
                PO poResponse2 = (PO)((ObjectContent)(response2.Result.Content)).Value;

                Assert.Equal(poResponse2.Comment, po.Comment);
                Assert.Equal(poResponse2.DeliveryDate, po.DeliveryDate);
                Assert.Equal(poResponse2.Number, po.Number);
                Assert.Equal(poResponse2.Qty, po.Qty);
                Assert.Equal(poResponse2.UnitPrice, po.UnitPrice);
                Assert.Equal(poResponse2.Part.Description, part.Description);
                Assert.Equal(poResponse2.Customer.Name, customer.Name);
            }
        }

        [Fact]
        [UseDatabase]
        public void POUpdateGetTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);
            var part = TestData.CreatePart(Condition.New, "description", "model", "nsn", "partNumber", 1010, 12, "source", new DateTime(2010, 10, 10), vendor);
            var customerContact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var customer = TestData.CreateCustomer("customerName", "userName", customerContact);
            var po = TestData.CreatePO("comment", customer, new DateTime(2010, 10, 10), "1234", part, 11, 100.05M);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<PO>(po);
                var response = client.PostAsync("odata/PO", requestMessage);
                PO poResponse = (PO)((ObjectContent)(response.Result.Content)).Value;

                const string comment = "poCommentUpdated";
                poResponse.Comment = comment;

                DateTime deliveryDate = new DateTime(2012, 02, 02);
                poResponse.DeliveryDate = deliveryDate;

                string  number = "53412";
                poResponse.Number = number;

                const short qty = 22;
                poResponse.Qty = qty;

                const decimal price = 111.06M;
                poResponse.UnitPrice = price;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<PO>(poResponse);
                var response2 = client.PutAsync(string.Format("odata/PO({0})", poResponse.Id), requestMessage2);
                var response3 = client.GetAsync(string.Format("odata/PO({0})", poResponse.Id));

                Assert.Equal(response3.Result.StatusCode, HttpStatusCode.OK);
                PO poResponse3 = (PO)((ObjectContent)(response3.Result.Content)).Value;

                Assert.Equal(poResponse3.Comment, comment);
                Assert.Equal(poResponse3.DeliveryDate, deliveryDate);
                Assert.Equal(poResponse3.Number, number);
                Assert.Equal(poResponse3.Qty, qty);
                Assert.Equal(poResponse3.UnitPrice, price);
            }
        }

        [Fact]
        [UseDatabase]
        public void POPatchGetTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);
            var part = TestData.CreatePart(Condition.New, "description", "model", "nsn", "partNumber", 1010, 12, "source", new DateTime(2010, 10, 10), vendor);
            var customerContact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var customer = TestData.CreateCustomer("customerName", "userName", customerContact);
            var po = TestData.CreatePO("comment", customer, new DateTime(2010, 10, 10), "1234", part, 11, 100.05M);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<PO>(po);
                var response = client.PostAsync("odata/PO", requestMessage);
                PO poResponse = (PO)((ObjectContent)(response.Result.Content)).Value;

                const string comment = "poCommentUpdated";
                poResponse.Comment = comment;

                DateTime deliveryDate = new DateTime(2012, 02, 02);
                poResponse.DeliveryDate = deliveryDate;

                string number = "53412";
                poResponse.Number = number;

                const short qty = 22;
                poResponse.Qty = qty;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<dynamic>(new { Comment = comment, DeliveryDate = deliveryDate, Number = number, Qty = qty });
                var response2 = client.PatchAsync(string.Format("odata/PO({0})", poResponse.Id), requestMessage2);
                var response3 = client.GetAsync(string.Format("odata/PO({0})", poResponse.Id));

                Assert.Equal(response3.Result.StatusCode, HttpStatusCode.OK);
                PO poResponse3 = (PO)((ObjectContent)(response3.Result.Content)).Value;

                Assert.Equal(poResponse3.Comment, comment);
                Assert.Equal(poResponse3.DeliveryDate, deliveryDate);
                Assert.Equal(poResponse3.Number, number);
                Assert.Equal(poResponse3.Qty, qty);
            }
        }

        [Fact]
        [UseDatabase]
        public void POInsertGetDeleteTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);
            var part = TestData.CreatePart(Condition.New, "description", "model", "nsn", "partNumber", 1010, 12, "source", new DateTime(2010, 10, 10), vendor);
            var customerContact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var customer = TestData.CreateCustomer("customerName", "userName", customerContact);
            var po = TestData.CreatePO("comment", customer, new DateTime(2010, 10, 10), "1234", part, 11, 100.05M);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<PO>(po);
                var response = client.PostAsync("odata/PO", requestMessage);
                PO rfqResponse = (PO)((ObjectContent)(response.Result.Content)).Value;

                var response2 = client.GetAsync(string.Format("odata/PO({0})", rfqResponse.Id));
                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.OK);

                var response3 = client.DeleteAsync(string.Format("odata/PO({0})", rfqResponse.Id));
                var response4 = client.GetAsync(string.Format("odata/PO({0})", rfqResponse.Id));
                Assert.Equal(response4.Result.StatusCode, HttpStatusCode.NotFound);
            }
        }

        [Fact]
        [UseDatabase]
        public void POExceptionDuringInsertTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);
            var part = TestData.CreatePart(Condition.New, "description", "model", "nsn", "partNumber", 1010, 12, "source", new DateTime(2010, 10, 10), vendor);
            var customerContact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var customer = TestData.CreateCustomer("customerName", "userName", customerContact);
            var po = TestData.CreatePO(null, customer, new DateTime(2010, 10, 10), "1234", part, 11, 100.05M);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<PO>(po);
                var response = client.PostAsync("odata/PO", requestMessage);
                Assert.Equal(response.Result.StatusCode, HttpStatusCode.InternalServerError);
                Assert.IsType<ObjectContent<ODataError>>(response.Result.Content);
            }
        }


        [Fact]
        [UseDatabase]
        public void POExceptionDuringUpdateMissingDataTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);
            var part = TestData.CreatePart(Condition.New, "description", "model", "nsn", "partNumber", 1010, 12, "source", new DateTime(2010, 10, 10), vendor);
            var customerContact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var customer = TestData.CreateCustomer("customerName", "userName", customerContact);
            var po = TestData.CreatePO("comment", customer, new DateTime(2010, 10, 10), "1234", part, 11, 100.05M);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<PO>(po);
                var response = client.PostAsync("odata/PO", requestMessage);
                PO poResponse = (PO)((ObjectContent)(response.Result.Content)).Value;

                poResponse.Comment = null;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<PO>(poResponse);
                var response2 = client.PutAsync(string.Format("odata/PO({0})", poResponse.Id), requestMessage2);
                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.InternalServerError);
                Assert.IsType<ObjectContent<ODataError>>(response2.Result.Content);
            }
        }


        [Fact]
        [UseDatabase]
        public void POExceptionDuringUpdateMissingIdTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);
            var part = TestData.CreatePart(Condition.New, "description", "model", "nsn", "partNumber", 1010, 12, "source", new DateTime(2010, 10, 10), vendor);
            var customerContact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var customer = TestData.CreateCustomer("customerName", "userName", customerContact);
            var po = TestData.CreatePO("comment", customer, new DateTime(2010, 10, 10), "1234", part, 11, 100.05M);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<PO>(po);
                var response = client.PostAsync("odata/PO", requestMessage);
                PO poResponse = (PO)((ObjectContent)(response.Result.Content)).Value;

                poResponse.Comment = "pocommentUpdated";
                poResponse.Id = 1000;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<PO>(poResponse);
                var response2 = client.PutAsync(string.Format("odata/PO({0})", poResponse.Id), requestMessage2);
                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.NotFound);
                Assert.IsType<ObjectContent<ODataError>>(response2.Result.Content);
            }
        }

        [Fact]
        [UseDatabase]
        public void POExceptionDuringPatchMissingIdTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);
            var part = TestData.CreatePart(Condition.New, "description", "model", "nsn", "partNumber", 1010, 12, "source", new DateTime(2010, 10, 10), vendor);
            var customerContact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var customer = TestData.CreateCustomer("customerName", "userName", customerContact);
            var po = TestData.CreatePO("comment", customer, new DateTime(2010, 10, 10), "1234", part, 11, 100.05M);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<PO>(po);
                var response = client.PostAsync("odata/PO", requestMessage);
                PO poResponse = (PO)((ObjectContent)(response.Result.Content)).Value;

                const string comment = "poCommentUpdated";
                poResponse.Comment = comment;
                poResponse.Id = 1000;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<dynamic>(new { Id = 1000, Comment = comment });
                var response2 = client.PatchAsync(string.Format("odata/PO({0})", poResponse.Id), requestMessage2);
                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.NotFound);
                Assert.IsType<ObjectContent<ODataError>>(response2.Result.Content);
            }
        }
    }
}
