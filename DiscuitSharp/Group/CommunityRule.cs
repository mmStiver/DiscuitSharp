using System.Text.Json.Serialization;
using DiscuitSharp.Core.Group.Serialization;

namespace DiscuitSharp.Core.Group
{
    public record struct CommunityRule(
        int Id,
        string Rule,
        string? Description,
        int ZIndex,
        string CreatedBy,
        DateTime CreatedAt
    )
    {
        [JsonConverter(typeof(CommunityIdJsonConverter))]
        public CommunityId? CommunityId { get; init; } = null;

    }
}
