using DiscuitSharp.Core.Auth;
using DiscuitSharp.Core.Common;
using DiscuitSharp.Core.Group.Serialization;
using DiscuitSharp.Core.Utility;
using DiscuitSharp.Core.Utility.Serialization;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using static DiscuitSharp.Core.Content.Post;

namespace DiscuitSharp.Core.Content
{
    public class Comment : IMutableState<string>
    {
        [JsonConverter(typeof(CommentIdConverter))]
        public CommentId? Id { get; init; }
        public string? UserId { get; init; }
        public string? Username { get; init; }
        [JsonConverter(typeof(NullableEnumConverter<UserGroup>))]
        public UserGroup? UserGroup { get; init; }
        [JsonConverter(typeof(CommentIdConverter))]
        public CommentId? ParentId { get; init; }
        public int? Depth { get; init; }
        public int? NoReplies { get; init; }
        public int? NoRepliesDirect { get; init; }
        public string[]? Ancestors { get; init; }
        private string? body;
        public string Body
        {
            get { return this?.body ?? String.Empty; }
            set
            {
                this.body = value;
                this["body"] = value;
            }
        }
        public int? Upvotes { get; init; }
        public int? Downvotes { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? EditedAt { get; init; }
        public bool Deleted { get; init; } = false;
        [JsonConverter(typeof(NullableEnumConverter<UserGroup>))]
        public UserGroup? DeletedAs { get; init; }
        public bool UserDeleted { get; init; } = false;
        public DateTime? DeletedAt { get; init; }
        [JsonConverter(typeof(DiscuitUserJsonConverter))]
        public DiscuitUser? Author { get; init; }
        public bool? UserVoted { get; init; }
        public bool? UserVotedUp { get; init; }

        private Dictionary<string, string> mutatedState = new();
        public ReadOnlyDictionary<string, string> MutatedState { get => new(mutatedState); }
        public bool ContentStripped { get; init; }

        private string this[string key]
        {
            get
            {
                return
                     (mutatedState.TryGetValue(key, out string? value))
                    ? value
                    : throw new KeyNotFoundException();
            }
            set
            {
                if (mutatedState.ContainsKey(key))
                    mutatedState[key] = value;
            }
        }

        [JsonConstructor]
        public Comment()
        {
        }
        public Comment(string Body)
        {
            this.Body = Body;
        }
        public Comment(CommentId parentId, string Body)
        {
            this.ParentId = parentId;
            this.Body = Body;
        }

    }
}