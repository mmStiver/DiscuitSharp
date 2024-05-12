
namespace DiscuitSharp.Core.Media
{
    public record ImageCopy
    {
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int BoxWidth { get; set; }
        public int BoxHeight { get; set; }
        public string? ObjectFit { get; set; }
        public string? Format { get; set; }
        public string? Url { get; set; }
    }
}
