using DiscuitSharp.Core;
using System.Net;
using System.Net.Http;
using System.Text.Json.Nodes;

namespace DiscuitSharp.Test
{
    internal class FakeHttpClientFactory : IHttpClientFactory
    {
        FakeMessageHandler handler;
        public FakeHttpClientFactory(Func<Uri?, string, JsonObject?, (HttpStatusCode, string)> ResponseContent)
        {

            this.handler = new FakeMessageHandler(ResponseContent)
            {
                CookieContainer = new CookieContainer(),
                UseCookies = true, 
            }; 
        }

        public HttpClient CreateClient(string name)
            => new HttpClient(handler, false);
    }
}
