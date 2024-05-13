using DiscuitSharp.Core;
using DiscuitSharp.Core.Auth;
using DiscuitSharp.Core.Exceptions;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

//using DiscuitSharp.Test.Unauthenticated.Unauthorized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DiscuitSharp.Test.Unauthenticated
{
    public class LoginTest : IClassFixture<NoAuthTestFixture>
    {
        private readonly Task<IDiscuitClient> clientTask;

        public LoginTest(NoAuthTestFixture fixture)
        {
            clientTask = fixture.InitializeClient();
        }

        [Fact]
        public async Task SignIn_ValidCredentials_ReturnAuthenticatedUser()
        {
            var client = await clientTask;
            var credentials = new Credentials("mmstiver", "password123");

            var user = await client.Authenticate(credentials);
            Assert.NotNull(user);
            Assert.Equal("mmstiver", user.Username);
        }

        [Fact]
        public async Task SignIn_InvalidCredentials_ReturnAuthenticatedUser()
        {
            var client = await clientTask;
            var credentials = new Credentials("RheaRipley", "Mommi");


            Task<DiscuitUser?> act() => client.Authenticate(credentials);

            var exception = await Assert.ThrowsAsync<HttpRequestException>(act);
            Assert.Equal(HttpStatusCode.Unauthorized, exception.StatusCode);
        }

        [Fact]
        public async Task SignIn_SuspendedAccountCredentials_ForbiddenException()
        {
            var client = await clientTask;
            var credentials = new Credentials("CodyRhodes", "Nightmare");


            Task<DiscuitUser?> act() => client.Authenticate(credentials);

            var exception = await Assert.ThrowsAsync<HttpRequestException>(act);
            Assert.Equal(HttpStatusCode.Forbidden, exception.StatusCode);
        }

        [Fact]
        public async Task SignOut_InternalServerError()
        {
            var client = await clientTask;

            Task<bool?> act() => client.InvalidateAuth();

            var exception = await Assert.ThrowsAsync<APIRequestException>(act);
            Assert.Equal(HttpStatusCode.InternalServerError, exception.StatusCode);
            Assert.Equal(500, exception.ErrorDetails.Status);
            Assert.Equal("Internal Server Error", exception.ErrorDetails.Message);
            Assert.Null(exception.ErrorDetails.Code);
        }
    }
}
