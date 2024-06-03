using DiscuitSharp.Core.Group;
using DiscuitSharp.Core.Group.Serialization;
using DiscuitSharp.Core.Media;
using DiscuitSharp.Core.Utility;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace DiscuitSharp.Core.Content
{
    public class ImagePost : Post, IMutableState<string>
    {
        
        private Image? image;
        [JsonConverter(typeof(ImageConverter))]
        public Image? Image
        {
            get { return this.image; }
            set
            {
                if (this.image != null && value is Image img)
                    if(img.Id is string id)
                        this["ImageId"] = id;
                this.image = value;
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
                if (base.Title != null && value != null)
                    this["Title"] = value;
                base.Title = value;
            }
        }

        protected string this[string key]
        {
            get
            {
                return
                     (mutatedState.TryGetValue(key, out string? value))
                    ?value
                    : throw new KeyNotFoundException();
            }
            set
            {
                if (mutatedState.ContainsKey(key))
                   mutatedState[key] = value;
                else mutatedState.Add(key, value);

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
