using DiscuitSharp.Core;
using DiscuitSharp.Core.Group;


namespace DiscuitSharp.Test.Authenticated
{
    public class CommunityTest : IClassFixture<AuthTestFixture>
    {
        readonly Task<IDiscuitClient> discClentTask;
        public CommunityTest(AuthTestFixture fixture)
        {
            discClentTask = fixture.AuthenticatedClient();
        }

        [Fact]
        public async Task GetCommunities_RequestSubscribedFeed_ReturnCommunityList()
        {
            var client = await discClentTask;

            var communities = await client.GetCommunities(QueryParams.Subscribed);

            Assert.NotNull(communities);
            Assert.Collection<Community>(communities, 
                c => Assert.True(c?.UserJoined ?? false),
                c => Assert.True(c?.UserJoined ?? false),
                c => Assert.True(c?.UserJoined ?? false)
            );
        }
    }
}
