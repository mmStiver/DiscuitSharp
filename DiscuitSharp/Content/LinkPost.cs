using DiscuitSharp.Core.Group.Serialization;
using DiscuitSharp.Core.Media;
using DiscuitSharp.Core.Member;
using DiscuitSharp.Core.Utility;
using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace DiscuitSharp.Core.Content
{
    public class LinkPost : Post, IMutableState<object>
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
        protected object this[string key]
        {
            get
            {
                return
                     (mutatedState.TryGetValue(key, out object? value))
                    ? value
                    : throw new KeyNotFoundException();
            }
            set
            {
                if (mutatedState.ContainsKey(key))
                    mutatedState[key] = value;
            }
        }
        public new string? Title
        {
            get { return base.Title; }
            set
            {
                this.Title = value;
                if (value != null)
                    this["Title"] = value;
            }
        }

        private Dictionary<string, object> mutatedState = new();
        public ReadOnlyDictionary<string, object> MutatedState
        {
            get
            {
                return new(this.mutatedState);
            }
        }
    #endregion
    }
}
