using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;


namespace BrewCoffeeApi.Tests
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var json = @"{
            ""main"": {
                ""temp"": 30.1
            }
        }";

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json)
            };

            return Task.FromResult(response);
        }
    }
}
