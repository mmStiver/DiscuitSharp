using DiscuitSharp.Core.Content;
using DiscuitSharp.Core.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitSharp.Test.Domain
{
    public class PostMutationTestTest
    {


        [Fact]
        public void New_CreateTextPost_NoStateWasModified()
        { 
                var post = new TextPost()
                {
                    Id = new("123"),
                    PublicId = new("asdf"),
                    Type = Post.Kind.Text
                };

                post.Title = "Set me";
                post.Body= "Set me";

                Assert.Empty(post.MutatedState);
        }
        [Fact]
        public void Set_UpdateTextPost_StateHasModifiedValues()
        {
            var post = new TextPost()
            {
                Id = new("123"),
                PublicId = new("asdf"),
                Type = Post.Kind.Text,
                Title = "Set me",
                Body = "Create me"
            };

            post.Title = "Set me title";
            post.Body  = "Set me body";

            var dict = post.MutatedState;
            Assert.Equal("Set me title", dict["Title"]);
            Assert.Equal("Set me body", dict["Body"]);
        }

        [Fact]
        public void New_CreateLinkPost_NoStateWasModified()
        {
            var post = new LinkPost()
            {
                Id = new("123"),
                PublicId = new("asdf"),
                Type = Post.Kind.Text
            };

            post.Title = "Set me";
            post.Link = new Link("https://example.ca");

            Assert.Empty(post.MutatedState);
        }
        [Fact]
        public void Set_UpdateLinkPost_StateHasModifiedValues()
        {
            var post = new LinkPost()
            {
                Id = new("123"),
                PublicId = new("asdf"),
                Type = Post.Kind.Text,
                Title = "Set me",
                Link = new Link("https://example.ca")
            };

            post.Title = "Set me title";
            post.Link = new Link("https://example.com");

            var dict = post.MutatedState;
            Assert.Equal("Set me title", dict["Title"]);
            object dictVal = dict["Url"];
            Assert.Equal("https://example.com", dictVal);
        }

        [Fact]
        public void New_CreateImagePost_NoStateWasModified()
        {
            var post = new ImagePost()
            {
                Id = new("123"),
                PublicId = new("asdf"),
                Type = Post.Kind.Text,
            };

            post.Title = "Set me";
            post.Image = new() { Id = new("11232323") };

            Assert.Empty(post.MutatedState);
        }
        [Fact]
        public void Set_UpdateImagePost_StateHasModifiedValues()
        {
            var post = new ImagePost()
            {
                Id = new("123"),
                PublicId = new("asdf"),
                Type = Post.Kind.Text,
                Title = "Set me",
                Image = new()
            };

            post.Title = "Set me title";
            post.Image = new() {  Id = new("11232323")};

            var dict = post.MutatedState;
            Assert.Equal("Set me title", dict["Title"]);
            object dictVal = dict["ImageId"];
            Assert.Equal("11232323", dictVal);
        }
    }
}
