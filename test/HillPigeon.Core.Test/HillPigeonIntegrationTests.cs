using HillPigeon.Core.Test.Infrastructure;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace HillPigeon.Core.Test
{
    public class HillPigeonIntegrationTests : WebHostTest
    {
        [Fact]
        public void SimpleRequest_Get()
        {
            var request = this.Server.CreateRequest("api/TestService");
            var response = request.GetAsync().GetAwaiter().GetResult();
            var message = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("[\"value1\",\"value2\"]", message);
        }

        [Fact]
        public void SimpleRequest_Get_FromRoute()
        {
            int id = 1;
            var request = this.Server.CreateRequest("api/TestService/"+ id);
            var response = request.GetAsync().GetAwaiter().GetResult();
            var message = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("value " + id, message);
        }

        [Fact]
        public void SimpleRequest_Post()
        {
            int value = 1;
            var request = this.Server.CreateRequest("api/TestService/?value="+ value);
            var response = request.PostAsync().GetAwaiter().GetResult();
            var message = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Post:value " + value, message);
        }

        [Fact]
        public void SimpleRequest_Post_FromBody()
        {
            List<string> values = new List<string>()
            {
                "abc","123"
            };
            var request = this.Server.CreateRequest("api/TestService/add");
            request.And(mReq =>
            {
                var json = JsonConvert.SerializeObject(values);
                mReq.Content = new StringContent(json, Encoding.UTF8, "application/json");
            });
            var response = request.PostAsync().GetAwaiter().GetResult();
            var message = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Post:value " + values.Count, message);
        }
    }
}
