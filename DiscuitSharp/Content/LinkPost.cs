using DiscuitSharp.Core.Group.Serialization;
using DiscuitSharp.Core.Media;
using DiscuitSharp.Core.Member;
using DiscuitSharp.Core.Utility;
using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace DiscuitSharp.Core.Content
{
    public class LinkPost : Post, IMutableState<Link>
    {
        private Link? link;
        public Link? Link
        {
            get { return this.link; }
            set
            {
                this.link = value;
                if (value != null)
                    this["link"] = value;
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

    #region IMutableState

        protected Link this[string key]
        {
            get
            {
                return
                     (mutatedState.TryGetValue(key, out Link? value))
                    ? value
                    : throw new KeyNotFoundException();
            }
            set
            {
                if (mutatedState.ContainsKey(key))
                    mutatedState[key] = value;
            }
        }

        private Dictionary<string, Link> mutatedState = new();
        public ReadOnlyDictionary<string, Link> MutatedState
        {
            get
            {
                return new(this.mutatedState);
            }
        }
    #endregion
    }
}
