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
    public class ContactTests
    {
        private HttpSelfHostServer _server;

        public ContactTests()
        {
            _server = HttpSelfHost.GetServer();
        }

        [Fact]
        [UseDatabase]
        public void ContactInsertGetTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Contact>(contact);

                var response = client.PostAsync("odata/Contacts", requestMessage);
                Contact contactResponse = (Contact)((ObjectContent)(response.Result.Content)).Value;

                var response2 = client.GetAsync(string.Format("odata/Contacts({0})", contactResponse.Id));

                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.OK);
                Contact contactResponse2 = (Contact)((ObjectContent)(response2.Result.Content)).Value;

                Assert.Equal(contactResponse2.Address1, contact.Address1);
                Assert.Equal(contactResponse2.Address2, contact.Address2);
                Assert.Equal(contactResponse2.Address3, contact.Address3);
                Assert.Equal(contactResponse2.City, contact.City);
                Assert.Equal(contactResponse2.Country, contact.Country);
                Assert.Equal(contactResponse2.County, contact.County);
                Assert.Equal(contactResponse2.Email, contact.Email);
                Assert.Equal(contactResponse2.Fax, contact.Fax);
                Assert.Equal(contactResponse2.Name, contact.Name);
                Assert.Equal(contactResponse2.Phone, contact.Phone);
                Assert.Equal(contactResponse2.PostCode, contact.PostCode);
                Assert.Equal(contactResponse2.Type, contact.Type);
            }
        }

        [Fact]
        [UseDatabase]
        public void ContactInsertUpdateGetTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Contact>(contact);

                var response = client.PostAsync("odata/Contacts", requestMessage);
                Contact contactResponse = (Contact)((ObjectContent)(response.Result.Content)).Value;

                const string addressLine1 = "addressLine1Updated";
                contactResponse.Address1 = addressLine1;

                const string addressLine2 = "addressLine2Updated";
                contactResponse.Address2 = addressLine2;

                const string addressLine3 = "addressLine3Updated";
                contactResponse.Address3 = addressLine3;

                const string city = "cityUpdated";
                contactResponse.City = city;

                const string country = "countryUpdated";
                contactResponse.Country = country;

                const string county = "countyUpdated";
                contactResponse.County = county;

                const string email = "emailUpdated";
                contactResponse.Email = email;

                const string fax = "faxUpdated";
                contactResponse.Fax = fax;

                const string name = "nameUpdated";
                contactResponse.Name = name;

                const string phone = "phoneUpdated";
                contactResponse.Phone = phone;

                const string postCode = "postCodeUpdated";
                contactResponse.PostCode = postCode;

                const ContactType contactType = ContactType.Businness;
                contactResponse.Type = contactType;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<Contact>(contactResponse);
                var response2 = client.PutAsync(string.Format("odata/Contacts({0})", contactResponse.Id), requestMessage2);
                var response3 = client.GetAsync(string.Format("odata/Contacts({0})", contactResponse.Id));

                Assert.Equal(response3.Result.StatusCode, HttpStatusCode.OK);
                Contact contactResponse3 = (Contact)((ObjectContent)(response3.Result.Content)).Value;

                Assert.Equal(contactResponse3.Address1, addressLine1);
                Assert.Equal(contactResponse3.Address2, addressLine2);
                Assert.Equal(contactResponse3.Address3, addressLine3);
                Assert.Equal(contactResponse3.City, city);
                Assert.Equal(contactResponse3.Country, country);
                Assert.Equal(contactResponse3.County, county);
                Assert.Equal(contactResponse3.Email, email);
                Assert.Equal(contactResponse3.Fax, fax);
                Assert.Equal(contactResponse3.Name, name);
                Assert.Equal(contactResponse3.Phone, phone);
                Assert.Equal(contactResponse3.PostCode, postCode);
                Assert.Equal(contactResponse3.Type, contactType);
            }
        }

        [Fact]
        [UseDatabase]
        public void ContactInsertPatchGetTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);
            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Contact>(contact);

                var response = client.PostAsync("odata/Contacts", requestMessage);
                Contact contactResponse = (Contact)((ObjectContent)(response.Result.Content)).Value;

                const string addressLine1 = "addressLine1Updated";
                contactResponse.Address1 = addressLine1;

                const string addressLine2 = "addressLine2Updated";
                contactResponse.Address2 = addressLine2;

                const string addressLine3 = "addressLine3Updated";
                contactResponse.Address3 = addressLine3;

                const string city = "cityUpdated";
                contactResponse.City = city;

                const string country = "countryUpdated";
                contactResponse.Country = country;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<dynamic>(new { Address1 = addressLine1, Address2 = addressLine2, Address3 = addressLine3, City = city, Country = country });
                var response2 = client.PatchAsync(string.Format("odata/Contacts({0})", contactResponse.Id), requestMessage2);
                var response3 = client.GetAsync(string.Format("odata/Contacts({0})", contactResponse.Id));

                Assert.Equal(response3.Result.StatusCode, HttpStatusCode.OK);
                Contact contactResponse3 = (Contact)((ObjectContent)(response3.Result.Content)).Value;

                Assert.Equal(contactResponse3.Address1, addressLine1);
                Assert.Equal(contactResponse3.Address2, addressLine2);
                Assert.Equal(contactResponse3.Address3, addressLine3);
                Assert.Equal(contactResponse3.City, city);
                Assert.Equal(contactResponse3.Country, country);
            }
        }

        [Fact]
        [UseDatabase]
        public void ContactExceptionDuringInsertTest()
        {
            var contact = TestData.CreateContact(null, null, "addressLine3", "city", "country", "county", null, "fax", null, "phone", "postCode", ContactType.Home);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Contact>(contact);

                var response = client.PostAsync("odata/Contacts", requestMessage);
                Assert.Equal(response.Result.StatusCode, HttpStatusCode.InternalServerError);
                Assert.IsType<ObjectContent<ODataError>>(response.Result.Content);
            }
        }

        [Fact]
        [UseDatabase]
        public void ContactExceptionDuringUpdateMissingDataTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Contact>(contact);

                var response = client.PostAsync("odata/Contacts", requestMessage);
                Contact contactResponse = (Contact)((ObjectContent)(response.Result.Content)).Value;

                contactResponse.Email = null;
                contactResponse.Name = null;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<Contact>(contactResponse);
                var response2 = client.PutAsync(string.Format("odata/Contacts({0})", response.Id), requestMessage2);
                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.InternalServerError);
                Assert.IsType<ObjectContent<ODataError>>(response2.Result.Content); 
            }
        }

        [Fact]
        [UseDatabase]
        public void ContactExceptionDuringUpdateMissingIdTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Contact>(contact);
                var response = client.PostAsync("odata/Contacts", requestMessage);
                Contact contactResponse = (Contact)((ObjectContent)(response.Result.Content)).Value;

                const string addressLine1 = "addressLine1Updated";
                contactResponse.Address1 = addressLine1;

                const string addressLine2 = "addressLine2Updated";
                contactResponse.Address2 = addressLine2;

                const string addressLine3 = "addressLine3Updated";
                contactResponse.Address3 = addressLine3;

                const string city = "cityUpdated";
                contactResponse.City = city;

                contactResponse.Id = 1000;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<Contact>(contactResponse);
                var response2 = client.PutAsync(string.Format("odata/Contacts({0})", contactResponse.Id), requestMessage2);
                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.NotFound);
                Assert.IsType<ObjectContent<ODataError>>(response2.Result.Content);

            }
        }

        [Fact]
        [UseDatabase]
        public void ContactExceptionDuringPatchMissingIdTest()
        {
            var contact = TestData.CreateContact("addressLine1", "addressLine2", "addressLine3", "city", "country", "county", "email", "fax", "name", "phone", "postCode", ContactType.Home);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Contact>(contact);
                var response = client.PostAsync("odata/Contacts", requestMessage);
                Contact contactResponse = (Contact)((ObjectContent)(response.Result.Content)).Value;

                const string addressLine1 = "addressLine1Updated";
                contactResponse.Address1 = addressLine1;

                const string addressLine2 = "addressLine2Updated";
                contactResponse.Address2 = addressLine2;

                const string addressLine3 = "addressLine3Updated";
                contactResponse.Address3 = addressLine3;

                const string city = "cityUpdated";
                contactResponse.City = city;

                contactResponse.Id = 1000;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<Contact>(contactResponse);
                var response2 = client.PatchAsync(string.Format("odata/Contacts({0})", contactResponse.Id), requestMessage2);
                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.NotFound);
                Assert.IsType<ObjectContent<ODataError>>(response2.Result.Content);

            }
        }    
   
    }
}
