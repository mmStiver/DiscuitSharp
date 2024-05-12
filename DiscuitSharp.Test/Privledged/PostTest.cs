using DiscuitDotNet.Core;
using DiscuitDotNet.Core.Content;
using DiscuitDotNet.Core.Exceptions;
using DiscuitDotNet.Test.Unauthenticated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitDotNet.Test.Privledged
{
    public class PostTest : IClassFixture<NoAuthTestFixture>
    {
        private Task<IDiscuitClient> clientTask;

        public PostTest(NoAuthTestFixture fixture)
        {
            this.clientTask = fixture.InitializeClient();
        }

        [Fact] public async Task
        UpdatePost_NoArgumentsSet_InvalidOperationException()
        {
            var client = await clientTask;
            PublicPostId postId = new PublicPostId();
            Task<Post?> act() => client.Update(postId);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(act);
            Assert.Equal("No Action Selected", exception.Message);

        }


    }
}
