using DiscuitSharp.Core;
using DiscuitSharp.Core.Content;
using DiscuitSharp.Core.Exceptions;
using DiscuitSharp.Core.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitSharp.Test.Unauthenticated
{
    public class CommunityTest : IClassFixture<NoAuthTestFixture>
    {
        private Task<IDiscuitClient> clientTask;

        public CommunityTest(NoAuthTestFixture fixture)
        {
            this.clientTask = fixture.InitializeClient();
        }

        [Fact]
        public async Task GetCommunity_GetById_ReturnCommunity()
        {
            var client = await clientTask;
            CommunityId Id = new CommunityId("176ef2e09e28701c14c0c148");
            Community? community = await client.Get(Id);

            Assert.NotNull(community);

            // Asserting the basic properties
            Assert.Equal(new("17692e122def73f25bd757e0"), community.Id);
            Assert.Equal(new("17692e04a6576d682930a4f5"), community.UserId);
            Assert.Equal("general", community.Name);
            Assert.False(community.Nsfw);
            Assert.Equal("General chat community. For everything that doesn't belong in other communities.", community.About);
            Assert.Equal(5990, community.NoMembers);

            // Asserting properties of the profile picture
            Assert.NotNull(community.ProPic);
            Assert.Equal("17c0becfc4aacacd57e276b5", community.ProPic.Id);
            Assert.Equal("jpeg", community.ProPic.Format);
            Assert.Equal("image/jpeg", community.ProPic.Mimetype);
            Assert.Equal("/images/17c0becfc4aacacd57e276b5.jpeg?sig=_Oj5E6isIecCt_9eQ-3asAi1JlDB6dBfd6bzlLTTAL8", community.ProPic.Url);

            // Asserting properties of the banner image
            Assert.NotNull(community.BannerImage);
            Assert.Equal("17c0c06dba3d07cdde20d30b", community.BannerImage.Id);
            Assert.Equal("jpeg", community.BannerImage.Format);
            Assert.Equal("image/jpeg", community.BannerImage.Mimetype);
            Assert.Equal("/images/17c0c06dba3d07cdde20d30b.jpeg?sig=g8j5VXaVXu5LFe_HZHESIbmeY7BAionS8YrohJKn284", community.BannerImage.Url);

            Assert.Equal("2024-03-27T09:15:45Z", community.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            Assert.Null(community.DeletedAt);

            // Asserting boolean flags
            Assert.False(community.IsDefault);
            Assert.True(community.UserJoined);
            Assert.True(community.UserMod);
            Assert.False(community.IsMuted);
        }

        [Fact]
        public async Task GetCommunity_GetById_CommunityContainsRules()
        {
            var client = await clientTask;
            CommunityId Id = new CommunityId("176ef2e09e28701c14c0c148");
            Community? community = await client.Get(Id);

            Assert.NotNull(community);
           // Assert.NotNull(community.Mods);
           // Assert.Single(community.Mods);
           // Assert.Equal(new ("17692e04a6576d682930a4f5"), community.Mods[0].Id);
           // Assert.Equal("previnder", community.Mods[0].Username);
        }

        [Fact]
        public async Task GetCommunity_GetById_CommunityContainsMods()
        {
            var client = await clientTask;
            CommunityId Id = new CommunityId("176ef2e09e28701c14c0c148");
            Community? community = await client.Get(Id);

            Assert.NotNull(community);
            Assert.NotNull(community.Rules);
            Assert.Single(community.Rules);
            var rule = community.Rules[0]; 
            Assert.Equal(35, rule.Id);
            Assert.Equal("No bigotry", rule.Rule);
            Assert.Equal("", rule.Description);
            Assert.Equal(new("17692e122def73f25bd757e0"), rule.CommunityId);
            Assert.Equal("176c98ce3fb02f05a7f3a24d", rule.CreatedBy);
            Assert.Equal("2023-06-30T18:05:59Z", rule.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"));

        }

        [Fact]
        public async Task GetCommunity_GetByName_ReturnCommunity()
        {
            var client = await clientTask;
            var community = await client.GetCommunity("general");

            Assert.NotNull(community);
            Assert.Equal("17692e122def73f25bd757e0", community.Id.ToString());
            Assert.Equal(new("17692e04a6576d682930a4f5"), community.UserId);
            Assert.Equal("general", community.Name);
        }

        [Fact]
        public async Task GetCommunity_CommunityNotExist_ThrowNotFoundException()
        {
            var client = await clientTask;
            
            Task<Community?> act() => client.GetCommunity("notExist");

            var exception = await Assert.ThrowsAsync<APIRequestException>(act);
            Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
            Assert.Equal("community/not-found", exception.ErrorDetails.Code);
            Assert.Equal("Community not found.", exception.ErrorDetails.Message);
            Assert.Equal(404, exception.ErrorDetails.Status);
        }

        [Fact]
        public async Task GetCommunities_EmptyGetRequest_ListAllCommunities()
        {
            var client = await clientTask;

            var communities = await client.GetCommunities();

            Assert.NotNull(communities);
            Assert.Equal(9, communities.Count());
        }

        [Fact]
        public async Task GetCommunities_RequestAll_ListAllCommunities()
        {
            var client = await clientTask;

            var communities = await client.GetCommunities(QueryParams.All);

            Assert.NotNull(communities);
            Assert.Equal(9, communities.Count());
        }

        [Fact]
        public async Task GetCommunities_RequestDefault_ListDefaultCommunities()
        {
            var client = await clientTask;

            var communities = await client.GetCommunities(QueryParams.Default);

            Assert.NotNull(communities);
            Assert.Equal(3, communities.Count());
        }

        [Fact]
        public async Task GetCommunities_RequestSubscribed_NotLoggedInError()
        {
            var client = await clientTask;

            Task<IEnumerable<Community>?> act() => client.GetCommunities(QueryParams.Subscribed);

            var exception = await Assert.ThrowsAsync<APIRequestException>(act);
            Assert.Equal(HttpStatusCode.Unauthorized, exception.StatusCode);
            Assert.Equal("not_logged_in", exception.ErrorDetails.Code);
            Assert.Equal("User is not logged in.", exception.ErrorDetails.Message);
            Assert.Equal(401, exception.ErrorDetails.Status);
        }

        [Fact]
        public async Task GetCommunities_QueryParam_ListFilteredDefaultCommunities()
        {
            var client = await clientTask;

            var communities = await client.GetCommunities("ask", QueryParams.Default);
            Assert.NotNull(communities);
            var found = communities.Where(c => c.Name!.ToLower().Contains("ask"));
            Assert.Equal(2, communities.Count());
        }
    
    
    }
}
