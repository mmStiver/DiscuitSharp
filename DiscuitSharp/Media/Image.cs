
namespace DiscuitSharp.Core.Media
{
    public record Image()
    {
        public string? Id { get; set; }
        public string? Format { get; set; }
        public string? Mimetype { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Size { get; set; }
        public string? AverageColor { get; set; }
        public string? Url { get; set; }
        public List<ImageCopy>? Copies { get; set; }
    }

}
