using DiscuitSharp.Core.Auth;
using DiscuitSharp.Core.Group;
using DiscuitSharp.Core.Group.Serialization;
using DiscuitSharp.Core.Media;
using DiscuitSharp.Core.Member;
using DiscuitSharp.Core.Utility;
using DiscuitSharp.Core.Utility.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DiscuitSharp.Core.Content
{
    public abstract class Post
    {
        [JsonConverter(typeof(PostIdJsonConverter))]
        public PostId? Id { get; init; }
        [JsonConverter(typeof(EnumConverter<Kind>))]
        public Kind Type { get; init; }
        [JsonConverter(typeof(PublicPostIdConverter))]
        public PublicPostId? PublicId { get; init; }
        public bool IsPinned { get; init; }
        public bool IsPinnedSite { get; init; }
        public Community? Community { get; init; }

        public string? CommunityName { get; init; }
        public string? Title {  get; set; }
        public bool Locked { get; init; }
        public string? LockedBy { get; init; }
        public DateTime? LockedAt { get; init; }
        public int Upvotes { get; init; }
        public int Downvotes { get; init; }
        public long Hotness { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? EditedAt { get; init; }
        public DateTime LastActivityAt { get; init; }
        public bool Deleted { get; init; }
        public DateTime? DeletedAt { get; init; }
        public string? DeletedAs { get; init; }
        public bool DeletedContent { get; init; }
        public int NoComments { get; init; }
        public Comment[]? Comments { get; init; }
        public object? CommentsNext { get; init; } 
        public bool? UserVoted { get; init; }
        public bool? UserVotedUp { get; init; } 
        public bool IsAuthorMuted { get; init; }
        public bool IsCommunityMuted { get; init; }
        public DiscuitUser? Author { get; init; }

        

        public enum Kind
        {
            [Description("text")]
            Text  = 1,
            [Description("image")]
            Image = 2,
            [Description("link")]
            Link  = 3
        }

        [JsonConstructor]
        public Post() { }

        public Post(string Title, string Community)
        {
            this.Title = Title;
            this.CommunityName = Community;
        }



    }

    
}
