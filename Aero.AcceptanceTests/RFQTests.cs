using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;
using Aero.Model;
using Microsoft.Data.OData;
using Xunit;

namespace Aero.AcceptanceTests
{
    public class RFQTests
    {
        private HttpSelfHostServer _server;

        public RFQTests()
        {
            _server = HttpSelfHost.GetServer();
        }

        [Fact]
        [UseDatabase]
        public void RFQInsertGetTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);
            var part = TestData.CreatePart(Condition.New, "description", "model", "nsn", "partNumber", 1010, 12, "source", new DateTime(2010, 10, 10), vendor);
            var customerContact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var customer = TestData.CreateCustomer("customerName", "userName", customerContact);
            var aogPriority = TestData.CreatePriority("AOG", "AOG");
            var routinePriority = TestData.CreatePriority("Routine", "Routine");
            var highPriority = TestData.CreatePriority("High", "High");
            var rfqOpenState = TestData.CreateRfqState("Open", "Open");
            var rfq = TestData.CreateRFQ("RFQComment", new DateTime(2012, 10, 10), part, aogPriority, 12, "your_ref", new DateTime(2013, 10, 10), customer, rfqOpenState);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<RFQ>(rfq);
                var response = client.PostAsync("odata/RFQ", requestMessage);
                RFQ rfqResponse = (RFQ)((ObjectContent)(response.Result.Content)).Value;

                var response2 = client.GetAsync(string.Format("odata/RFQ({0})", rfqResponse.Id));

                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.OK);
                RFQ rfqResponse2 = (RFQ)((ObjectContent)(response2.Result.Content)).Value;

                Assert.Equal(rfqResponse2.Comment, rfq.Comment);
                Assert.Equal(rfqResponse2.NeedBy, rfq.NeedBy);
                Assert.Equal(rfqResponse2.Priority.Code, rfq.Priority.Code);
                Assert.Equal(rfqResponse2.Qty, rfq.Qty);
                Assert.Equal(rfqResponse2.YourRef, rfq.YourRef);
                Assert.Equal(rfqResponse2.Part.Description, part.Description);
                Assert.Equal(rfqResponse2.Customer.Name, customer.Name);
            }
        }

        [Fact]
        [UseDatabase]
        public void RFQUpdateGetTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);
            var part = TestData.CreatePart(Condition.New, "description", "model", "nsn", "partNumber", 1010, 12, "source", new DateTime(2010, 10, 10), vendor);
            var customerContact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var customer = TestData.CreateCustomer("customerName", "userName", customerContact);
            var aogPriority = TestData.CreatePriority("AOG", "AOG");
            var routinePriority = TestData.CreatePriority("Routine", "Routine");
            var highPriority = TestData.CreatePriority("High", "High");
            var rfqOpenState = TestData.CreateRfqState("Open", "Open");
            var rfq = TestData.CreateRFQ("RFQComment", new DateTime(2012, 10, 10), part, aogPriority, 12, "your_ref", new DateTime(2013, 10, 10), customer, rfqOpenState);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<RFQ>(rfq);
                var response = client.PostAsync("odata/RFQ", requestMessage);
                RFQ rfqResponse = (RFQ)((ObjectContent)(response.Result.Content)).Value;

                const string comment = "rfqCommentUpdated";
                rfqResponse.Comment = comment;

                DateTime needBy = new DateTime(2012, 02, 02);
                rfqResponse.NeedBy = needBy;

                Priority priotity = highPriority;
                //rfqResponse.Priority = priotity;

                const short qty = 22;
                rfqResponse.Qty = qty;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<RFQ>(rfqResponse);
                var response2 = client.PutAsync(string.Format("odata/RFQ({0})", rfqResponse.Id), requestMessage2);
                var response3 = client.GetAsync(string.Format("odata/RFQ({0})", rfqResponse.Id));

                Assert.Equal(response3.Result.StatusCode, HttpStatusCode.OK);
                RFQ rfqResponse3 = (RFQ)((ObjectContent)(response3.Result.Content)).Value;

                Assert.Equal(rfqResponse3.Comment, comment);
                Assert.Equal(rfqResponse3.NeedBy, needBy);
                //Assert.Equal(rfqResponse3.Priority.Code, priotity.Code);
                Assert.Equal(rfqResponse3.Qty, qty);
            }
        }

        [Fact]
        [UseDatabase]
        public void RFQPatchGetTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);
            var part = TestData.CreatePart(Condition.New, "description", "model", "nsn", "partNumber", 1010, 12, "source", new DateTime(2010, 10, 10), vendor);
            var customerContact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var customer = TestData.CreateCustomer("customerName", "userName", customerContact);
            var aogPriority = TestData.CreatePriority("AOG", "AOG");
            var routinePriority = TestData.CreatePriority("Routine", "Routine");
            var highPriority = TestData.CreatePriority("High", "High");
            var rfqOpenState = TestData.CreateRfqState("Open", "Open");
            var rfq = TestData.CreateRFQ("RFQComment", new DateTime(2012, 10, 10), part, aogPriority, 12, "your_ref", new DateTime(2013, 10, 10), customer, rfqOpenState);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<RFQ>(rfq);
                var response = client.PostAsync("odata/RFQ", requestMessage);
                RFQ rfqResponse = (RFQ)((ObjectContent)(response.Result.Content)).Value;

                const string comment = "rfqCommentUpdated";
                DateTime needBy = new DateTime(2012, 02, 02);
                const short qty = 22;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<dynamic>(new { Comment = comment, NeedBy = needBy, Qty = qty });
                var response2 = client.PatchAsync(string.Format("odata/RFQ({0})", rfqResponse.Id), requestMessage2);
                var response3 = client.GetAsync(string.Format("odata/RFQ({0})", rfqResponse.Id));

                Assert.Equal(response3.Result.StatusCode, HttpStatusCode.OK);
                RFQ rfqResponse3 = (RFQ)((ObjectContent)(response3.Result.Content)).Value;

                Assert.Equal(rfqResponse3.Comment, comment);
                Assert.Equal(rfqResponse3.NeedBy, needBy);
                Assert.Equal(rfqResponse3.Qty, qty);
            }
        }

        [Fact]
        [UseDatabase]
        public void RFQInsertGetDeleteTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);
            var part = TestData.CreatePart(Condition.New, "description", "model", "nsn", "partNumber", 1010, 12, "source", new DateTime(2010, 10, 10), vendor);
            var customerContact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var customer = TestData.CreateCustomer("customerName", "userName", customerContact);
            var aogPriority = TestData.CreatePriority("AOG", "AOG");
            var routinePriority = TestData.CreatePriority("Routine", "Routine");
            var highPriority = TestData.CreatePriority("High", "High");
            var rfqOpenState = TestData.CreateRfqState("Open", "Open");
            var rfq = TestData.CreateRFQ("RFQComment", new DateTime(2012, 10, 10), part, aogPriority, 12, "your_ref", new DateTime(2013, 10, 10), customer, rfqOpenState);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<RFQ>(rfq);
                var response = client.PostAsync("odata/RFQ", requestMessage);
                RFQ rfqResponse = (RFQ)((ObjectContent)(response.Result.Content)).Value;

                var response2 = client.GetAsync(string.Format("odata/RFQ({0})", rfqResponse.Id));
                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.OK);

                var response3 = client.DeleteAsync(string.Format("odata/RFQ({0})", rfqResponse.Id));
                var response4 = client.GetAsync(string.Format("odata/RFQ({0})", rfqResponse.Id));
                Assert.Equal(response4.Result.StatusCode, HttpStatusCode.NotFound);
            }
        }

        [Fact]
        [UseDatabase]
        public void RFQExceptionDuringInsertTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);
            var part = TestData.CreatePart(Condition.New, "description", "model", "nsn", "partNumber", 1010, 12, "source", new DateTime(2010, 10, 10), vendor);
            var customerContact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var customer = TestData.CreateCustomer("customerName", "userName", customerContact);
            var aogPriority = TestData.CreatePriority("AOG", "AOG");
            var routinePriority = TestData.CreatePriority("Routine", "Routine");
            var highPriority = TestData.CreatePriority("High", "High");
            var rfqOpenState = TestData.CreateRfqState("Open", "Open");
            var rfq = TestData.CreateRFQ(null, new DateTime(2012, 10, 10), part, aogPriority, 12, "your_ref", new DateTime(2013, 10, 10), customer, null);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<RFQ>(rfq);
                var response = client.PostAsync("odata/RFQ", requestMessage);
                Assert.Equal(response.Result.StatusCode, HttpStatusCode.InternalServerError);
                Assert.IsType<ObjectContent<HttpError>>(response.Result.Content);
            }
        }


        [Fact]
        [UseDatabase]
        public void RFQExceptionDuringUpdateMissingDataTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);
            var part = TestData.CreatePart(Condition.New, "description", "model", "nsn", "partNumber", 1010, 12, "source", new DateTime(2010, 10, 10), vendor);
            var customerContact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var customer = TestData.CreateCustomer("customerName", "userName", customerContact);
            var aogPriority = TestData.CreatePriority("AOG", "AOG");
            var routinePriority = TestData.CreatePriority("Routine", "Routine");
            var highPriority = TestData.CreatePriority("High", "High");
            var rfqOpenState = TestData.CreateRfqState("Open", "Open");
            var rfq = TestData.CreateRFQ("RFQComment", new DateTime(2012, 10, 10), part, aogPriority, 12, "your_ref", new DateTime(2013, 10, 10), customer, rfqOpenState);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<RFQ>(rfq);
                var response = client.PostAsync("odata/RFQ", requestMessage);
                RFQ rfqResponse = (RFQ)((ObjectContent)(response.Result.Content)).Value;

                rfqResponse.NeedBy = null;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<RFQ>(rfqResponse);
                var response2 = client.PutAsync(string.Format("odata/RFQ({0})", rfqResponse.Id), requestMessage2);
                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.NoContent);
            }
        }


        [Fact]
        [UseDatabase]
        public void RFQExceptionDuringUpdateMissingIdTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);
            var part = TestData.CreatePart(Condition.New, "description", "model", "nsn", "partNumber", 1010, 12, "source", new DateTime(2010, 10, 10), vendor);
            var customerContact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var customer = TestData.CreateCustomer("customerName", "userName", customerContact);
            var aogPriority = TestData.CreatePriority("AOG", "AOG");
            var routinePriority = TestData.CreatePriority("Routine", "Routine");
            var highPriority = TestData.CreatePriority("High", "High");
            var rfqOpenState = TestData.CreateRfqState("Open", "Open");
            var rfq = TestData.CreateRFQ("RFQComment", new DateTime(2012, 10, 10), part, aogPriority, 12, "your_ref", new DateTime(2013, 10, 10), customer, rfqOpenState);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<RFQ>(rfq);
                var response = client.PostAsync("odata/RFQ", requestMessage);
                RFQ rfqResponse = (RFQ)((ObjectContent)(response.Result.Content)).Value;

                rfqResponse.Comment = "rfqcommentUpdated";
                rfqResponse.Id = 1000;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<RFQ>(rfqResponse);
                var response2 = client.PutAsync(string.Format("odata/RFQ({0})", rfqResponse.Id), requestMessage2);
                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.NotFound);
                Assert.IsType<ObjectContent<ODataError>>(response2.Result.Content);
            }
        }

        [Fact]
        [UseDatabase]
        public void RFQExceptionDuringPatchMissingIdTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var vendor = TestData.CreateVendor("VendorName", contact);
            var part = TestData.CreatePart(Condition.New, "description", "model", "nsn", "partNumber", 1010, 12, "source", new DateTime(2010, 10, 10), vendor);
            var customerContact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            var customer = TestData.CreateCustomer("customerName", "userName", customerContact);
            var aogPriority = TestData.CreatePriority("AOG", "AOG");
            var routinePriority = TestData.CreatePriority("Routine", "Routine");
            var highPriority = TestData.CreatePriority("High", "High");
            var rfqOpenState = TestData.CreateRfqState("Open", "Open");
            var rfq = TestData.CreateRFQ("RFQComment", new DateTime(2012, 10, 10), part, aogPriority, 12, "your_ref", new DateTime(2013, 10, 10), customer, rfqOpenState);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<RFQ>(rfq);
                var response = client.PostAsync("odata/RFQ", requestMessage);
                RFQ rfqResponse = (RFQ)((ObjectContent)(response.Result.Content)).Value;

                const string comment = "rfqCommentUpdated";
                rfqResponse.Comment = comment;
                rfqResponse.Id = 1000;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<dynamic>(new { Id = 1000, Comment = comment });
                var response2 = client.PatchAsync(string.Format("odata/RFQ({0})", rfqResponse.Id), requestMessage2);
                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.NotFound);
                Assert.IsType<ObjectContent<ODataError>>(response2.Result.Content);
            }
        }
    }
}
