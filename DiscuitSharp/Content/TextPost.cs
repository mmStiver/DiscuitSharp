using DiscuitSharp.Core.Media;
using DiscuitSharp.Core.Member;
using DiscuitSharp.Core.Utility;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace DiscuitSharp.Core.Content
{
    public class TextPost : Post, IMutableState<string>
    {
        private string? body = string.Empty;
        public string? Body
        {
            get { return this.body; }
            set
            {
                this.body = value;
                if (value != null)
                    this["body"] = value;
            }
        }

        [JsonConstructor]
        public TextPost() : base() { }
        public TextPost(string Title, string Community, string Body) 
                : base(Title, Community)
        {
            this.Type = Post.Kind.Text;
            this.Body = Body;
        }

        #region IMutableState

        public new string? Title
        {
            get { return base.Title; }
            set
            {
                this.Title = value;
                if (value != null)
                    this["body"] = value;
            }
        }

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
