using DiscuitSharp.Core.Group;
using DiscuitSharp.Core.Group.Serialization;
using DiscuitSharp.Core.Media;
using DiscuitSharp.Core.Utility;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace DiscuitSharp.Core.Content
{
    public class ImagePost : Post, IMutableState<object>
    {
        
        private Image? image;
        [JsonConverter(typeof(ImageConverter))]
        public Image? Image
        {
            get { return this.image; }
            set
            {
                this.image = value;
                if(value != null && value.Id != null)
                    this["imageId"] = value.Id;
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

        #region IMutableState
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

        protected object this[string key]
        {
            get
            {
                return
                     (mutatedState.TryGetValue(key, out object? value))
                    ?value
                    : throw new KeyNotFoundException();
            }
            set
            {
                if (mutatedState.ContainsKey(key))
                   mutatedState[key] = value;
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
