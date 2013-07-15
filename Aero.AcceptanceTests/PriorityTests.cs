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
    public class PriorityTests
    {
        private HttpSelfHostServer _server;

        public PriorityTests()
        {
            _server = HttpSelfHost.GetServer();
        }

        [Fact]
        [UseDatabase]
        public void PriorityInsertGetTest()
        {
            var aogPriority = TestData.CreatePriority("AOG", "AOG");
            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Priority>(aogPriority);

                var response = client.PostAsync("odata/Priorities", requestMessage);
                Priority priorityResponse = (Priority)((ObjectContent)(response.Result.Content)).Value;

                var response2 = client.GetAsync(string.Format("odata/Priorities({0})", priorityResponse.Id));

                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.OK);
                Priority priorityResponse2 = (Priority)((ObjectContent)(response2.Result.Content)).Value;

                Assert.Equal(priorityResponse2.Code, aogPriority.Code);
                Assert.Equal(priorityResponse2.Display, aogPriority.Display);
            }
        }

        [Fact]
        [UseDatabase]
        public void PriorityInsertUpdateGetTest()
        {
            var aogPriority = TestData.CreatePriority("AOG", "AOG");
            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Priority>(aogPriority);

                var response = client.PostAsync("odata/Priorities", requestMessage);
                Priority priorityResponse = (Priority)((ObjectContent)(response.Result.Content)).Value;

                const string code = "updatedCode";
                priorityResponse.Code = code;

                const string display = "updatedDisplay";
                priorityResponse.Display = display;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<Priority>(priorityResponse);
                var response2 = client.PutAsync(string.Format("odata/Priorities({0})", priorityResponse.Id), requestMessage2);
                var response3 = client.GetAsync(string.Format("odata/Priorities({0})", priorityResponse.Id));

                Assert.Equal(response3.Result.StatusCode, HttpStatusCode.OK);
                Priority priorityResponse3 = (Priority)((ObjectContent)(response3.Result.Content)).Value;

                Assert.Equal(priorityResponse3.Code, code);
                Assert.Equal(priorityResponse3.Display, display);
            }
        }

        [Fact]
        [UseDatabase]
        public void PriorityInsertPatchGetTest()
        {
            var aogPriority = TestData.CreatePriority("AOG", "AOG");
            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Priority>(aogPriority);

                var response = client.PostAsync("odata/Priorities", requestMessage);
                Priority priorityResponse = (Priority)((ObjectContent)(response.Result.Content)).Value;

                const string code = "updatedCode";
                priorityResponse.Code = code;

                const string display = "updatedDisplay";
                priorityResponse.Display = display;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<dynamic>(new { Code = code, Display = display });
                var response2 = client.PatchAsync(string.Format("odata/Priorities({0})", priorityResponse.Id), requestMessage2);
                var response3 = client.GetAsync(string.Format("odata/Priorities({0})", priorityResponse.Id));

                Assert.Equal(response3.Result.StatusCode, HttpStatusCode.OK);
                Priority priorityResponse3 = (Priority)((ObjectContent)(response3.Result.Content)).Value;

                Assert.Equal(priorityResponse3.Code, code);
                Assert.Equal(priorityResponse3.Display, display);
            }
        }

        [Fact]
        [UseDatabase]
        public void PriorityExceptionDuringInsertTest()
        {
            var aogPriority = TestData.CreatePriority("AOG", null);

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Priority>(aogPriority);

                var response = client.PostAsync("odata/Priorities", requestMessage);
                Assert.Equal(response.Result.StatusCode, HttpStatusCode.InternalServerError);
                Assert.IsType<ObjectContent<ODataError>>(response.Result.Content);
            }
        }

        [Fact]
        [UseDatabase]
        public void PriorityExceptionDuringUpdateMissingDataTest()
        {
            var aogPriority = TestData.CreatePriority("AOG", "AOG");

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Priority>(aogPriority);

                var response = client.PostAsync("odata/Priorities", requestMessage);
                Priority priorityResponse = (Priority)((ObjectContent)(response.Result.Content)).Value;

                priorityResponse.Code = null;
                priorityResponse.Display = null;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<Priority>(priorityResponse);
                var response2 = client.PutAsync(string.Format("odata/Priorities({0})", response.Id), requestMessage2);
                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.InternalServerError);
                Assert.IsType<ObjectContent<ODataError>>(response2.Result.Content); 
            }
        }

        [Fact]
        [UseDatabase]
        public void PriorityExceptionDuringUpdateMissingIdTest()
        {
            var aogPriority = TestData.CreatePriority("AOG", "AOG");
            
            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Priority>(aogPriority);
                var response = client.PostAsync("odata/Priorities", requestMessage);
                Priority priorityResponse = (Priority)((ObjectContent)(response.Result.Content)).Value;

                const string code = "updatedcode";
                priorityResponse.Code = code;

                const string display = "updateddisplay";
                priorityResponse.Display = display;

                priorityResponse.Id = 1000;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<Priority>(priorityResponse);
                var response2 = client.PutAsync(string.Format("odata/Priorities({0})", priorityResponse.Id), requestMessage2);
                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.NotFound);
                Assert.IsType<ObjectContent<ODataError>>(response2.Result.Content);

            }
        }

        [Fact]
        [UseDatabase]
        public void PriorityExceptionDuringPatchMissingIdTest()
        {
            var aogPriority = TestData.CreatePriority("AOG", "AOG");

            using (var client = new HttpClient(_server))
            {
                client.BaseAddress = HttpSelfHost.BaseAddress;
                var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Priority>(aogPriority);
                var response = client.PostAsync("odata/Priorities", requestMessage);
                Priority contactResponse = (Priority)((ObjectContent)(response.Result.Content)).Value;

                const string code = "updatedcode";
                contactResponse.Code = code;

                const string display = "updateddisplay";
                contactResponse.Display = display;

                contactResponse.Id = 1000;

                var requestMessage2 = HttpSelfHost.CreateHttpRequestMessage<dynamic>(new { Code = code, Display = display });
                var response2 = client.PatchAsync(string.Format("odata/Priorities({0})", contactResponse.Id), requestMessage2);
                Assert.Equal(response2.Result.StatusCode, HttpStatusCode.NotFound);
                Assert.IsType<ObjectContent<ODataError>>(response2.Result.Content);

            }
        }    
   
    }
}
