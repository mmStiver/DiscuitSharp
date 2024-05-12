using DiscuitSharp.Core;
using DiscuitSharp.Core.Content;
using DiscuitSharp.Core.Group;
using System.Net;
using DiscuitSharp.Core.Utility;
using DiscuitSharp.Core.Media;
using DiscuitSharp.Core.Exceptions;
using DiscuitSharp.Core.Auth;
using System.Text.Json;
using static DiscuitSharp.Core.DiscuitClient;
using System.Xml.Linq;
using DiscuitSharp.Core.Common;

namespace DiscuitSharp.Test.Authenticated
{
    public class CommentsTest : IClassFixture<AuthTestFixture>
    {
        readonly Task<IDiscuitClient> discClientTask;
        public CommentsTest(AuthTestFixture fixture)
        {
            discClientTask = fixture.AuthenticatedClient();
        }

        [Fact] public async Task
        CreateComment_SubmitComment_ReturnNewlyCreatedPost()
        {
            var client = await discClientTask;
            PublicPostId postId = new("kvzM1JLq");
            Comment newComment = new("Newly Created Comment");

            var post = await client.Create(postId, newComment);
            Assert.NotNull(post);
            Assert.IsType<Comment>(post);
            if (post is Comment comment)
            {
                Assert.NotNull(comment);
                Assert.Equal(new("17c93f8221b8ded65ca6dcce"), comment.Id);
                Assert.Equal("176fb67a6a2d10402a2b", comment.UserId);
                Assert.Equal("TestUser", comment.Username);
                Assert.NotNull(comment.UserGroup);
                Assert.Equal(UserGroup.Normal.Description(), comment.UserGroup.Description());
                Assert.False(comment.UserDeleted);
                Assert.Equal(2, comment.Depth);
                Assert.Equal(0, comment.NoReplies);
                Assert.Equal(0, comment.NoRepliesDirect);
                Assert.Equal(new[] { "17c93f68a36ece261f6ce336" }, comment.Ancestors);
                Assert.Equal("Newly Created Comment", comment.Body);
                Assert.Equal(1, comment.Upvotes);
                Assert.Equal(0, comment.Downvotes);
                Assert.Equal("2024-04-24T15:02:41Z", comment.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                Assert.Null(comment.EditedAt);
                Assert.False(comment.Deleted);
                Assert.Null(comment.DeletedAt);
                Assert.NotNull(comment.Author);
                Assert.Equal(new("176fb67a6a2d10402a2b"), comment.Author.Id);
                Assert.Equal("TestUser", comment.Author.Username);
                Assert.True(comment.UserVoted);
                Assert.True(comment.UserVotedUp);
            }
        }

        [Fact]
        public async Task
       CreateComment_SubmitNestedComment_ReturnNewlyCreatedPost()
        {
            var client = await discClientTask;
            PublicPostId postId = new("kvzM1JLq");
            Comment topLevelComment = new("Top") {  Id = new("17c93f8221b8ded65ca6dcce") };
            Comment newComment = new(topLevelComment.Id.Value, "Created Nested Comment");

            var post = await client.Create(postId, topLevelComment.Id, newComment);
            Assert.NotNull(post);
            Assert.IsType<Comment>(post);
            if (post is Comment comment)
            {
                Assert.NotNull(comment);
                Assert.Equal(new("17c93f8221b8ded65ca6dcce"), comment.Id);
                Assert.Equal("176fb67a6a2d10402a2b", comment.UserId);
                Assert.Equal("TestUser", comment.Username);
                Assert.NotNull(comment.UserGroup);
                Assert.Equal(UserGroup.Normal.Description(), comment.UserGroup.Description());
                Assert.False(comment.UserDeleted);
                Assert.Equal(new("17c93f6dd54807d149629e5c"), comment.ParentId);
                Assert.Equal(2, comment.Depth);
                Assert.Equal(0, comment.NoReplies);
                Assert.Equal(0, comment.NoRepliesDirect);
                Assert.Equal(new[] { "17c93f68a36ece261f6ce336", "17c93f6dd54807d149629e5c" }, comment.Ancestors);
                Assert.Equal("Created Nested Comment", comment.Body);
                Assert.Equal(1, comment.Upvotes);
                Assert.Equal(0, comment.Downvotes);
                Assert.Equal("2024-04-24T15:02:41Z", comment.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                Assert.Null(comment.EditedAt);
                Assert.False(comment.Deleted);
                Assert.Null(comment.DeletedAt);
                Assert.NotNull(comment.Author);
                Assert.Equal(new("176fb67a6a2d10402a2b"), comment.Author.Id);
                Assert.Equal("TestUser", comment.Author.Username);
                Assert.True(comment.UserVoted);
                Assert.True(comment.UserVotedUp);
            }
        }


        [Fact]
        public async Task
        UpdateComment_UnalteredPost_ReturnOriginalPost()
        {
            var client = await discClientTask;
            PostId postId = new("17c81a0e3f77cfb3fcea5a11");
            PublicPostId postPublicId = new("kvzM1JLq");
            Comment comment = new()
            {
                Id = new("17c93f8221b8ded65ca6dcce"),
                Body = "Newly Created Comment",
                CreatedAt = DateTime.Parse("2024-04-24T15:02:41Z"),
                EditedAt = null,
                Deleted = false,
                DeletedAt = null,
                UserDeleted = false
            };

            var updated = await client.Update(postPublicId, comment);
            Assert.NotNull(updated);

            Assert.Equal(comment.Id, updated.Id);
            Assert.Equal(comment.Body, updated.Body);
            Assert.Equal("2024-04-24T15:02:41Z", updated.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            Assert.Equal(comment.EditedAt, updated.EditedAt);
        }

        [Fact] public async Task
        UpdateComment_ModifyCommentText_ReturnCommentWithNewText()
        {
            var client = await discClientTask;
            PublicPostId postPublicId = new("kvzM1JLq");
            Comment comment = new()
            {
                Id = new("17c81cf908cc7edb4f5de499"),
                Body = "Newly Created Comment",
                CreatedAt = DateTime.Parse("2024-04-20T22:18:34Z"),
                EditedAt = DateTime.Parse("2024-04-20T22:48:34Z"),
                Deleted = false,
                DeletedAt = null,
                UserDeleted = false
            };
            comment.Body = "Freshly Updated Body";
            var updated = await client.Update(postPublicId, comment);
            Assert.NotNull(updated);

            Assert.Equal(comment.Id, updated.Id);
            Assert.Equal("Freshly Updated Body", updated.Body);
            Assert.Equal("2024-04-24T15:02:41Z", updated.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            Assert.NotNull(updated.EditedAt);
            Assert.Equal("2024-04-20T22:48:34Z", updated.EditedAt.Value.ToString("yyyy-MM-ddTHH:mm:ssZ"));
        }

        [Fact]
        public async Task
        DeleteComment_UserDeleteCommentById_ReturnDeletedComment()
        {
            var client = await discClientTask;
            PublicPostId postId = new("kvzM1JLq");
            CommentId commentId = new("72547b41d89423318430fdeb");

            var deleted = await client.Delete(postId, commentId);

            Assert.NotNull(deleted);
            Assert.Equal(commentId, deleted.Id);
            Assert.Equal("Currently Deleted Comment", deleted.Body);
            Assert.Null(deleted.EditedAt);
            Assert.NotNull(deleted.DeletedAt);
            Assert.Equal("2024-04-21T00:10:52Z", deleted.DeletedAt.Value.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            Assert.True(deleted.UserDeleted);
            Assert.True(deleted.Deleted);
        }
    }
}