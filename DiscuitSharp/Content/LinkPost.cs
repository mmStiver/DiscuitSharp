using DiscuitSharp.Core.Group.Serialization;
using DiscuitSharp.Core.Media;
using DiscuitSharp.Core.Member;
using DiscuitSharp.Core.Utility;
using System;
using System.Text.Json.Serialization;

namespace DiscuitSharp.Core.Content
{
    public class LinkPost : Post
    {
        private Link? link;
        public Link? Link
        {
            get { return this.link; }
            set
            {
                this.link = value;
                base["link"] = value;
            }
        }

        [JsonConstructor]
        public LinkPost() : base() { }

        public LinkPost(string Title, string Community, Link Link) : base(Title, Community){
            this.Type = Post.Kind.Link;

            this.Title = Title;
            this.CommunityName = Community;
            this.Link = Link;
        }
    }

    
}
