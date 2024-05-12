using DiscuitSharp.Core;
using DiscuitSharp.Core.Content;
using DiscuitSharp.Core.Group;
using System.Net;
using DiscuitSharp.Core.Utility;
using DiscuitSharp.Core.Media;
using DiscuitSharp.Core.Exceptions;
using DiscuitSharp.Core.Auth;

namespace DiscuitSharp.Test.Authenticated
{
    public class VoteTest : IClassFixture<AuthTestFixture>
    {
        readonly Task<IDiscuitClient> discClentTask;
        public VoteTest(AuthTestFixture fixture)
        {
            discClentTask = fixture.AuthenticatedClient();
        }

        [Fact]
        public async Task
        Vote_SendVoteForNonexistantPost_NotFoundRequestException()
        {
            var client = await discClentTask;
            PostVote Vote = new(new PostId("000000000000"), null);


            Task<Post?> act() => client.Update(Vote);

            var exception = await Assert.ThrowsAsync<APIRequestException>(act);
            Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
            Assert.Equal("post/not-found", exception.ErrorDetails.Code);
            Assert.Equal("Post(s) not found.", exception.ErrorDetails.Message);
            Assert.Equal(404, exception.ErrorDetails.Status);
        }
        [Fact]
        public async Task
        Vote_UnvotePost_ReturnUnvotedPost()
        {
            var client = await discClentTask;
            PostVote Vote = new(new PostId("G7YMijpa"), null);


            Post? post = await client.Update(Vote);
            Assert.NotNull(post);
            Assert.Null(post.UserVoted);
            Assert.Null(post.UserVotedUp);
            Assert.Equal(50, post.Upvotes);
            Assert.Equal(0, post.Downvotes);
        }
        [Fact]
        public async Task
        Vote_UpvotePost_ReturnUpovedPost()
        {
            var client = await discClentTask;
            PostVote Vote = new(new PostId("G7YMijpa"), true);

            Post? post = await client.Update(Vote);
            Assert.NotNull(post);
            Assert.True(post.UserVoted);
            Assert.True(post.UserVotedUp);
            Assert.Equal(51, post.Upvotes);
            Assert.Equal(0, post.Downvotes);
        }

        [Fact]
        public async Task
        Vote_DownvotePost_ReturnDownvotedPost()
        {
            var client = await discClentTask;
            PostVote Vote = new(new PostId("G7YMijpa"), false);


            Post? post = await client.Update(Vote);
            Assert.NotNull(post);
            Assert.True(post.UserVoted);
            Assert.False(post.UserVotedUp);
            Assert.Equal(50, post.Upvotes);
            Assert.Equal(1, post.Downvotes);
        }


        [Fact]
        public async Task
        Vote_SendVoteForNonexistantComment_NotFoundRequestException()
        {
            var client = await discClentTask;
            CommentVote Vote = new(new CommentId("000000000000"), null);


            Task<Comment?> act() => client.Update(Vote);

            var exception = await Assert.ThrowsAsync<APIRequestException>(act);
            Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
            Assert.Equal("post/not-found", exception.ErrorDetails.Code);
            Assert.Equal("Post(s) not found.", exception.ErrorDetails.Message);
            Assert.Equal(404, exception.ErrorDetails.Status);
        }
        [Fact]
        public async Task
        Vote_UnvoteComment_ReturnUnvotedComment()
        {
            var client = await discClentTask;
            CommentVote Vote = new(new CommentId("17c68df98f91eeea40da0934"), null);

            Comment? Comment = await client.Update(Vote);
            Assert.NotNull(Comment);
            Assert.Null(Comment.UserVoted);
            Assert.Null(Comment.UserVotedUp);
            Assert.Equal(50, Comment.Upvotes);
            Assert.Equal(0, Comment.Downvotes);
        }
        [Fact]
        public async Task
        Vote_UpvoteComment_ReturnUpvotedComment()
        {
            var client = await discClentTask;
            CommentVote Vote = new(new CommentId("17c68df98f91eeea40da0934"), true);

            Comment? Comment = await client.Update(Vote);
            Assert.NotNull(Comment);
            Assert.True(Comment.UserVoted);
            Assert.True(Comment.UserVotedUp);
            Assert.Equal(51, Comment.Upvotes);
            Assert.Equal(0, Comment.Downvotes);
        }

        [Fact] public async Task
        Vote_DownvoteComment_ReturnDownvotedComment()
        {
            var client = await discClentTask;
            CommentVote Vote = new(new CommentId("17c68df98f91eeea40da0934"), false);

            Comment? Comment = await client.Update(Vote);
            Assert.NotNull(Comment);
            Assert.True(Comment.UserVoted);
            Assert.False(Comment.UserVotedUp);
            Assert.Equal(50, Comment.Upvotes);
            Assert.Equal(1, Comment.Downvotes);
        }

        [Fact] public async Task
        Vote_SendVoteForDeleted_ForbiddenException()
        {
            var client = await discClentTask;
            CommentVote Vote = new(new CommentId("4d4s5sd4f6s4dee6f54s88dff"), true);


            Task<Comment?> act() => client.Update(Vote);

            var exception = await Assert.ThrowsAsync<APIRequestException>(act);
            Assert.Equal(HttpStatusCode.Forbidden, exception.StatusCode);
            Assert.Equal("comment_deleted", exception.ErrorDetails.Code);
            Assert.Equal("Comment(s) deleted.", exception.ErrorDetails.Message);
            Assert.Equal(403, exception.ErrorDetails.Status);
        }
    }
}