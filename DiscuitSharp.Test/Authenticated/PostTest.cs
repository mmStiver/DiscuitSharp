﻿using DiscuitSharp.Core;
using DiscuitSharp.Core.Content;
using DiscuitSharp.Core.Group;
using System.Net;
using DiscuitSharp.Core.Utility;
using DiscuitSharp.Core.Media;
using DiscuitSharp.Core.Exceptions;
using DiscuitSharp.Core.Auth;

namespace DiscuitSharp.Test.Authenticated
{
    public class PostTest : IClassFixture<AuthTestFixture>
    {
        readonly Task<IDiscuitClient> discClentTask;
        public PostTest(AuthTestFixture fixture)
        {
            discClentTask = fixture.AuthenticatedClient();
        }

        [Fact] public async Task 
        GetPost_FindPostByPublicId_ReturnPost()
        {
            var client = await discClentTask;
            PublicPostId PostId = new("RakY4sqi");

            Post? Post = await client.Get(PostId);
            Assert.NotNull(Post);
            Assert.Equal(new("17c68bb8829688ba13844398"), Post.Id);
            Assert.Equal(new("RakY4sqi"), Post.PublicId);
        }

        [Fact] public async Task 
        GetPost_FindPostFetchCommunity_ReturnPostWithCommunity()
        {
            var client = await discClentTask;
            PublicPostId PostId = new("RakY4sqi");

            Post? Post = await client.Get(PostId);
            Assert.NotNull(Post);
            Assert.Equal(new("17c68bb8829688ba13844398"), Post.Id);
            Assert.Equal(new("RakY4sqi"), Post.PublicId);
            Assert.Null(Post.Community);
            if(Post.Community is Community comm)
            {
                Assert.Equal(new("177e4d1ddede4a7920e6a4e1"), comm.Id);
                Assert.Equal(new("17692e04a6576d682930a4f5"), comm.UserId);
                Assert.Equal(new("test"), comm.Name);
            }
        }

        [Fact] public async Task 
        GetPost_FindPostDontFetchCommunity_ReturnPostWithoutCommunity()
        {
            var client = await discClentTask;
            PublicPostId PostId = new("RakY4sqi");

            Post? Post = await client.Get(PostId);
            Assert.NotNull(Post);
            Assert.Equal(new("17c68bb8829688ba13844398"), Post.Id);
            Assert.Equal(new("RakY4sqi"), Post.PublicId);
            Assert.Null(Post.Community);
        }

        [Fact]
        public async Task
        GetPost_FindPostByPublicId_ReturnPostWithNoComments()
        {
            var client = await discClentTask;
            PublicPostId PostId = new("RakY4sqi");

            Post? Post = await client.Get(PostId);
            Assert.NotNull(Post);
            Assert.Null(Post.Comments);
        }

        [Fact]
        public async Task
         GetPost_FindPostByPublicId_ReturnPostWithSingleComment()
        {
            var client = await discClentTask;
            PublicPostId PostId = new("A3c39F9");

            Post? Post = await client.Get(PostId);
            Assert.NotNull(Post);
            Assert.NotNull(Post.Comments);
            if (Post.Comments.FirstOrDefault() is Comment detailedComment)
            {
                Assert.Equal(new("6d26b73a3d93e52479a2d952"), detailedComment.Id);
                Assert.Equal("176fb67a0c266a2d10402a2b", detailedComment.UserId);
                Assert.Equal("asyncrosaurus", detailedComment.Username);
                Assert.NotNull(detailedComment.UserGroup);
                Assert.Equal(Core.Common.UserGroup.Normal.Description(), detailedComment.UserGroup.Description());
                Assert.False(detailedComment.UserDeleted);
                Assert.Null(detailedComment.ParentId);
                Assert.Equal(0, detailedComment.Depth);
                Assert.Equal(0, detailedComment.NoReplies);
                Assert.Equal(0, detailedComment.NoRepliesDirect);
                Assert.Null(detailedComment.Ancestors);
                Assert.Equal("Test Comment 1!!", detailedComment.Body);
                Assert.Equal(1, detailedComment.Upvotes);
                Assert.Equal(0, detailedComment.Downvotes);
                Assert.Equal("2024-04-24T15:02:41Z", detailedComment.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                Assert.Null(detailedComment.EditedAt);
                Assert.False(detailedComment.Deleted);
                Assert.Null(detailedComment.DeletedAt);
            }
        }

        [Fact]
        public async Task
        GetPost_FindPostByPublicId_ReturnPostWithComments()
        {
            var client = await discClentTask;
            PublicPostId PostId = new("A3c39F9");

            Post? Post = await client.Get(PostId);
            Assert.NotNull(Post);
            Assert.NotNull(Post.Comments);
            if (Post.Comments is Comment[] comms)
            {
                Assert.Collection(comms, 
                (c) => Assert.Equal(new("6d26b73a3d93e52479a2d952"), c.Id),
                (c) => Assert.Equal(new("d1801e698d68cd3e2b74bbe8"), c.Id),
                (c) => Assert.Equal(new("ecf5cd18ec2ffc582e8ca614"), c.Id)
                );
            }
        }

        [Fact] public async Task 
        CreatePost_SubmitTextPost_ReturnNewlyCreatedPost()
        {
            var client = await discClentTask;

            TextPost newPost = new("First Post", "general", "Hi Mom!");

            var post = await client.Create(newPost);
            Assert.NotNull(post);
            Assert.IsType<TextPost>(post);
            if (post is TextPost txt)
            {
                Assert.Equal(Post.Kind.Text, txt.Type);
                Assert.Equal(newPost.Title, txt.Title);
                Assert.Equal(newPost.CommunityName, txt.CommunityName);
                Assert.Equal(newPost.Body, txt.Body);
            }
        }
        
        [Fact] public async Task 
        CreatePost_SubmitLinkPost_ReturnNewlyCreatedPost()
        {
            var client = await discClentTask;
            Link li = new Link("https://www.example.com/9iLxR1h2208", new Core.Media.Image());
            LinkPost newPost = new("First Link", "programming", li);

            var post = await client.Create(newPost);
            Assert.NotNull(post);
            Assert.IsType<LinkPost>(post);
            if (post is LinkPost lnk)
            {
                Assert.Equal(Post.Kind.Link, lnk.Type);
                Assert.Equal(newPost.Title, lnk.Title);
                Assert.Equal(newPost.CommunityName, lnk.CommunityName);
                Assert.Equal(newPost.Link?.Url, lnk.Link?.Url);
                Assert.Equal(newPost.Link?.Hostname, lnk.Link?.Hostname);
            }
        }
        
        [Fact] public async Task 
        CreatePost_SubmitInvalidImage_BadRequestException()
        {
            var client = await discClentTask;
            Image img = new() { Id = "notreal" };
            ImagePost newPost = new("First Post", "test", img);

            Task<ImagePost?> act() => client.Create(newPost);

            var exception = await Assert.ThrowsAsync<APIRequestException>(act);
            Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
        }

        [Fact]
        public async Task
        CreatePost_SubmitImagePost_ReturnNewlyCreatedPost()
        {
            var client = await discClentTask;
            Image newImage = new() { Id = "17c67e1be2b732bfd47" };
            ImagePost newPost = new("First Image", "DiscuitMeta", newImage);

            var post = await client.Create(newPost);
            Assert.NotNull(post);
            Assert.IsType<ImagePost>(post);
            if (post is ImagePost img)
            {
                Assert.Equal(Post.Kind.Image, img.Type);
                Assert.Equal(newPost.Title, img.Title);
                Assert.Equal(newPost.CommunityName, img.CommunityName);
                Assert.Equal(newPost?.Image?.Id, img?.Image?.Id);
            }
        }

        [Fact]
        public async Task
        CreatePost_SubmitEmptyImagePost_ArgumentNullException()
        {
            var client = await discClentTask;
            ImagePost newPost = new() { Title = "First Image", CommunityName = "DiscuitMeta" };

            Task<ImagePost?> act() => client.Create(newPost);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(act);
        }

        [Fact]
        public async Task
        CreatePost_SubmitImagePostWithoutTitle_ArgumentException()
        {
            var client = await discClentTask;
            Image newImage = new() { Id = "17c67e1be2b732bfd47" };
            ImagePost newPost = new() { CommunityName = "DiscuitMeta", Image = newImage };

            Task<ImagePost?> act() => client.Create(newPost);

            var exception = await Assert.ThrowsAsync<ArgumentException>(act);
        }

        [Fact]
        public async Task
        CreatePost_SubmitEmptyLinkPost_ArgumentNullException()
        {
            var client = await discClentTask;
            LinkPost newPost = new() { Title = "this title" };
            Task<LinkPost?> act() => client.Create(newPost);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(act);
        }

        [Fact]
        public async Task
        CreatePost_SubmitLinkPostWithoutTitle_ArgumentException()
        {
            var client = await discClentTask;
            Link li = new Link("https://www.example.com/9iLxR1h2208", new Core.Media.Image());
            LinkPost newPost = new() {  Link = li };

            Task<LinkPost?> act() => client.Create(newPost);

            var exception = await Assert.ThrowsAsync<ArgumentException>(act);
        }
        
        [Fact]
        public async Task
        CreatePost_SubmitTextPostWithoutTitle_ArgumentException()
        {
            var client = await discClentTask;
            TextPost newPost = new() { Body ="this here" };

            Task<TextPost?> act() => client.Create(newPost);

            var exception = await Assert.ThrowsAsync<ArgumentException>(act);
        }
        
        [Fact]
        public async Task
        UpdatePost_UnalteredPost_ReturnOriginalPost()
        {
            var client = await discClentTask;
            PublicPostId publicPostId = new("Rg8TkGaE");
            var post = await client.Get(publicPostId);
            Assert.NotNull(post);

            var updated = await client.Update(post);
            Assert.NotNull(updated);

            Assert.Equal(post.Id, updated.Id);
            Assert.Equal(post.Type, updated.Type);
            Assert.Equal(post.PublicId, updated.PublicId);
            Assert.Equal(post.Title, updated.Title);
            Assert.Equal(post.EditedAt, updated.EditedAt);
        }

        [Fact]
        public async Task
        UpdatePost_ModifyPostTitle_ReturnPostWithNewTitle()
        {
            var client = await discClentTask;
            TextPost txtPost = new()
            {
                Id = new("17c68bb8829688ba13844398"),
                Type = Post.Kind.Text,
                PublicId = new PublicPostId("Rg8TkGaE"),
                Title = "original title",
                Body = "Oh, Canada"
            };

            txtPost.Title = "updated title";

            var updated = await client.Update(txtPost);

            Assert.NotNull(updated);
            Assert.IsType<TextPost>(updated);
            Assert.Equal(txtPost.Id, updated.Id);
            Assert.Equal(txtPost.Type, updated.Type);
            Assert.Equal(txtPost.PublicId, updated.PublicId);
            Assert.Equal("updated title", updated.Title);
            Assert.NotNull(updated.EditedAt);
            Assert.Equal("2024-04-17T12:05:11Z", updated.EditedAt.Value.ToString("yyyy-MM-ddTHH:mm:ssZ"));

        }

        [Fact] public async Task
        UpdatePost_ModifyTextPostContent_ReturnPostWithNewText()
        {
            var client = await discClentTask;

            TextPost txtPost = new () { 
              Id = new( "17c68bb8829688ba13844398"),
              Type = Post.Kind.Text,
              PublicId = new PublicPostId("Rg8TkGaE"),
              Title = "original title",
              Body = "Oh, Canada"
            };
            
            txtPost.Body = "updated value";
            var updated = await client.Update(txtPost);

            if (updated is TextPost updatedPost)
            {
                Assert.NotNull(updatedPost);
                Assert.Equal(txtPost.Id, updated.Id);
                Assert.Equal(txtPost.Type, updated.Type);
                Assert.Equal(txtPost.PublicId, updated.PublicId);
                Assert.Equal(txtPost.Title, updated.Title);
                Assert.Equal("updated value", updatedPost.Body);
                Assert.NotNull(updated.EditedAt);
                Assert.Equal("2024-04-17T12:05:11Z", updated.EditedAt.Value.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            }
        }

        [Fact] public async Task
        UpdatePost_ModifyPostTextContent_UpdateTextPostContent()
        {
            var client = await discClentTask;
            TextPost txtPost = new()
            {
                Id = new("17c68bb8829688ba13844398"),
                Type = Post.Kind.Text,
                PublicId = new PublicPostId("Rg8TkGaE"),
                Title = "original title",
                Body = "Oh, Canada"
            };

            txtPost!.Title = "updated title";
            txtPost.Body = "updated value";
            var updated = await client.Update(txtPost);

            if (updated is TextPost updatedPost)
            {
                Assert.NotNull(updatedPost);
                Assert.Equal(Post.Kind.Text, updatedPost.Type);
                Assert.Equal("updated value", updatedPost.Body);
                Assert.Equal("updated title", updatedPost.Title);
            }        
        }

        [Fact] public async Task
        UpdatePost_UpdateImagePostContent_ReturnUpdateImagePost()
        {
            var client = await discClentTask;
            ImagePost imgPost = new()
            {
                Id = new("17c68636766fdade1af11c4d"),
                Type = Post.Kind.Image,
                PublicId = new PublicPostId("vclkj85d"),
                Title = "original title",
                Image = new() { Id = "hh0d9sdisd93838sdf", Url = "https://example.ca/image1.jpg" }
            };
            imgPost.Image = new() { Id = "j09K8xasdsfYlkj3jf2", Url = "https://example.net/image2.jpg" };
            imgPost.Title = "Updated Image";

            var updated = await client.Update(imgPost);

            Assert.NotNull(updated);
            Assert.IsType<ImagePost>(updated);
            if (updated is ImagePost img)
            {
                Assert.NotNull(img.Image);
                Assert.Equal("Updated Image", img.Title);
                Assert.Equal(imgPost.Image.Id, img.Image.Id);
                Assert.Equal(imgPost.Image.Url, img.Image.Url);
                Assert.NotNull(img.EditedAt);
                Assert.Equal("2024-04-17T12:05:11Z", img.EditedAt.Value.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            
            }
        }

        [Fact] public async Task
        UpdatePost_UpdateLinkPostContent_ReturnUpdateLinkPost()
        {
            var client = await discClentTask;
            LinkPost lnkPost = new()
            {
                Id = new("17c68636766fdade1af11c4d"),
                Type = Post.Kind.Image,
                PublicId = new PublicPostId("vclkj85d"),
                Title = "original title",
                Link = new("https://example.com/link")
            };

            lnkPost.Title = "Updated Link";
            lnkPost.Link = new("https://example.net/page");

            var updated = await client.Update(lnkPost);

            Assert.NotNull(updated);
            Assert.IsType<LinkPost>(lnkPost);
            if (updated is LinkPost lnk)
            {
                Assert.NotNull(lnk.Link);
                Assert.Equal("Updated Link", lnk.Title);
                Assert.Equal("https://example.net/page", lnk.Link.Url);
                Assert.Equal("example.net", lnk.Link.Hostname);
                Assert.NotNull(lnk.EditedAt);
                Assert.Equal("2024-04-15T19:45:32Z", lnk.EditedAt.Value.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            }
            
        }

        [Fact] public async Task
        DeletePost_DeletePostById_ReturnSuccess()
        {
            var client = await discClentTask;
            TextPost? txt = new TextPost("Title", "Test", "Data")
            {
                PublicId = new PublicPostId("fL0ounHq")
            };
           
            var res = await client.Delete(txt.PublicId.Value);
            
            Assert.NotNull(res);
            Assert.True(res);
        }
        [Fact]
        public async Task
        DeletePost_DeletePostAndContent_ReturnSuccess()
        {
            var client = await discClentTask;
            TextPost? txt = new TextPost("Title", "Test", "Data")
            {
                PublicId = new PublicPostId("d3viYYpm")
            };

            var res = await client.Delete(txt.PublicId.Value, true);

            Assert.NotNull(res);
            Assert.True(res);
        }

        [Fact] public async Task
        LockPost_SubmitPostToLockAsUser_InsufficientPrivledgesException()
        {
            var client = await discClentTask;
            PublicPostId Id = new PublicPostId("Rg8TkGaE");
            Task<Post?> act() => client.Update(Id, PostAction.Lock, Core.Common.UserGroup.Normal);

            var exception = await Assert.ThrowsAsync<InsufficientPrivledgesException>(act);

        }

        [Fact] public async Task
        LockPost_SubmitPostToLockAsModerator_ForbiddenRequestException()
        {
            var client = await discClentTask;
            PublicPostId Id = new PublicPostId("Rg8TkGaE");
            Task<Post?> act() => client.Update(Id, PostAction.Lock, Core.Common.UserGroup.Moderator);

            var exception = await Assert.ThrowsAsync<APIRequestException>(act);
            Assert.Equal(HttpStatusCode.Forbidden, exception.StatusCode);
            Assert.Equal("not_mod", exception.ErrorDetails.Code);
            Assert.Equal("You are not a moderator.", exception.ErrorDetails.Message);
            Assert.Equal(403, exception.ErrorDetails.Status);
        }
        //":""not_mod"",""message"":""You are not a moderator.  
        //not_admin"",""message"":""You are not a administrator.

        [Fact] public async Task
        LockPost_SubmitPostToLockAsAdmin_ForbiddenRequestException()
        {
            var client = await discClentTask;
            PublicPostId Id = new PublicPostId("Rg8TkGaE");
            Task<Post?> act() => client.Update(Id, PostAction.Lock, Core.Common.UserGroup.Administrator);

            var exception = await Assert.ThrowsAsync<APIRequestException>(act);
            Assert.Equal(HttpStatusCode.Forbidden, exception.StatusCode);
            Assert.Equal("not_admin", exception.ErrorDetails.Code);
            Assert.Equal("You are not a administrator.", exception.ErrorDetails.Message);
            Assert.Equal(403, exception.ErrorDetails.Status);
        }


        [Fact] public async Task
        SpeakerPost_ChangeUserSpeaker_InsufficientPrivledgesException()
        {
            var client = await discClentTask;
            PublicPostId Id = new PublicPostId("Rg8TkGaE");
            UserId userId = new UserId("");
            Task<Post?> act() => client.Update(Id, PostAction.ChangeSpeaker, Core.Common.UserGroup.Normal);

            var exception = await Assert.ThrowsAsync<InsufficientPrivledgesException>(act);
        }
        [Fact]
        public async Task
        SpeakerPost_ChangeUserSpeakerAsMod_ForbiddenRequestException()
        {
            var client = await discClentTask;
            PublicPostId Id = new PublicPostId("Rg8TkGaE");
            UserId userId = new UserId("");
            Task<Post?> act() => client.Update(Id, PostAction.ChangeSpeaker, Core.Common.UserGroup.Moderator);

            var exception = await Assert.ThrowsAsync<APIRequestException>(act);
            Assert.Equal(HttpStatusCode.Forbidden, exception.StatusCode);
            Assert.Equal("not_mod", exception.ErrorDetails.Code);
            Assert.Equal("You are not a moderator.", exception.ErrorDetails.Message);
            Assert.Equal(403, exception.ErrorDetails.Status);
        }
    
        [Fact]
        public async Task
        SpeakerPost_ChangeUserSpeakerAsAdmin_ForbiddenRequestException()
        {
            var client = await discClentTask;
            PublicPostId Id = new PublicPostId("Rg8TkGaE");
            UserId userId = new UserId("");
            Task<Post?> act() => client.Update(Id, PostAction.ChangeSpeaker, Core.Common.UserGroup.Administrator);

            var exception = await Assert.ThrowsAsync<APIRequestException>(act);
            Assert.Equal(HttpStatusCode.Forbidden, exception.StatusCode);
            Assert.Equal("not_admin", exception.ErrorDetails.Code);
            Assert.Equal("You are not a administrator.", exception.ErrorDetails.Message);
            Assert.Equal(403, exception.ErrorDetails.Status);
        }
    }
}