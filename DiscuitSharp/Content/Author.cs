using DiscuitSharp.Core.Media;
using DiscuitSharp.Core.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitSharp.Core.Content
{
    public class Author
    {
        public string? Id { get; set; }
        public string? Username { get; set; }
        public string Email { get; set; }
        public DateTime? EmailConfirmedAt { get; set; }
        public string AboutMe { get; set; }
        public int Points { get; set; }
        public bool IsAdmin { get; set; }
        public Image? ProPic { get; set; }
        public List<Badge>? Badges { get; set; }
        public int NoPosts { get; set; }
        public int NoComments { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool UpvoteNotificationsOff { get; set; }
        public bool ReplyNotificationsOff { get; set; }
        public string? HomeFeed { get; set; }
        public bool RememberFeedSort { get; set; }
        public bool EmbedsOff { get; set; }
        public bool HideUserProfilePictures { get; set; }
        public DateTime? BannedAt { get; set; }
        public bool IsBanned { get; set; }
        public int NotificationsNewCount { get; set; }
        public object? ModdingList { get; set; } // Could be a list or another class,

    }
}
