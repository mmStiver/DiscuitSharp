using DiscuitSharp.Core.Auth;
using DiscuitSharp.Core.Common;
using DiscuitSharp.Core.Content;
using DiscuitSharp.Core.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitSharp.Core
{
    /// <summary>
    /// Interface defining methods for interacting with a Discuit web API to manage users, posts, comments, and communities.
    /// </summary>
    public interface IDiscuitClient
    {
        /// <summary>
        /// Gets the session token.
        /// </summary>
        string? CSRFtoken { get; }

        /// <summary>
        /// Retrieves initial setup data required by the client.
        /// </summary>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The initial setup data, or null if unavailable.</returns>
        Task<Initial?> GetInitial(CancellationToken Token = default);

        /// <summary>
        /// Authenticates a user with the provided credentials.
        /// </summary>
        /// <param name="creds">The user's credentials.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The authenticated user, or null if authentication fails.</returns>
        Task<DiscuitUser?> Authenticate(Credentials creds, CancellationToken Token = default);

        /// <summary>
        /// Invalidates the current authentication token.
        /// </summary>
        /// <returns>True if the authentication is successfully invalidated, otherwise false.</returns>
        Task<bool?> InvalidateAuth(CancellationToken Token = default);

        /// <summary>
        /// Retrieves the currently authenticated user's details.
        /// </summary>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The authenticated user, or null if no user is authenticated.</returns>
        Task<DiscuitUser?> GetAuthenticatedUser(CancellationToken Token = default);

        /// <summary>
        /// Retrieves a user by username.
        /// </summary>
        /// <param name="UserName">The username of the user to retrieve.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The user if found, otherwise null.</returns>
        Task<DiscuitUser?> GetUser(string UserName, CancellationToken Token = default);

        /// <summary>
        /// Retrieves a specific post by ID.
        /// </summary>
        /// <param name="postId">The public post ID.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The requested post, or null if not found.</returns>
        Task<Post?> Get(PublicPostId postId, CancellationToken Token = default);

        /// <summary>
        /// Retrieves a cursor-based paginated list of posts.
        /// </summary>
        /// <param name="cursor">The starting cursor index, optional.</param>
        /// <param name="feed">The specific feed to retrieve posts from, optional.</param>
        /// <param name="sort">The sorting criteria, optional.</param>
        /// <param name="limit">The maximum number of posts to retrieve, optional.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>A cursor-based list of posts.</returns>
        Task<Cursor<Post>> GetPosts(CursorIndex? cursor = null, Feed? feed = null, Sort? sort = null, int? limit = null, CancellationToken Token = default);

        /// <summary>
        /// Retrieves posts for a specific community by community ID.
        /// </summary>
        /// <param name="Id">The community ID.</param>
        /// <param name="limit">The maximum number of posts to retrieve, optional.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>A cursor-based list of posts for the specified community.</returns>
        Task<Cursor<Post>> GetPosts(CommunityId Id, CursorIndex? cursor = null, int? limit = null, CancellationToken Token = default);

        /// <summary>
        /// Retrieves comments for a specific post by post ID.
        /// </summary>
        /// <param name="Id">The public post ID to retrieve comments for.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>A cursor-based list of comments, or null if not available.</returns>
        Task<Cursor<Comment?>?> GetComments(PublicPostId Id, CancellationToken Token = default);

        /// <summary>
        /// Retrieves comments for a specific post by post ID with pagination.
        /// </summary>
        /// <param name="Id">The public post ID.</param>
        /// <param name="page">The cursor index for pagination, optional.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>A cursor-based list of comments, or null if not available.</returns>
        Task<Cursor<Comment?>?> GetComments(PublicPostId Id, CursorIndex? page, CancellationToken Token = default);

        /// <summary>
        /// Creates a new text post.
        /// </summary>
        /// <param name="post">The text post to create.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The created text post, or null if the operation fails.</returns>
        Task<TextPost?> Create(TextPost post, CancellationToken Token = default);

        /// <summary>
        /// Creates a new link post.
        /// </summary>
        /// <param name="post">The link post to create.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The created link post, or null if the operation fails.</returns>
        Task<LinkPost?> Create(LinkPost post, CancellationToken Token = default);

        /// <summary>
        /// Creates a new image post.
        /// </summary>
        /// <param name="post">The image post to create.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The created image post, or null if the operation fails.</returns>
        Task<ImagePost?> Create(ImagePost post, CancellationToken Token = default);

        /// <summary>
        /// Creates a new comment on a specific post.
        /// </summary>
        /// <param name="postId">The public post ID where the comment will be added.</param>
        /// <param name="post">The comment to add.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The created comment, or null if the operation fails.</returns>
        Task<Comment?> Create(PublicPostId postId, Comment post, CancellationToken Token = default);

        /// <summary>
        /// Creates a new comment as a reply to an existing comment on a specific post.
        /// </summary>
        /// <param name="postId">The public post ID where the comment will be added.</param>
        /// <param name="parentId">The parent comment ID to reply to, optional.</param>
        /// <param name="comment">The reply comment to add.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The created comment, or null if the operation fails.</returns>
        Task<Comment?> Create(PublicPostId postId, CommentId? parentId, Comment comment, CancellationToken Token = default);

        /// <summary>
        /// Deletes a specific post by ID.
        /// </summary>
        /// <param name="postId">The public post ID of the post to delete.</param>
        /// <param name="deleteContent">Whether to delete the content of the post as well, optional.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>True if the post is successfully deleted, otherwise false.</returns>
        Task<bool?> Delete(PublicPostId postId, bool? deleteContent = null, CancellationToken Token = default);

        /// <summary>
        /// Deletes a specific comment by ID.
        /// </summary>
        /// <param name="postId">The public post ID of the post containing the comment.</param>
        /// <param name="commentId">The comment ID of the comment to delete.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The deleted comment, or null if the operation fails.</returns>
        Task<Comment?> Delete(PublicPostId postId, CommentId commentId, CancellationToken Token = default);

        /// <summary>
        /// Retrieves a community by ID.
        /// </summary>
        /// <param name="Id">The community ID.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The community, or null if not found.</returns>
        Task<Community?> Get(CommunityId Id, CancellationToken Token = default);

        /// <summary>
        /// Retrieves a community by name.
        /// </summary>
        /// <param name="Name">The name of the community to retrieve.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The community, or null if not found.</returns>
        Task<Community?> GetCommunity(string Name, CancellationToken Token = default);

        /// <summary>
        /// Retrieves a list of all communities.
        /// </summary>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>A list of communities, or null if none are available.</returns>
        Task<IEnumerable<Community>?> GetCommunities(CancellationToken Token = default);

        /// <summary>
        /// Retrieves a list of communities based on specific query parameters.
        /// </summary>
        /// <param name="Set">The query parameters to filter communities.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>A list of communities, or null if none match the query parameters.</returns>
        Task<IEnumerable<Community>?> GetCommunities(QueryParams Set, CancellationToken Token = default);

        /// <summary>
        /// Retrieves a list of communities based on a search query.
        /// </summary>
        /// <param name="searchQuery">The search query to use for finding communities.</param>
        /// <returns>A list of communities, or null if none match the search query.</returns>
        Task<IEnumerable<Community>?> GetCommunities(string searchQuery, CancellationToken Token = default);

        /// <summary>
        /// Retrieves a list of communities based on a search query and specific query parameters.
        /// </summary>
        /// <param name="searchQuery">The search query to use for finding communities, optional.</param>
        /// <param name="Set">The query parameters to filter communities, optional.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>A list of communities, or null if none match the search criteria and query parameters.</returns>
        Task<IEnumerable<Community>?> GetCommunities(string? searchQuery, QueryParams? Set, CancellationToken Token = default);

        /// <summary>
        /// Updates a specific post.
        /// </summary>
        /// <param name="post">The post with updated information.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The updated post, or null if the update fails.</returns>
        Task<Post?> Update(Post post, CancellationToken Token = default);

        /// <summary>
        /// Updates the vote status of a post.
        /// </summary>
        /// <param name="Vote">The post vote update.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The updated post, or null if the update fails.</returns>
        Task<Post?> Update(PostVote Vote, CancellationToken Token = default);

        /// <summary>
        /// Updates a specific comment.
        /// </summary>
        /// <param name="postId">The public post ID containing the comment.</param>
        /// <param name="comment">The comment with updated information.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The updated comment, or null if the update fails.</returns>
        Task<Comment?> Update(PublicPostId postId, Comment comment, CancellationToken Token = default);

        /// <summary>
        /// Updates the vote status of a comment.
        /// </summary>
        /// <param name="Vote">The comment vote update.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The updated comment, or null if the update fails.</returns>
        Task<Comment?> Update(CommentVote Vote, CancellationToken Token = default);

        ///<summary>
        /// Applies an action to a post for a specific user group.
        /// </summary>
        /// <param name="postId">The public post ID of the post to update.</param>
        /// <param name="Action">The action to apply to the post.</param>
        /// <param name="userGroup">The user group for which the action is applicable, optional.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <return>The updated post, or null if the operation fails.</return>
        Task<Post?> Update(PublicPostId postId, PostAction? Action, UserGroup? userGroup, CancellationToken Token = default);
    }
}
