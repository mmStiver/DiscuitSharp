using DiscuitDotNet.Core;
using DiscuitDotNet.Core.Auth;
using DiscuitDotNet.Core.Utils;
using DiscuitDotNet.Test.Unauthenticated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DiscuitDotNet.Test.Unauthenticated.LiveServer
{
    public class LiveServiceTestFixture
    {
        public HttpClient Client { get; init; }
        public HttpClientHandler Handler { get; init; }


        public LiveServiceTestFixture()
        {
            var cookieContainer = new CookieContainer();
            Handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            Client = new HttpClient(Handler)
            {
                BaseAddress = new Uri("https://discuit.net/api/")
            };
        }
        public String? GetToken()
        {
            var uri = new Uri("https://discuit.net/api/");
            var cookies = Handler.CookieContainer.GetCookies(uri);
            return cookies.FirstOrDefault(cookie => cookie.Name == "csrftoken")?.Value;
        }

        public async Task<Initial?> Init()
        => await Client.GetFromJsonAsync<Initial>("https://discuit.net/api/_initial");

        public async Task<DiscuitUser?> Login(Credentials creds)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "_login");

            var uri = new Uri("https://discuit.net/api/");
            var cookies = Handler.CookieContainer.GetCookies(uri);
            request.Headers.Add("X-Csrf-Token", cookies.FirstOrDefault(cookie => cookie.Name == "csrftoken")?.Value);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new LowercaseNamingPolicy(),
            };
            var body = JsonSerializer.Serialize(new Credentials("mmstiver", "XfPF*;-^h/OA4wWs"), options);
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            var httpResponseMessage = await Client.SendAsync(request);
            var contentStr = await httpResponseMessage.Content.ReadAsStringAsync();
            return await httpResponseMessage.Content.ReadFromJsonAsync<DiscuitUser>();
        }

        public async Task<DiscuitUser?> User()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "_user");

            var uri = new Uri("https://discuit.net/api/");
            var cookies = Handler.CookieContainer.GetCookies(uri);
            request.Headers.Add("X-Csrf-Token", cookies.FirstOrDefault(cookie => cookie.Name == "csrftoken")?.Value);

            var httpResponseMessage = await Client.SendAsync(request);
            return await httpResponseMessage.Content.ReadFromJsonAsync<DiscuitUser>();
        }

    }
}
