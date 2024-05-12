using System.Text.Json;

namespace DiscuitSharp.Core.Utils
{
    public class LowercaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return name.ToLowerInvariant();
        }
    }

}
