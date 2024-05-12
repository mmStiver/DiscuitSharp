using DiscuitDotNet.Core.Auth;
using DiscuitDotNet.Core.Utils;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DiscuitDotNet.Test.Unauthenticated.LiveServer
{
    public class APITest : IClassFixture<LiveServiceTestFixture>
    {
        private LiveServiceTestFixture context;

        public APITest(LiveServiceTestFixture fixture)
        {
            this.context = fixture;
        }

        [Fact]
        public async Task GetInitial_ServerLive_ReturnInitialResponse()
        {
            var httpResponseMessage = await context.Client.GetAsync("_initial");

            var result = await httpResponseMessage.Content.ReadAsStringAsync();

            Assert.NotNull(result);

        }

        [Fact]
        public async Task SignIn_ServerLive_ReturnInitialResponse()
        {
            _ = await context.Init();

            var request = new HttpRequestMessage(HttpMethod.Post, "_login");
            request.Headers.Add("X-Csrf-Token",context.GetToken() ?? String.Empty);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new LowercaseNamingPolicy(),
            };
            var body = JsonSerializer.Serialize(new Credentials("mmstiver", "XfPF*;-^h/OA4wWs"), options);
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            var httpResponseMessage = await context.Client.SendAsync(request);
            var user = await httpResponseMessage.Content.ReadFromJsonAsync<DiscuitUser>();
        
            Assert.NotNull(httpResponseMessage);
            Assert.NotNull(user);
        }

        [Fact]
        public async Task SignOut_ServerLive_ReturnInitialResponse()
        {
            _ = await context.Init();

            var request = new HttpRequestMessage(HttpMethod.Post, "_login?action=logout");
            request.Headers.Add("X-Csrf-Token", context.GetToken() ?? String.Empty);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new LowercaseNamingPolicy(),
            };
            var body = JsonSerializer.Serialize(new Credentials("mmstiver", "XfPF*;-^h/OA4wWs"), options);
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            var httpResponseMessage = await context.Client.SendAsync(request);
            var userstr = await httpResponseMessage.Content.ReadAsStringAsync();

            Assert.NotNull(httpResponseMessage);
            Assert.NotNull(userstr);
        }
        [Fact]
        public async Task SignOut_Unauthenticated_ApiErrorForbidden()
        {
            _ = await context.Init();

            var request = new HttpRequestMessage(HttpMethod.Post, "_login?action=logout");
            request.Headers.Add("X-Csrf-Token", context.GetToken() ?? String.Empty);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new LowercaseNamingPolicy(),
            };
            //var body = JsonSerializer.Serialize(new Credentials("mmstiver", "XfPF*;-^h/OA4wWs"), options);
            //request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            var httpResponseMessage = await context.Client.SendAsync(request);
            var userstr = await httpResponseMessage.Content.ReadAsStringAsync();

            Assert.NotNull(httpResponseMessage);
            Assert.NotNull(userstr);
        }

        [Fact]
        public async Task User_ServerLive_unauthenticated_ReturnUserResponse()
        {
            _ = await context.Init();
            
            var request = new HttpRequestMessage(HttpMethod.Get, "_user");
            request.Headers.Add("X-Csrf-Token", context.GetToken() ?? String.Empty);
            

            var httpResponseMessage = await context.Client.SendAsync(request);
            var status = () => { httpResponseMessage.EnsureSuccessStatusCode(); };
            var exception = Assert.Throws<HttpRequestException>(status);
        }
        [Fact]
        public async Task User_ServerLive_Authenticated_ReturnUserResponse()
        {
            _ = await context.Init();
            var loggedinUser = await context.Login(new Credentials("mmstiver", "XfPF*;-^h/OA4wWs"));

            var request = new HttpRequestMessage(HttpMethod.Get, "_user");
            request.Headers.Add("X-Csrf-Token", context.GetToken() ?? String.Empty);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new LowercaseNamingPolicy(),
            };
            var httpResponseMessage = await context.Client.SendAsync(request);
            var user = await httpResponseMessage.Content.ReadFromJsonAsync<DiscuitUser>();

            Assert.NotNull(httpResponseMessage);
            Assert.NotNull(user);
            Assert.Equal(user, loggedinUser);
        }
    }
}
