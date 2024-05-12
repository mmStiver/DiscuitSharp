using DiscuitSharp.Core.Content;
using DiscuitSharp.Core.Group.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DiscuitSharp.Core.Auth
{
    public class DiscuitUser
    {
        [JsonConverter(typeof(PostIdJsonConverter))]
        public PostId? Id { get; init; }
        public string? Username { get; init; }
        public string? Email { get; init; }
        public DateTime? EmailConfirmedAt { get; init; }
        public string? AboutMe { get; init; }
        public int Points { get; init; }
        public bool IsAdmin { get; init; }
        public object? ProPic { get; init; }
        // public List<object> Badges { get; init; } // Replace object with the actual type of badges if known
        public int NoPosts { get; init; }
        public int NoComments { get; init; }
        public DateTime CreatedAt { get; init; }
        public bool Deleted { get; init; }
        public DateTime? DeletedAt { get; init; }
        public bool UpvoteNotificationsOff { get; init; }
        public bool ReplyNotificationsOff { get; init; }
        public string? HomeFeed { get; init; }
        public bool RememberFeedSort { get; init; }
        public bool EmbedsOff { get; init; }
        public bool HideUserProfilePictures { get; init; }
        public DateTime? BannedAt { get; init; }
        public bool IsBanned { get; init; }
        public int NotificationsNewCount { get; init; }
        public object? ModdingList { get; init; }

        [JsonConstructor]
        public DiscuitUser()
        {

        }
    }

}