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

namespace DiscuitSharp.Test.Authenticated
{
    public class CancellationTest : IClassFixture<AuthTestFixture>
    {
        readonly Task<IDiscuitClient> clientTask;
        readonly CancellationTokenSource cts = new CancellationTokenSource();
        public CancellationTest(AuthTestFixture fixture)
        {
            clientTask = fixture.InitializeClient();
        }

        [Fact]
        public async Task InvalidateAuth_SendCancellationRequest_ThrowCancellationException()
        {
            var client = await clientTask;
            CancellationToken ct = cts.Token;

            Task<bool?> act() => client.InvalidateAuth(ct);
            cts.Cancel();
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

        [Fact]
        public async Task GetAuthenticatedUser_SendCancellationRequest_ThrowCancellationException()
        {
            var client = await clientTask;
            CancellationToken ct = cts.Token;

            Task<DiscuitUser?> act() => client.GetAuthenticatedUser(ct);
            cts.Cancel();
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

        [Fact]
        public async Task CreateTextPost_SendCancellationRequest_ThrowsCancellationException()
        {
            var client = await clientTask;
            CancellationToken ct = cts.Token;

            Task<TextPost?> act() => client.Create(new TextPost() {  Title = "Tit" , Body = "bdy"}, ct);
            cts.Cancel();
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

        [Fact]
        public async Task CreateLinkPost_SendCancellationRequest_ThrowsCancellationException()
        {
            var client = await clientTask;
            CancellationToken ct = cts.Token;
            var lnk = new LinkPost()
            {
                Id = new("")
                  ,
                Link = new Link("http://www.example.ca")
                ,
                Type = Post.Kind.Image
                ,
                Title = "title"
                ,
                CommunityName = "communityName"
            };

            Task<LinkPost?> act() => client.Create(lnk, ct);
            cts.Cancel();
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

        [Fact]
        public async Task CreateImagePost_SendCancellationRequest_ThrowsCancellationException()
        {
            var client = await clientTask;
            CancellationToken ct = cts.Token;
            var img = new ImagePost() 
            { 
                  Id = new("")
                  , Image = new Core.Media.Image() {  Id = "000" }
                , Type = Post.Kind.Image
                , Title = "title"
                , CommunityName = "communityName"
            };

            Task<ImagePost?> act() => client.Create(img, ct);
            cts.Cancel();
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

        [Fact]
        public async Task DeletePost_SendCancellationRequest_ThrowsCancellationException()
        {
            var client = await clientTask;
            CancellationToken ct = cts.Token;

            Task<bool?> act() => client.Delete(new PublicPostId(), null, ct);
            cts.Cancel();
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

        [Fact]
        public async Task CreateCommentOnPost_SendCancellationRequest_ThrowsCancellationException()
        {
            var client = await clientTask;
            CancellationToken ct = cts.Token;

            Task<Comment?> act() => client.Create(new PublicPostId(), new Comment(), ct);
            cts.Cancel();
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

        [Fact]
        public async Task CreateReplyComment_SendCancellationRequest_ThrowsCancellationException()
        {
            var client = await clientTask;
            CancellationToken ct = cts.Token;

            Task<Comment?> act() => client.Create(new PublicPostId(), new CommentId(), new Comment(), ct);
            cts.Cancel();
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

        [Fact]
        public async Task DeleteComment_SendCancellationRequest_ThrowsCancellationException()
        {
            var client = await clientTask;
            CancellationToken ct = cts.Token;

            Task<Comment?> act() => client.Delete(new PublicPostId(), new CommentId(), ct);
            cts.Cancel();
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

        [Fact]
        public async Task UpdatePost_SendCancellationRequest_ThrowsCancellationException()
        {
            var client = await clientTask;
            CancellationToken ct = cts.Token;

            Task<Post?> act() => client.Update(new TextPost(), ct);
            cts.Cancel();
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

        [Fact]
        public async Task UpdatePostVote_SendCancellationRequest_ThrowsCancellationException()
        {
            var client = await clientTask;
            CancellationToken ct = cts.Token;

            Task<Post?> act() => client.Update(new PostVote(new(""), true), ct);
            cts.Cancel();
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

        [Fact]
        public async Task UpdateComment_SendCancellationRequest_ThrowsCancellationException()
        {
            var client = await clientTask;
            CancellationToken ct = cts.Token;
            var cmt = new Comment() { Body = ""};

            Task<Comment?> act() => client.Update(new PublicPostId("ffff"), cmt, ct);
            cts.Cancel();
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

        [Fact]
        public async Task UpdateCommentVote_SendCancellationRequest_ThrowsCancellationException()
        {
            var client = await clientTask;
            CancellationToken ct = cts.Token;

            Task<Comment?> act() => client.Update(new CommentVote(new(""), true), ct);
            cts.Cancel();
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

        [Fact]
        public async Task UpdatePostWithAction_SendCancellationRequest_ThrowsCancellationException()
        {
            var client = await clientTask;
            CancellationToken ct = cts.Token;

            PublicPostId Id = new PublicPostId("Rg8TkGaE");
            Task<Post?> act() => client.Update(Id, PostAction.Lock, Core.Common.UserGroup.Moderator, cts.Token);
            cts.Cancel();

            var exception = await Assert.ThrowsAsync<TaskCanceledException>(act);
        }

    }
}
