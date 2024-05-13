using DiscuitSharp.Core;
using DiscuitSharp.Core.Auth;
using DiscuitSharp.Core.Exceptions;



//using DiscuitSharp.Test.Unauthenticated.Unauthorized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DiscuitSharp.Test.Authenticated
{
    public class UserTest : IClassFixture<AuthTestFixture>
    {
        readonly Task<IDiscuitClient> discClentTask;
        public UserTest(AuthTestFixture fixture)
        {
            discClentTask = fixture.AuthenticatedClient();
        }

        [Fact]
        public async Task GetUser_CurrentUser_ReturnRequestedUser()
        {
            var client = await discClentTask;

            var user = await client.GetAuthenticatedUser();

            Assert.NotNull(user);
            Assert.Equal("mmstiver", user.Username);
        }

        [Fact]
        public async Task SignOut_CurrentAuth_InvalidateLogin()
        {
            var client = await discClentTask;

            Assert.True(await client.InvalidateAuth());

            
        }
    }
}
