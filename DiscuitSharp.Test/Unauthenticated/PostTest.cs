using DiscuitSharp.Core;
using DiscuitSharp.Core.Group;
using DiscuitSharp.Core.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using DiscuitSharp.Core.Auth;
using DiscuitSharp.Core.Exceptions;
using DiscuitSharp.Core.Group.Serialization;
using Microsoft.Extensions.Options;
using System.Text.Json.Nodes;
using System.Text.Json;
using DiscuitSharp.Core.Utils;
using DiscuitSharp.Core.Utility;

namespace DiscuitSharp.Test.Unauthenticated
{
    public class PostTest : IClassFixture<NoAuthTestFixture>
    {
        private Task<IDiscuitClient> clientTask;

        public PostTest(NoAuthTestFixture fixture)
        {
            this.clientTask = fixture.InitializeClient();
        }

        [Fact]
        public async Task GetPost_RequestByPublicId_ReturnTextPost()
        {
            var client = await clientTask;
            var Id = new PublicPostId("9l5Om_AV");
            var post = await client.Get(Id);


            Assert.NotNull(post);
            Assert.IsType<TextPost>(post);

            if (post is TextPost lp)
            {
                Assert.NotNull(lp.Body);
                Assert.Equal("This is a test", lp.Body);
            }

            Assert.Equal("ba7b4dad8e80bad97bb7", post.Id.ToString());
            Assert.Equal(Post.Kind.Text, post.Type);
            Assert.Equal(new ("777e7240"), post.PublicId);
            Assert.Equal("Random Title 65", post.Title);
        }

        [Fact]
        public async Task GetPost_RequestByPublicId_ReturnLinkPost()
        {
            var client = await clientTask;
            var Id = new PublicPostId("04f99cf5");
            var post = await client.Get(Id);

            Assert.NotNull(post);
            Assert.IsType<LinkPost>(post);

            if (post is LinkPost lp)
            {
                Assert.NotNull(lp.Link);
                Assert.Equal("https://www.example.ca/blog/post", lp.Link.Url);
                Assert.Equal("www.example.ca", lp.Link.Hostname);
            }

            Assert.Equal("17c35c1fae47c7c7ebd18d08", post.Id.ToString());
            Assert.Equal(Post.Kind.Link, post.Type);
            Assert.Equal(new ("04f99cf5"), post.PublicId);
            Assert.Equal("Title for a Link Post", post.Title);
        }

        [Fact]
        public async Task GetPost_RequestByPublicId_ReturnImagePost()
        {
            var client = await clientTask;
            var Id = new PublicPostId("mTf2fgYj");
            Post? post = await client.Get(Id);

            Assert.NotNull(post);
            Assert.IsType<ImagePost>(post);

            if (post is ImagePost ip)
            {
                Assert.NotNull(ip.Image);
                Assert.Equal("17c36146d6ba09fd32c01816", ip.Image.Id);
                Assert.Equal("jpeg", ip.Image.Format);
                Assert.Equal("image/jpeg", ip.Image.Mimetype);
                Assert.Equal(546, ip.Image.Width);
                Assert.Equal(831, ip.Image.Height);
                Assert.Equal(64702, ip.Image.Size);
                Assert.Equal("rgb(84,83,86)", ip.Image.AverageColor);
                Assert.Equal("/images/17c36146d6ba09fd32c01816.jpeg?sig=MWMwJRDXGfPfirNMFw_3fQYNJmtRwSf2SwB9dj67cgE", ip.Image.Url);
            }

            Assert.Equal("17c3614a1bbb7509626b5e8c", post.Id.ToString());
            Assert.Equal(Post.Kind.Image, post.Type);
            Assert.Equal(new ("mTf2fgYj"), post.PublicId);
            Assert.Equal("NK Mods renewing their contracts", post.Title);
        }

        [Fact]
        public async Task GetPost_RequestByPublicId_PostContainsComments()
        {
            var client = await clientTask;
            var Id = new PublicPostId("j4ji0rd");
            var post = await client.Get(Id);
            
            Assert.NotNull(post);
            Assert.NotNull(post.Comments);
            Assert.Single(post.Comments);
            var comment = post.Comments.FirstOrDefault();
            if(comment is not null){
                Assert.Equal(new("17bda6ac79023e752bf42053"), comment.Id);
                Assert.Equal("1781b7443a2ffc18b5cbedf4", comment.UserId);
                Assert.Equal("Hardpawns", comment.Username);
                Assert.NotNull(comment.UserGroup);
                Assert.Equal(Core.Common.UserGroup.Normal.Description(), comment.UserGroup.Description());
                Assert.False(comment.UserDeleted);
                Assert.Null(comment.ParentId);
                Assert.Equal(0, comment.Depth);
                Assert.Equal(0, comment.NoReplies);
                Assert.Equal(0, comment.NoRepliesDirect);
                Assert.Null(comment.Ancestors);
                Assert.Equal("Comment Content Body Text", comment.Body);
                Assert.Equal(10, comment.Upvotes);
                Assert.Equal(0, comment.Downvotes);
                Assert.Equal("2024-03-17T20:18:13Z", comment.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                Assert.NotNull(comment.EditedAt);
                Assert.Equal("2024-03-17T20:18:25Z", comment.EditedAt.Value.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                Assert.False(comment.Deleted);
                Assert.Null(comment.DeletedAt);
                Assert.False(comment.UserVoted);
                Assert.Null(comment.UserVotedUp);
            }
        }

        [Fact]
        public async Task GetPost_RequestByPublicId_PostContainsPostDetails()
        {
            var client = await clientTask;
            var Id = new PublicPostId("j4ji0rd");

            var post = await client.Get(Id);

            Assert.NotNull(post);
            Assert.NotNull(post.Id);
            Assert.Equal(new("ba7b4dad8e80bad97bb7"), post.Id.Value);
            Assert.Equal(Post.Kind.Text, post.Type);
            Assert.Equal(new("777e7240"), post.PublicId);
            Assert.Equal("Noir", post.Title);
            Assert.NotNull(post.Author);
            Assert.Equal(new("87cceeba608643298da552d"), post.Author.Id);
            Assert.Equal("TestAuthor", post.Author.Username);
            Assert.False(post.IsPinned);
            Assert.False(post.IsPinnedSite);
            Assert.False(post.Locked);
            Assert.Null(post.LockedBy);
            Assert.Null(post.LockedAt);
            Assert.Equal(46, post.Upvotes);
            Assert.Equal(0, post.Downvotes);
            Assert.Equal(380179868967, post.Hotness);
            Assert.Equal("2024-03-17T20:04:24Z", post.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            Assert.Null(post.EditedAt);
            Assert.Equal("2024-03-17T20:18:13Z", post.LastActivityAt.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            Assert.False(post.Deleted);
            Assert.Null(post.DeletedAt);
            Assert.False(post.DeletedContent);
            Assert.Equal(1, post.NoComments);
        }
    
        [Fact]
        public async Task GetPosts_DefaultRequest_ReturnPosts()
        {
            var client = await clientTask;

            (IEnumerable<Post>? posts, string? next) = await client.GetPosts();
            Assert.NotNull(posts);
            Assert.NotNull(next);
            Assert.Equal("17692e122def73f25bd757e0", next);

            Assert.Collection(posts,
          post =>
          {
              Assert.Equal("a6a945f8-fac7-441d-a170-937870d03605", post.Id.ToString());
              Assert.Equal(Post.Kind.Text, post.Type);
              Assert.Equal(new ("P3T4GERf"), post.PublicId);
              Assert.Equal("Home Title 1", post.Title);
              Assert.Equal("2024-03-07T01:06:18Z", post.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"));
              Assert.Equal("2024-03-07T07:06:18Z", post.LastActivityAt.ToString("yyyy-MM-ddTHH:mm:ssZ"));
          },
          post =>
          {
              Assert.Equal("d612b7c1-16fd-4a7d-a7c1-665d2871af21", post.Id.ToString());
              Assert.Equal(Post.Kind.Text, post.Type);
              Assert.Equal(new ("9l5Om_AV"), post.PublicId);
              Assert.Equal("Home Title 2", post.Title);
              Assert.Equal("2023-09-29T08:06:18Z", post.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"));
              Assert.Equal("2023-09-29T12:06:18Z", post.LastActivityAt.ToString("yyyy-MM-ddTHH:mm:ssZ"));
          });
        }

        [Fact]
        public async Task GetPosts_HomeFeed_ReturnPosts()
        {
            var client = await clientTask;

            (var posts, string next) = await client.GetPosts(feed: Feed.Home);
            Assert.NotNull(posts);
            Assert.NotNull(next);
            Assert.Equal("17692e122def73f25bd757e0", next);

            Assert.Collection(posts,
          post =>
          {
              Assert.Equal("a6a945f8-fac7-441d-a170-937870d03605", post.Id.ToString());
              Assert.Equal(Post.Kind.Text, post.Type);
              Assert.Equal(new ("P3T4GERf"), post.PublicId);
              Assert.Equal("Home Title 1", post.Title);
              Assert.Equal("2024-03-07T01:06:18Z", post.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"));
              Assert.Equal("2024-03-07T07:06:18Z", post.LastActivityAt.ToString("yyyy-MM-ddTHH:mm:ssZ"));
          },
          post =>
          {
              Assert.Equal("d612b7c1-16fd-4a7d-a7c1-665d2871af21", post.Id.ToString());
              Assert.Equal(Post.Kind.Text, post.Type);
              Assert.Equal(new ("9l5Om_AV"), post.PublicId);
              Assert.Equal("Home Title 2", post.Title);
              Assert.Equal("2023-09-29T08:06:18Z", post.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"));
              Assert.Equal("2023-09-29T12:06:18Z", post.LastActivityAt.ToString("yyyy-MM-ddTHH:mm:ssZ"));
          });
        }

        [Fact]
        public async Task GetPosts_HomeFeedByHot_ReturnPosts()
        {
            var client = await clientTask;

            (var posts, string next) = await client.GetPosts(feed: Feed.Home, sort: Sort.Hot);
            Assert.NotNull(posts);
            Assert.NotNull(next);
            Assert.Equal("17692e122def73f25bd757e0", next);

            Assert.Collection(posts,
          post =>
          {
              Assert.Equal("a6a945f8-fac7-441d-a170-937870d03605", post.Id.ToString());
              Assert.Equal(Post.Kind.Text, post.Type);
              Assert.Equal(new ("P3T4GERf"), post.PublicId);
              Assert.Equal("Home Title 1", post.Title);
              Assert.Equal("2024-03-07T01:06:18Z", post.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"));
              Assert.Equal("2024-03-07T07:06:18Z", post.LastActivityAt.ToString("yyyy-MM-ddTHH:mm:ssZ"));
          },
          post =>
          {
              Assert.Equal("d612b7c1-16fd-4a7d-a7c1-665d2871af21", post.Id.ToString());
              Assert.Equal(Post.Kind.Text, post.Type);
              Assert.Equal(new ("9l5Om_AV"), post.PublicId);
              Assert.Equal("Home Title 2", post.Title);
              Assert.Equal("2023-09-29T08:06:18Z", post.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"));
              Assert.Equal("2023-09-29T12:06:18Z", post.LastActivityAt.ToString("yyyy-MM-ddTHH:mm:ssZ"));
          });
        }

        [Fact]
        public async Task GetPosts_FindByCommunityId_PostsFilteredForCommunity()
        {
            var client = await clientTask;
            var id = new CommunityId("17692e122def73f25bd757e0");
            (var posts, string? next) = await client.GetPosts(id);

            Assert.NotNull(posts);
            Assert.NotNull(next);
            Assert.Equal("hh34jdf500dfsdf2qiimj", next);
            Assert.All(posts, (post) =>
                        {
                            Assert.NotNull(post.Community);
                            Assert.Equal(new("17692e122def73f25bd757e0"), post.Community.Id);
                            Assert.Equal("general", post.Community.Name);
                        });
        }

        [Fact]
        public async Task GetPosts_FindByCommunityId_ReturnPosts()
        {
            var client = await clientTask;
            var id = new CommunityId("17692e122def73f25bd757e0");
            (var posts, string? next) = await client.GetPosts(id);
            
            Assert.NotNull(posts);
            Assert.NotNull(next);
            Assert.Equal("hh34jdf500dfsdf2qiimj", next);
            
            Assert.Collection(posts,
                (post ) =>
                {
                    Assert.IsType<ImagePost>(post);
                    Assert.Equal(new("17a2ae34de8303dea3c54a72"), post.Id);
                    Assert.Equal(Post.Kind.Image, post.Type);
                    if (post is ImagePost imgpost)
                    {
                        Assert.NotNull(imgpost.Image);
                        Assert.Equal("17a369be13ff8ac23bb8df3e", imgpost.Image.Id);
                        Assert.Equal("jpeg", imgpost.Image.Format);
                        Assert.Equal("/images/17a369be13ff8ac23bb8df3e.jpeg?sig=sIH6sk0WJDrsOkqcajQNoOk4ZwHZGWu0M_Aoy07e84I", imgpost.Image.Url);

                    }
                },
                post => 
                {
                    Assert.IsType<TextPost>(post);
                    Assert.Equal(new("17977138c89990dab7682cfd"), post.Id);
                    Assert.Equal(Post.Kind.Text, post.Type);
                    Assert.Equal(new ("AkxbHnol"), post.PublicId);
                    Assert.Equal("Title for a text Post", post.Title);
                    if (post is TextPost txt)
                    {
                        Assert.Equal("Test date", txt.Body);
                    }
                }, 
                post =>
                {
                    Assert.IsType<LinkPost>(post);
                    Assert.Equal(new("17c35c1fae47c7c7ebd18d08"), post.Id);
                    Assert.Equal(Post.Kind.Link, post.Type);
                    Assert.Equal(new("04f99cf5"), post.PublicId);
                    Assert.Equal("Title for a Link Post", post.Title);
                    if (post is LinkPost lnk)
                    {
                        Assert.NotNull(lnk.Link);
                        Assert.Equal("https://www.example.ca/blog/post", lnk.Link.Url);
                        Assert.Equal("www.example.ca", lnk.Link.Hostname);
                    }
                });
        }

        [Fact]
        public async Task GetPosts_LimitRequests_ReturnPosts()
        {
            var client = await clientTask;

            (var posts, string next) = await client.GetPosts(limit: 1);
            Assert.NotNull(posts);
            Assert.NotNull(next);
            Assert.Equal("17692e122def73f25bd757e0", next);

            Assert.Collection<Post>(posts, post =>
            {
                Assert.IsType<ImagePost>(post);

                if (post is ImagePost ip)
                {
                    Assert.NotNull(ip.Image);
                    Assert.Equal("17c36146d6ba09fd32c01816", ip.Image.Id);
                    Assert.Equal("jpeg", ip.Image.Format);
                    Assert.Equal("image/jpeg", ip.Image.Mimetype);
                    Assert.Equal(546, ip.Image.Width);
                    Assert.Equal(831, ip.Image.Height);
                    Assert.Equal(64702, ip.Image.Size);
                    Assert.Equal("rgb(84,83,86)", ip.Image.AverageColor);
                    Assert.Equal("/images/17c36146d6ba09fd32c01816.jpeg?sig=MWMwJRDXGfPfirNMFw_3fQYNJmtRwSf2SwB9dj67cgE", ip.Image.Url);
                }

                Assert.Equal("17c3614a1bbb7509626b5e8c", post.Id.ToString());
                Assert.Equal(Post.Kind.Image, post.Type);
                Assert.Equal("NK Mods renewing their contracts", post.Title);
            });
        }

        [Fact]
        public async Task GetPosts_CursorRequests_ReturnPosts()
        {
            var client = await clientTask;
            var cursor = new CursorIndex("17692e122def73f25bd757e0");
            (List<Post>? posts, string? next) = await client.GetPosts(cursor: cursor);
            Assert.NotNull(posts);
            Assert.NotNull(next);
            Assert.Equal("6146d6ba09fd32c01816a09fd32c", next);

            Assert.Collection<Post>(posts.Take(1), post =>
            {
                Assert.IsType<ImagePost>(post);

                if (post is ImagePost ip)
                {
                    Assert.NotNull(ip.Image);
                    Assert.Equal("17c36146d6ba09fd32c01816", ip.Image.Id);
                    Assert.Equal("jpeg", ip.Image.Format);
                    Assert.Equal("image/jpeg", ip.Image.Mimetype);
                    Assert.Equal(546, ip.Image.Width);
                    Assert.Equal(831, ip.Image.Height);
                    Assert.Equal(64702, ip.Image.Size);
                    Assert.Equal("rgb(84,83,86)", ip.Image.AverageColor);
                    Assert.Equal("/images/17c36146d6ba09fd32c01816.jpeg?sig=MWMwJRDXGfPfirNMFw_3fQYNJmtRwSf2SwB9dj67cgE", ip.Image.Url);
                }

                Assert.Equal("17c3614a1bbb7509626b5e8c", post.Id.ToString());
                Assert.Equal(Post.Kind.Image, post.Type);
                Assert.Equal("NK Mods renewing their contracts", post.Title);
            });
        }
        [Fact]
        public async Task GetPosts_CursorRequests_ReturnCursorOfPosts()
        {
            var client = await clientTask;
            var index = new CursorIndex("17692e122def73f25bd757e0");

            var cursor = await client.GetPosts(cursor: index);

            Assert.NotNull(cursor.Records);
            Assert.NotNull(cursor.Next);
            Assert.Equal("6146d6ba09fd32c01816a09fd32c", cursor.Next);
        }

        [Fact]
        public async Task CreatePost_ValidTextPost_UnauthenticatedException()
        {
            var client = await clientTask;
            Link li = new Link("http://example.ca", new Core.Media.Image());
            LinkPost newPost = new("First Post", "test", li);

            Task<LinkPost?> act() => client.Create(newPost);

            var exception = await Assert.ThrowsAsync<APIRequestException>(act);
            Assert.Equal(HttpStatusCode.Unauthorized, exception.StatusCode);
        }

        [Fact]
        public async Task DeletePost_NoIdExist_UnauthenticatedException()
        {
            var client = await clientTask;
            PublicPostId Id = new("000000");
            Task<Post?> act() => client.Delete(Id);

            var exception = await Assert.ThrowsAsync<APIRequestException>(act);
            Assert.Equal(HttpStatusCode.Unauthorized, exception.StatusCode);
        }
        [Fact]
        public async Task DeletePost_NoIdExistWithDeleteContent_UnauthenticatedException()
        {
            var client = await clientTask;
            PublicPostId Id = new("000000");
            Task<Post?> act() => client.Delete(Id, true);

            var exception = await Assert.ThrowsAsync<APIRequestException>(act);
            Assert.Equal(HttpStatusCode.Unauthorized, exception.StatusCode);
        }
        [Fact]
        public async Task DeletePost_NoIdExistWithoutDeleteContent_UnauthenticatedException()
        {
            var client = await clientTask;
            PublicPostId Id = new("000000");
            Task<Post?> act() => client.Delete(Id, false);

            var exception = await Assert.ThrowsAsync<APIRequestException>(act);
            Assert.Equal(HttpStatusCode.Unauthorized, exception.StatusCode);
        }
        [Fact]
        public async Task DeletePost_ValidId_UnauthenticatedException()
        {
            var client = await clientTask;
            PublicPostId Id = new("fL0ounHq");

            Task<Post?> act() => client.Delete(Id);

            var exception = await Assert.ThrowsAsync<APIRequestException>(act);
            Assert.Equal(HttpStatusCode.Unauthorized, exception.StatusCode);
        }
    }
}

