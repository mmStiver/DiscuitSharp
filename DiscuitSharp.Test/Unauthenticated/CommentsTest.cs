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
using DiscuitSharp.Core.Common;

namespace DiscuitSharp.Test.Unauthenticated
{
    public class CommentsTest : IClassFixture<NoAuthTestFixture>
    {
        readonly Task<IDiscuitClient> discClentTask;
        public CommentsTest(NoAuthTestFixture fixture)
        {
            discClentTask = fixture.InitializeClient();
        }

        [Fact]
        public async Task GetComments_FindCommentsByPostId_ReturnCursorOfComments()
        {
            var client = await discClentTask;
            PublicPostId PostId = new("G7YMijpa");

            var cursor = await client.GetComments(PostId);
            Assert.NotNull(cursor);
            Assert.NotNull(cursor.Records);
            Assert.NotNull(cursor.Next);
            Assert.Single(cursor.Records);
        }

        [Fact]
        public async Task GetComments_FindCommentsByPostId_ReturnPagedComments()
        {
            var client = await discClentTask;
            PublicPostId PostId = new("G7YMijpa");

            var cursor = await client.GetComments(PostId);
            Assert.NotNull(cursor?.Records);
            Assert.Single(cursor.Records);
        }

        [Fact]
        public async Task GetComment_FindCommentsByPostId_ReturnPagedComments()
        {
            var client = await discClentTask;
            PublicPostId PostId = new("G7YMijpa");

            Cursor<Comment?>? cursor = await client.GetComments(PostId);
            Assert.NotNull(cursor);
            (var comments, var _) = cursor;
            Assert.NotNull(comments);
            var comment = comments.FirstOrDefault();

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
            Assert.Equal(new string[] { "17c93f68a36ece261f6ce336", "17c93f6dd54807d149629e5c" }, comment.Ancestors);
            Assert.Equal("Third!", comment.Body);
            Assert.Equal(1, comment.Upvotes);
            Assert.Equal(0, comment.Downvotes);
            Assert.Equal("2024-04-24T15:02:41Z", comment.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            Assert.Null(comment.EditedAt);
            Assert.False(comment.Deleted);
            Assert.Null(comment.DeletedAt);

            Assert.True(comment.UserVoted);
            Assert.True(comment.UserVotedUp);
        }

        [Fact]
        public async Task GetComments_FindCommentsByPostId_ReturnPageCursor()
        {
            var client = await discClentTask;
            PublicPostId PostId = new("G7YMijpa");

            Cursor<Comment?>? cursor = await client.GetComments(PostId);
            Assert.NotNull(cursor);
            (var comments, var _) = cursor;
            Assert.NotNull(cursor.Next);
            Assert.Equal("2aa28ac7329270fff34f04a", cursor.Next);
        }

        [Fact]
        public async Task GetComment_FindCommentsByPageCursor_ReturnSecondPageComments()
        {
            var client = await discClentTask;
            PublicPostId PostId = new("G7YMijpa");
            CursorIndex? index = new("2aa28ac7329270fff34f04a");

            var cursor = await client.GetComments(PostId, index);
            Assert.NotNull(cursor);
            Assert.NotNull(cursor.Records);
            var comments = cursor.Records;  
            Assert.Null(cursor.Next);
            var comment = comments.FirstOrDefault();
            Assert.Collection(comments,
                (comment) => {
                    Assert.Equal(new("0a6dc6d94e4c5ee2e024f33a"), comment!.Id);
                    Assert.Equal("nextpage Comment 1", comment.Body);
                },
                (comment) => {
                    Assert.Equal(new("h9d9v0hgf445ff62dd2445"), comment!.Id);
                    Assert.Equal("nextpage Comment 2", comment.Body);
                }
                );
            Assert.NotNull(comment);
        }

        [Fact]
        public async Task GetComment_GetDeletedCommentsByPostId_ReturnDeletedComment()
        {
            var client = await discClentTask;
            PublicPostId PostId = new("MoiuhIkK");
            
            var cursor = await client.GetComments(PostId);
            Assert.NotNull(cursor);
            var comments = cursor.Records;
            Assert.NotNull(comments);
            Assert.Collection(comments,
                (comment) => {
                    Assert.Equal(new("17ce2ef6d1b02d8b15c2722c"), comment!.Id);
                    Assert.Equal(new("[Hidden]"), comment.Username);
                    Assert.Equal(new("000000000000000000000000"), comment.UserId);
                    Assert.Equal("[Deleted comment]", comment.Body);
                    Assert.True(comment.Deleted);
                    Assert.True(comment.ContentStripped);
                    Assert.Equal(UserGroup.Normal, comment.DeletedAs);
                    Assert.NotNull(comment.DeletedAt);
                    Assert.Equal("2024-05-10T17:20:16Z", comment.DeletedAt.Value.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                }
                );
        }
    }
}