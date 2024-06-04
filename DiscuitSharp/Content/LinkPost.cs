using DiscuitSharp.Core.Group.Serialization;
using DiscuitSharp.Core.Media;
using DiscuitSharp.Core.Member;
using DiscuitSharp.Core.Utility;
using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace DiscuitSharp.Core.Content
{
    public class LinkPost : Post, IMutableState<string>
    {
        private Link? link;
        public Link? Link
        {
            get { return this.link; }
            set
            {
                if (this.link != null && value != null)
                    this["Url"] = value.Url;
                this.link = value;
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
        protected string this[string key]
        {
            get
            {
                return
                     (mutatedState.TryGetValue(key, out string? value))
                    ? value
                    : throw new KeyNotFoundException();
            }
            set
            {
                if (mutatedState.ContainsKey(key))
                    mutatedState[key] = value;
                else mutatedState.Add(key, value);

            }
        }
        public new string? Title
        {
            get { return base.Title; }
            set
            {
                if (base.Title != null && value != null)
                    this["Title"] = value;
                base.Title = value;
            }
        }

        private Dictionary<string, string> mutatedState = new();
        public ReadOnlyDictionary<string, string> MutatedState
        {
            get
            {
                return new(this.mutatedState);
            }
        }
    #endregion
    }
}
