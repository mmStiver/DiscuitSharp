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

namespace DiscuitSharp.Test.Unauthenticated
{
    public class UserTest : IClassFixture<NoAuthTestFixture>
    {
        readonly Task<IDiscuitClient> discClentTask;
        public UserTest(NoAuthTestFixture fixture)
        {
            discClentTask = fixture.InitializeClient();
        }

        [Fact]
        public async Task GetUser_CurrentUser_ReturnRequestedUser()
        {
            var client = await discClentTask;

            Task<DiscuitUser?> act() => client.GetAuthenticatedUser();

            var exception = await Assert.ThrowsAsync<APIRequestException>(act);
            Assert.Equal(HttpStatusCode.Unauthorized, exception.StatusCode);
            Assert.Equal("not_logged_in", exception.ErrorDetails.Code);
            Assert.Equal("User is not logged in.", exception.ErrorDetails.Message);
            Assert.Equal(401, exception.ErrorDetails.Status);
        }


        [Fact]
        public async Task GetUser_ExistingUser_ReturnRequestedUser()
        {
            var client = await discClentTask;

            var user = await client.GetUser("previnder");

            Assert.NotNull(user);
            Assert.Equal("previnder", user.Username);
        }

        [Fact]
        public async Task GetUser_UserDoesNotExist_404HttpException()
        {
            var client = await discClentTask;

            Task<DiscuitUser?> act() => client.GetUser("RomanReigns");

            var exception = await Assert.ThrowsAsync<APIRequestException>(act);
            Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
            Assert.Equal("user_not_found", exception.ErrorDetails.Code);
            Assert.Equal("User not found.", exception.ErrorDetails.Message);
            Assert.Equal(404, exception.ErrorDetails.Status);
        }
    }
}
