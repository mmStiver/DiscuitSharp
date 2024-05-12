using DiscuitSharp.Core.Media;
using DiscuitSharp.Core.Member;
using DiscuitSharp.Core.Utility;
using System.Text.Json.Serialization;

namespace DiscuitSharp.Core.Content
{
    public class TextPost : Post
    {
        private string? body = string.Empty;
        public string? Body
        {
            get { return this.body; }
            set
            {
                this.body = value;
                base["body"] = value;
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
    }

    
}
