using DiscuitSharp.Core.Group;
using DiscuitSharp.Core.Group.Serialization;
using DiscuitSharp.Core.Media;
using DiscuitSharp.Core.Utility;
using System.Text.Json.Serialization;

namespace DiscuitSharp.Core.Content
{
    public class ImagePost : Post
    {
        private Image? image;
        [JsonConverter(typeof(ImageConverter))]
        public Image? Image
        {
            get { return this.image; }
            set
            {
                this.image = value;
                base["imageId"] = value?.Id;
            }
        }


        [JsonConstructor]
        public ImagePost() : base(){}

        public ImagePost(string Title, String CommunityName, Image Image)
            :base(Title, CommunityName)
        {
            this.Type = Post.Kind.Image;
            this.Title = Title;
            this.Community= new Community(CommunityName);
            this.Image = Image;
        }
    }
}
