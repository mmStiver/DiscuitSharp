using DiscuitSharp.Core.Media;

namespace DiscuitSharp.Core.Content
{
    public class Link
    {
        public string? Hostname { get; set; }
        public string? Url { get; set; }
        public Image? Image { get; set; }

        public Link(string Url, Image? Image = null)
        {
            this.Url = Url;
            this.Image = Image ?? null;
            this.Hostname = new Uri(Url).Host;
        }
        
    }
}