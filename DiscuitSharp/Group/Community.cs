
using System.Text.Json.Serialization;
using DiscuitSharp.Core.Auth;
using DiscuitSharp.Core.Group.Serialization;
using DiscuitSharp.Core.Media;


namespace DiscuitSharp.Core.Group
{
    public class Community {
        [JsonConstructor]
        public Community()
        { 
        }

        public Community(string Name)
        {
            this.Name = Name;
        }

        [JsonConverter(typeof(CommunityIdJsonConverter))]
        public CommunityId? Id { get; init; }
        [JsonConverter(typeof(UserIdJsonConverter))]
        public UserId? UserId { get; init; }
        public string? Name { get; init; }
        public bool Nsfw { get; init; }
        public string? About { get; init; }
        public int NoMembers { get; init; }
        public Image? ProPic { get; init; }
        public Image? BannerImage { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? DeletedAt { get; init; }
        public bool? UserJoined { get; init; }
        public bool? UserMod { get; init; }
        public bool? IsMuted { get; init; }
        public bool? IsDefault { get; init; }
        public DiscuitUser[]? Mods { get; init; } 
        public CommunityRule[]? Rules { get; init; } 
        public object? ReportsDetails { get; init; }
        public int? NoUsers { get; init; }
        public object? BannedFrom { get; init; }
        public string? VapidPublicKey { get; init; }
        public Mutes? Mutes { get; init; }
    }

   
    public record Mutes(
        IEnumerable<object> CommunityMutes, // Replace 'object' with the appropriate type if known
        IEnumerable<object> UserMutes // Replace 'object' with the appropriate type if known
    );

}
