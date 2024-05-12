using DiscuitDotNet.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitDotNet.Test.Unauthenticated.Unauthorized
{
    public class InvalidLoginTest : IClassFixture<UnauthorizedTestFixture>
    {
        readonly IDiscuitClient discClent;
        public InvalidLoginTest(UnauthorizedTestFixture fixture)
        {
            discClent = fixture.CreateClient();

        }

        [Fact]
        public async Task SignIn_InvalidCredentials_HttpRequestException()
        {
            var credentials = new Credentials("name", "password");

            Task<DiscuitUser?> act() => discClent.GetAuthenticatedUser();

            await Assert.ThrowsAsync<HttpRequestException>(act);
        }
    }
}
