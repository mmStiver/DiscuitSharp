using DiscuitSharp.Core;
using DiscuitSharp.Core.Auth;
using DiscuitSharp.Core.Common;
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
    public class CancellationTest : IClassFixture<NoAuthTestFixture>
    {
        readonly Task<IDiscuitClient> clientTask;
        readonly CancellationTokenSource cts = new CancellationTokenSource();
        public CancellationTest(NoAuthTestFixture fixture)
        {
            clientTask = fixture.InitializeClient();
        }


        [Fact]
        public async Task GetInitial_SendCancellationRequest_ThrowCancellationException()
        {
            var client = await clientTask;
            CancellationToken ct = cts.Token;
            Task<Initial?> act() => client.GetInitial(ct);
            cts.Cancel();
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

        [Fact]
        public async Task Authenticate_SendCancellationRequest_ThrowCancellationException()
        {
            var client = await clientTask;
            CancellationToken ct = cts.Token;

            Task<DiscuitUser?> act() => client.Authenticate(new Core.Auth.Credentials("",""), ct);
            cts.Cancel();
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

        [Fact]
        public async Task GetUser_SendCancellationRequest_ThrowCancellationException()
        {
            var client = await clientTask;
            CancellationToken ct = cts.Token;

            Task<DiscuitUser?> act() => client.GetUser("user", ct);
            cts.Cancel();
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

        [Fact]
        public async Task Get_SendCancellationRequest_ThrowsCancellationException()
        {
            var client = await clientTask;
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken ct = cts.Token;

            Task<Post?> act() => client.Get(new PublicPostId(), ct);
            cts.Cancel();
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

        [Fact]
        public async Task GetPosts_SendCancellationRequest_ThrowsCancellationException()
        {
            var client = await clientTask;
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken ct = cts.Token;

            Task<Cursor<Post>?> act() => client.GetPosts(null, null, null, null, ct);

            cts.Cancel();
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

        [Fact]
        public async Task GetPostsForCommunity_SendCancellationRequest_ThrowsCancellationException()
        {
            var client = await clientTask;
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken ct = cts.Token;

            Task<Cursor<Post>?> act() => client.GetPosts(new CommunityId("ggg"), 20, ct);
            cts.Cancel();
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

        [Fact]
        public async Task GetComments_SendCancellationRequest_ThrowsCancellationException()
        {
            var client = await clientTask;
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken ct = cts.Token;

            Task<Cursor<Comment?>?> act() => client.GetComments(new PublicPostId(), ct);
            cts.Cancel();
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

        [Fact]
        public async Task GetCommentsWithPagination_SendCancellationRequest_ThrowsCancellationException()
        {
            var client = await clientTask;
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken ct = cts.Token;

            Task<Cursor<Comment?>?> act() => client.GetComments(new PublicPostId("hgfhgfh"), new CursorIndex("ghfghfg"), ct);
            cts.Cancel();
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

        [Fact]
        public async Task CreateCommentOnPost_SendCancellationRequest_ThrowsCancellationException()
        {
            var client = await clientTask;
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken ct = cts.Token;

            Task<Comment?> act() => client.Create(new PublicPostId(), new Comment(), ct);
            cts.Cancel();
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

        [Fact]
        public async Task CreateReplyComment_SendCancellationRequest_ThrowsCancellationException()
        {
            var client = await clientTask;
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken ct = cts.Token;

            Task<Comment?> act() => client.Create(new PublicPostId(), new CommentId(), new Comment(), ct);
            cts.Cancel();
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

        [Fact]
        public async Task GetCommunityById_SendCancellationRequest_ThrowsCancellationException()
        {
            var client = await clientTask;
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken ct = cts.Token;

            Task<Community?> act() => client.Get(new CommunityId(), ct);
            cts.Cancel();
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

        [Fact]
        public async Task GetCommunityByName_SendCancellationRequest_ThrowsCancellationException()
        {
            var client = await clientTask;
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken ct = cts.Token;

            Task<Community?> act() => client.GetCommunity("communityName", ct);
            cts.Cancel();
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

        [Fact]
        public async Task GetCommunities_SendCancellationRequest_ThrowsCancellationException()
        {
            var client = await clientTask;
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken ct = cts.Token;

            Task<IEnumerable<Community>?> act() => client.GetCommunities(ct);
            cts.Cancel();
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

        [Fact]
        public async Task GetCommunitiesWithParams_SendCancellationRequest_ThrowsCancellationException()
        {
            var client = await clientTask;
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken ct = cts.Token;

            Task<IEnumerable<Community>?> act() => client.GetCommunities(new QueryParams(), ct);
            cts.Cancel();
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

        [Fact]
        public async Task GetCommunitiesWithSearchQuery_SendCancellationRequest_ThrowsCancellationException()
        {
            var client = await clientTask;
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken ct = cts.Token;

            Task<IEnumerable<Community>?> act() => client.GetCommunities("searchQuery", ct);
            cts.Cancel();
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

        [Fact]
        public async Task GetCommunitiesWithSearchQueryAndParams_SendCancellationRequest_ThrowsCancellationException()
        {
            var client = await clientTask;
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken ct = cts.Token;

            Task<IEnumerable<Community>?> act() => client.GetCommunities("searchQuery", new QueryParams(), ct);
            cts.Cancel();
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

    }
}
