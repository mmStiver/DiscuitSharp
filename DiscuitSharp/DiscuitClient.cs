using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using System.Collections.Specialized;
using DiscuitSharp.Core.Utils;
using DiscuitSharp.Core.Group;
using DiscuitSharp.Core.Exceptions;
using DiscuitSharp.Core.Auth;
using DiscuitSharp.Core.Content;
using DiscuitSharp.Core.Group.Serialization;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using DiscuitSharp.Core.Utility;
using System.Collections;
using DiscuitSharp.Core.Common;
using System.Net.NetworkInformation;
using Microsoft.Extensions.Options;
using DiscuitSharp.Core.Common.Serialization;
using System.Xml.Linq;

namespace DiscuitSharp.Core
{
    /// <summary>
    /// Provides a concrete implementation of IDiscuitClient for managing interactions with the Discuit web API.
    /// </summary>
    public class DiscuitClient : IDiscuitClient
    {
        HttpClient client;
        JsonSerializerOptions defaultSerializationOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        /// <summary>
        /// Gets the authentication token used in HTTP requests to the Discuit API.
        /// </summary>
        public string CSRFtoken { get; private set; } = String.Empty;

        /// <summary>
        /// Initializes a new instance of the DiscuitClient class with a new HttpClient instance.
        /// This constructor sets the base address to the Discuit API endpoint.
        /// </summary>
        public DiscuitClient()
        {
            this.client = new HttpClient();
            this.client.BaseAddress = new Uri("https://discuit.net/api/");
        }

        /// <summary>
        /// Initializes a new instance of the DiscuitClient class using an existing HttpClient instance.
        /// This constructor is typically used for testing or when the HttpClient is managed elsewhere in the application.
        /// </summary>
        /// <param name="client">The HttpClient to be used by this instance.</param>
        public DiscuitClient(HttpClient client)
        {
            this.client = client;
        }

        /// <summary>
        /// Initializes a new instance of the DiscuitClient class using an HttpClient from an IHttpClientFactory.
        /// This is useful in scenarios where HttpClient instances are managed by a factory, such as in ASP.NET Core applications.
        /// The base address is set to the Discuit API endpoint.
        /// </summary>
        /// <param name="factory">The factory used to create the HttpClient instance.</param>
        public DiscuitClient(IHttpClientFactory factory)
        {
            this.client = factory.CreateClient();
            this.client.BaseAddress = new Uri("https://discuit.net/api/");
        }

        /// <summary>
        /// Retrieves initial setup data required by the client from the Discuit API.
        /// </summary>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The initial setup data encapsulated in an Initial object, or null if unavailable.</returns>
        public async Task<Initial?> GetInitial(CancellationToken Token = default)
        {
            var httpResponseMessage = await client.GetAsync("_initial", Token);
            httpResponseMessage.EnsureSuccessStatusCode();
            if (httpResponseMessage.Headers != null)
                StoreResponseHeader(httpResponseMessage.Headers);
            return await httpResponseMessage.Content.ReadFromJsonAsync<Initial>();
        }

        /// <summary>
        /// Authenticates a user using the provided credentials and retrieves the user object from the Discuit API.
        /// </summary>
        /// <param name="creds">The credentials used to authenticate the user.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The authenticated Discuit user object, or null if authentication fails.</returns>
        public async Task<DiscuitUser?> Authenticate(Credentials creds, CancellationToken Token = default)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new LowercaseNamingPolicy(),
            };
            var body = JsonSerializer.Serialize(creds, options);
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            return await Send<DiscuitUser?>(HttpMethod.Post, "_login", content, Token);

        }

        /// <summary>
        /// Stores CSRF tokens from HTTP response headers to maintain session integrity.
        /// </summary>
        /// <param name="header">HTTP response headers containing CSRF tokens.</param>
        private void StoreResponseHeader(System.Net.Http.Headers.HttpResponseHeaders header)
        {
            var cookiieHeader2 = header.GetValues("Set-Cookie")
                .Select(cookie => cookie.Split(";")[0].Split('=', 2, StringSplitOptions.RemoveEmptyEntries))
                .Where(cookie => cookie.Length == 2)
                .Select(cookie => new KeyValuePair<string, string>(cookie[0], cookie[1]));
            this.CSRFtoken = cookiieHeader2.FirstOrDefault(c => c.Key == "csrftoken").Value;
        }

        /// <summary>
        /// Retrieves the currently authenticated user from the Discuit API.
        /// </summary>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The authenticated user's details encapsulated in a DiscuitUser object, or null if no user is authenticated.</returns>
        public async Task<DiscuitUser?> GetAuthenticatedUser(CancellationToken Token = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "_user");
            request.Headers.Add("X-Csrf-Token", this.CSRFtoken);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new LowercaseNamingPolicy(),
            };
            var httpResponseMessage = await client.SendAsync(request, Token);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                APIError? errorBody = await httpResponseMessage.Content.ReadFromJsonAsync<APIError?>();
                if (errorBody != null)
                {
                    var exception = new APIRequestException(errorBody.Value, httpResponseMessage.StatusCode, httpResponseMessage.ReasonPhrase);
                    throw exception;
                }

                httpResponseMessage.EnsureSuccessStatusCode();
            }
            var content = await httpResponseMessage.Content.ReadFromJsonAsync<DiscuitUser?>();
            return content;
        }

        /// <summary>
        /// Retrieves a specific user by username from the Discuit API.
        /// </summary>
        /// <param name="UserName">The username of the user to retrieve.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The user details encapsulated in a DiscuitUser object, or null if the user is not found.</returns>
        public async Task<DiscuitUser?> GetUser(string UserName, CancellationToken Token = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"users/{UserName}");
            request.Headers.Add("X-Csrf-Token", this.CSRFtoken);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new LowercaseNamingPolicy(),
            };
            var httpResponseMessage = await client.SendAsync(request, Token);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                APIError? errorBody = await httpResponseMessage.Content.ReadFromJsonAsync<APIError?>();
                if (errorBody != null)
                {
                    var exception = new APIRequestException(errorBody.Value, httpResponseMessage.StatusCode, httpResponseMessage.ReasonPhrase);
                    throw exception;
                }

                httpResponseMessage.EnsureSuccessStatusCode();
            }
            var content = await httpResponseMessage.Content.ReadFromJsonAsync<DiscuitUser?>();
            return content;
        }

        /// <summary>
        /// Invalidates the current user's authentication session by logging out.
        /// </summary>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>Always returns true indicating the session has been successfully invalidated.</returns>
        public async Task<bool?> InvalidateAuth(CancellationToken Token = default)
        {
            StringContent? content = null;
            await Send(HttpMethod.Post, "_login?action=logout", content, Token);
            this.CSRFtoken = String.Empty;
            return true;
        }

        /// <summary>
        /// Retrieves all communities from the Discuit API.
        /// </summary>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>A collection of communities, or null if none are available.</returns>
        public async Task<IEnumerable<Community>?> GetCommunities(CancellationToken Token = default)
            => await GetCommunities(null, null, Token);

        /// <summary>
        /// Retrieves communities from the Discuit API based on specified query parameters.
        /// </summary>
        /// <param name="Set">The query parameters to apply to the request.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>A collection of communities matching the query parameters, or null if none match.</returns>
        public async Task<IEnumerable<Community>?> GetCommunities(QueryParams Set, CancellationToken Token = default)
           => await GetCommunities(null, Set, Token);

        /// <summary>
        /// Retrieves communities from the Discuit API based on a search query.
        /// </summary>
        /// <param name="searchQuery">The search query to filter communities.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>A collection of communities matching the search query, or null if none match.</returns>
        public async Task<IEnumerable<Community>?> GetCommunities(string searchQuery, CancellationToken Token = default)
          => await GetCommunities(searchQuery, null, Token);

        /// <summary>
        /// Retrieves communities from the Discuit API based on a search query and additional query parameters.
        /// </summary>
        /// <param name="searchQuery">The search query to filter communities, optional.</param>
        /// <param name="Set">The query parameters to apply to the request, optional.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>A collection of communities matching the search query and query parameters, or null if none match.</returns>
        public async Task<IEnumerable<Community>?> GetCommunities(string? searchQuery = null, QueryParams? Set = null, CancellationToken Token = default)
        {
            string endpoint = "communities";
            NameValueCollection queryParams = new();
            if (Set != null)
                queryParams.Add("set", Set.ToString());
            if (searchQuery != null)
                queryParams.Add("q", searchQuery);

            if (queryParams.Count > 0)
                endpoint += "?" + queryParams.ToUriQuery();

            return await Send<List<Community>?>(HttpMethod.Get, endpoint, Token);
        }

        /// <summary>
        /// Retrieves a specific community by ID from the Discuit API.
        /// </summary>
        /// <param name="Id">The unique identifier of the community to retrieve.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The community, or null if it is not found.</returns>
        public async Task<Community?> Get(CommunityId Id, CancellationToken Token = default)
            => await Send<Community?>(HttpMethod.Get, $"communities/{Id.ToString()}", Token);

        /// <summary>
        /// Retrieves a specific community by name from the Discuit API.
        /// </summary>
        /// <param name="Name">The name of the community to retrieve.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The community, or null if it is not found.</returns>
        public async Task<Community?> GetCommunity(string Name, CancellationToken Token = default)
            => await Send<Community?>(HttpMethod.Get, $"communities/{Name}?byName=true", Token);

        /// <summary>
        /// Retrieves a specific post by its ID from the Discuit API.
        /// </summary>
        /// <param name="Id">The unique identifier of the post to retrieve.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The post, or null if it is not found. Throws APIRequestException if the API call is unsuccessful.</returns>
        public async Task<Post?> Get(PublicPostId Id, CancellationToken Token = default)
        {

            return await Send<Post>(HttpMethod.Get, $"posts/{Id.ToString()}", Token);
            
           // var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            //options.Converters.Add(new PostJsonConverter());
            //return await httpResponseMessage.Content.ReadFromJsonAsync<Post?>(options);
        }

        /// <summary>
        /// Retrieves a paginated list of posts with optional parameters for cursor-based pagination, feed type, sorting, and limit.
        /// </summary>
        /// <param name="cursor">The cursor index to start pagination from, if specified.</param>
        /// <param name="feed">The type of feed from which to retrieve posts, if specified.</param>
        /// <param name="sort">The sorting order of the posts, if specified.</param>
        /// <param name="limit">The maximum number of posts to retrieve, if specified.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>A cursor-based paginated list of posts.</returns>
        public async Task<Cursor<Post>?> GetPosts(CursorIndex? cursor = null, Feed? feed = null, Sort? sort = null, int? limit = null, CancellationToken Token = default)
        {
            string endpoint = "posts";
            NameValueCollection queryParams = new();
            if (cursor != null)
                queryParams.Add("next", cursor.Value.Value);
            if (feed != null)
                queryParams.Add("feed", feed.ToString()?.ToLowerInvariant());
            if (sort != null)
                queryParams.Add("sort", sort.ToString()?.ToLowerInvariant());
            if (limit != null)
                queryParams.Add("limit", limit.ToString()?.ToLowerInvariant());
            if (queryParams.Count > 0)
                endpoint += "?" + queryParams.ToUriQuery();

            return await Send<Cursor<Post>?>(HttpMethod.Get, endpoint, Token);
        }

        /// <summary>
        /// Retrieves a paginated list of posts from a specific community, with an optional limit.
        /// </summary>
        /// <param name="Id">The community ID from which to retrieve posts.</param>
        /// <param name="limit">The maximum number of posts to retrieve, if specified.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>A cursor-based paginated list of posts from the specified community.</returns>
        public async Task<Cursor<Post>?> GetPosts(CommunityId Id, CursorIndex? cursor = null, int? limit = null, CancellationToken Token = default)
        {
            string endpoint = $"posts";
            NameValueCollection queryParams = new();
            queryParams.Add("CommunityId", Id.ToString());

            if (cursor != null)
                queryParams.Add("next", (string)cursor);

            if (limit != null)
                queryParams.Add("limit", limit.ToString());

            if (queryParams.Count > 0)
                endpoint += "?" + queryParams.ToUriQuery();

            return await Send<Cursor<Post>?>(HttpMethod.Get, endpoint, Token);
        }

        /// <summary>
        /// Retrieves a cursor-based paginated list of comments for a specific post, potentially filtered by parent comment ID and with pagination.
        /// </summary>
        /// <param name="Id">The post ID to retrieve comments for.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>A cursor-based list of comments, or null if no comments are available.</returns>
        public async Task<Cursor<Comment?>?> GetComments(PublicPostId Id, CancellationToken Token = default)
            => await GetComments(Id, null, Token);

        /// <summary>
        /// Retrieves a cursor-based paginated list of comments for a specific post, potentially filtered by parent comment ID and with pagination.
        /// </summary>
        /// <param name="Id">The post ID to retrieve comments for.</param>
        /// <param name="index ">The cursor index to start pagination from, if specified.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>A cursor-based list of comments, or null if no comments are available.</returns>
        public async Task<Cursor<Comment?>?> GetComments(PublicPostId Id, CursorIndex? index, CancellationToken Token = default)
        {
            string endpoint = $"posts/{Id}/comments";
            NameValueCollection queryParams = new();
            if (index.HasValue)
                queryParams.Add("next", (string)index);

            if (queryParams.Count > 0)
                endpoint += "?" + queryParams.ToUriQuery();

            return await Send<Cursor<Comment?>?>(HttpMethod.Get, endpoint, Token);
        }

        /// <summary>
        /// Creates a new text post.
        /// </summary>
        /// <param name="post">The text post to be created.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The newly created text post, or null if the creation fails.</returns>
        public async Task<TextPost?> Create(TextPost post, CancellationToken Token = default)
        {
            var options = new JsonSerializerOptions { PropertyNamingPolicy = new LowercaseNamingPolicy() };

            var body = JsonSerializer.Serialize(new { type = post.Type.Description(), title = post.Title, body = post.Body, community = post.CommunityName }, options);
            var strContent = new StringContent(body, Encoding.UTF8, "application/json");

            return await Send<TextPost?>(HttpMethod.Post, "posts", strContent, Token);
        }

        /// <summary>
        /// Creates a new link post.
        /// </summary>
        /// <param name="post">The link post to be created.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The newly created link post, or null if the creation fails.</returns>
        public async Task<LinkPost?> Create(LinkPost post, CancellationToken Token = default)
        {
            var options = new JsonSerializerOptions { PropertyNamingPolicy = new LowercaseNamingPolicy() };

            var body = JsonSerializer.Serialize(new { type = post.Type.Description(), title = post.Title, url = post.Link.Url, community = post.CommunityName }, options);
            var strContent = new StringContent(body, Encoding.UTF8, "application/json");

            return await Send<LinkPost?>(HttpMethod.Post, "posts", strContent, Token);
        }

        /// <summary>
        /// Creates a new image post.
        /// </summary>
        /// <param name="post">The image post to be created.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The newly created image post, or null if the creation fails.</returns>
        public async Task<ImagePost?> Create(ImagePost post, CancellationToken Token = default)
        {
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var body = JsonSerializer.Serialize(new { type = post.Type.Description(), title = post.Title, community = post.CommunityName, ImageId = post.Image.Id }, options);
            var strContent = new StringContent(body, Encoding.UTF8, "application/json");

            return await Send<ImagePost?>(HttpMethod.Post, "posts", strContent, Token);

        }

        /// <summary>
        /// Creates a new comment on a specific post.
        /// </summary>
        /// <param name="postId">The ID of the post on which to comment.</param>
        /// <param name="comment">The comment to be created.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The newly created comment, or null if the creation fails.</returns>

        public async Task<Comment?> Create(PublicPostId postId, Comment comment, CancellationToken Token = default)
            => await Create(postId, null, comment, Token);

        /// <summary>
        /// Creates a new comment on a specific post, optionally as a reply to an existing comment.
        /// </summary>
        /// <param name="postId">The ID of the post on which to comment.</param>
        /// <param name="parentId">The ID of the parent comment to reply to, if applicable.</param>
        /// <param name="comment">The comment to be created.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The newly created comment, or null if the creation fails.</returns>
        public async Task<Comment?> Create(PublicPostId postId, CommentId? parentId, Comment comment, CancellationToken Token = default)
        {
            var body = (parentId is CommentId parent)
                ? JsonSerializer.Serialize(new { parentCommentId = parent.Value, body = comment.Body}, defaultSerializationOptions)
                : JsonSerializer.Serialize(new { body = comment.Body}, defaultSerializationOptions);

            var strContent = new StringContent(body, Encoding.UTF8, "application/json");
            return await Send<Comment?>(HttpMethod.Post, $"posts/{postId.Value}/comments", strContent, Token);
        }

        /// <summary>
        /// Updates an existing post with modified attributes.
        /// </summary>
        /// <param name="post">The post object containing the updated information.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The updated post object, or null if the update fails.</returns>
        public async Task<Post?> Update(Post post, CancellationToken Token = default)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var body = SerializeMutableState(post, options);

            var strContent = new StringContent(body, Encoding.UTF8, "application/json");
            return await Send<Post?>(HttpMethod.Put, $"posts/{post.PublicId}", strContent, Token);
        }

        /// <summary>
        /// Performs an action on a post, like locking or unlocking, based on the user group's permissions.
        /// </summary>
        /// <param name="post">The public post ID.</param>
        /// <param name="action">The action to be performed on the post.</param>
        /// <param name="userGroup">The user group performing the action.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The updated post object, or null if the action fails.</returns>
        /// <exception cref="InsufficientPrivledgesException">Thrown if the user group does not have sufficient privileges to perform the action.</exception>
        public async Task<Post?> Update(PublicPostId post, PostAction? action, UserGroup? userGroup, CancellationToken Token = default)
        {
            UserGroup group = (!userGroup.HasValue ? UserGroup.Normal : userGroup.Value);
            if (UserGroup.Normal == group)
                throw new InsufficientPrivledgesException();

            string endpoint = $"posts/{post.Value}";
            NameValueCollection queryParams = new();
            if (action == PostAction.Lock || action == PostAction.Unlock)
            {
                queryParams.Add("action", action.Description());
                queryParams.Add("lockAs", group.Description());
            }
            else if (action == PostAction.ChangeSpeaker)
            {
                queryParams.Add("action", action.Description());
                queryParams.Add("userGroup", group.Description());
            }
            else if (action == PostAction.Pin || action == PostAction.Unpin)
            {
                queryParams.Add("action", action.Description());
                queryParams.Add("siteWide", "false");
            }
            
            if (queryParams.Count > 0)
            
                endpoint += "?" + queryParams.ToUriQuery();


            return await Send<Post?>(HttpMethod.Put, endpoint, token: Token);
        }

        /// <summary>
        /// Updates the vote status of a post.
        /// </summary>
        /// <param name="Vote">The vote object containing the post ID and vote value (upvote/downvote).</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The updated post object, or null if the update fails.</returns>
        public async Task<Post?> Update(PostVote Vote, CancellationToken Token = default)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            options.Converters.Add(new PostJsonConverter());
            var jsonBody = JsonSerializer.Serialize(new { postId = Vote.Id.Value, up = Vote.Vote }, options);
            StringContent content = new(jsonBody, Encoding.UTF8, "application/json");
            ;
            return await Send<Post>(HttpMethod.Post, "_postVote", content, Token);
        }

        /// <summary>
        /// Updates an existing comment with modified attributes.
        /// </summary>
        /// <param name="postId">The public post ID of the comment's parent post.</param>
        /// <param name="comment">The comment object containing the updated information.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The updated comment object, or null if the update fails.</returns>
        public async Task<Comment?> Update(PublicPostId postId, Comment comment, CancellationToken Token = default)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var body = SerializeMutableState<string>(comment, options);
            var contentBody = new StringContent(body, Encoding.UTF8, "application/json");

            return await Send<Comment?>(HttpMethod.Post, $"posts/{postId.Value}/comments", contentBody, Token);
        }

        /// <summary>
        /// Updates the vote status of a comment.
        /// </summary>
        /// <param name="Vote">The vote object containing the comment ID and the vote value (upvote or downvote).</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>The updated comment object, or null if the update fails.</returns>
        public async Task<Comment?> Update(CommentVote Vote, CancellationToken Token = default) { 
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            options.Converters.Add(new PostJsonConverter());
                var jsonBody = JsonSerializer.Serialize(new { commentId = Vote.Id.Value, up = Vote.Vote }, options);
            StringContent content = new(jsonBody, Encoding.UTF8, "application/json");
            ;
            return await Send<Comment?>(HttpMethod.Post, "_commentVote", content, Token);
        }

        /// <summary>
        /// Deletes a post by its ID, with an option to delete its content.
        /// </summary>
        /// <param name="postId">The public post ID of the post to be deleted.</param>
        /// <param name="deleteContent">Optional. A boolean flag indicating whether the content of the post should also be deleted. If not specified, the deletion will proceed with default permissions.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>True if the deletion was successful, otherwise returns null.</returns>
        public async Task<bool?> Delete(PublicPostId postId, bool? deleteContent = null, CancellationToken Token = default)
        {
            HttpMethod method = HttpMethod.Delete;
                String endpoint = deleteContent switch
                {
                    true => $"posts/{postId.Value}?deleteContent=true",
                    false => $"posts/{postId.Value}?deleteContent=false",
                    _ => $"posts/{postId.Value}?deleteAs=Normal"
                }; 
            StringContent? sc = null;
            await Send(method, endpoint, sc, Token);
            return true;
        }

        /// <summary>
        /// Deletes a comment by a Post Id and Comment ID.
        /// </summary>
        /// <param name="postId">The public post ID of the post to be deleted.</param>
        /// <param name="commentId">The public post Id of the post to be deleted.</param>
        /// <param name="Token">The cancellation token to monitor for cancellation requests. It allows the operation to be cancelled before completion.</param>
        /// <returns>True if the deletion was successful, otherwise returns null.</returns>
        public async Task<Comment?> Delete(PublicPostId postId, CommentId commentId, CancellationToken Token = default)
            => await Send<Comment?>(HttpMethod.Delete, $"posts/{postId.Value}/comments/{commentId.Value}", token: Token);

        private string SerializeMutableState<T>(IMutableState<T> state, JsonSerializerOptions options, CancellationToken Token = default)
            => JsonSerializer.Serialize(state.MutatedState
                            .Where(kvp => kvp.Value != null && kvp.Value is string s ? !string.IsNullOrWhiteSpace(s) : true)
                            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value), options);

        private async Task<T?> Post<T>(string endpoint, string? contentBody = null, CancellationToken Token = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);

            if (!String.IsNullOrEmpty(this.CSRFtoken))
                request.Headers.Add("X-Csrf-Token", this.CSRFtoken);

            if (contentBody != null)
                request.Content = new StringContent(contentBody, Encoding.UTF8, "application/json");

            var httpResponseMessage = await client.SendAsync(request);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                APIError? errorBody = await httpResponseMessage.Content.ReadFromJsonAsync<APIError?>();
                if (errorBody != null)
                {
                    var exception = new APIRequestException(errorBody.Value, httpResponseMessage.StatusCode, httpResponseMessage.ReasonPhrase);
                    throw exception;
                }

                httpResponseMessage.EnsureSuccessStatusCode();
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            options.Converters.Add(new PostJsonConverter());
            return await httpResponseMessage.Content.ReadFromJsonAsync<T>(options);

        }

        private async Task Send(HttpMethod method, string endpoint, StringContent? contentBody = null, CancellationToken Token = default)
        {
            var request = new HttpRequestMessage(method, endpoint);

            if (!String.IsNullOrEmpty(this.CSRFtoken))
                request.Headers.Add("X-Csrf-Token", this.CSRFtoken);

            if (contentBody != null)
                request.Content = contentBody;

            var httpResponseMessage = await client.SendAsync(request, Token);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                APIError? errorBody = await httpResponseMessage.Content.ReadFromJsonAsync<APIError?>(defaultSerializationOptions, Token);
                if (errorBody != null)
                {
                    var exception = new APIRequestException(errorBody.Value, httpResponseMessage.StatusCode, httpResponseMessage.ReasonPhrase);
                    throw exception;
                }

                httpResponseMessage.EnsureSuccessStatusCode();
            }
        }

        private async Task<T?> Send<T>(HttpMethod method, string endpoint, CancellationToken token = default)
            => await Send<T>(method, endpoint, (StringContent?)null, token);
        private async Task<T?> Send<T>(HttpMethod method, string endpoint, StringContent? contentBody, CancellationToken token)
        {
            var request = new HttpRequestMessage(method, endpoint);

            if (!String.IsNullOrEmpty(this.CSRFtoken))
                request.Headers.Add("X-Csrf-Token", this.CSRFtoken);

            if (contentBody != null)
                request.Content = contentBody;
            try { 
            var httpResponseMessage = await client.SendAsync(request, token);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                var responseStr = await httpResponseMessage.Content.ReadAsStringAsync(token);
                if(!String.IsNullOrEmpty(responseStr)) { 
                    APIError? errorBody = JsonSerializer.Deserialize<APIError?>(responseStr, defaultSerializationOptions);
                    if (errorBody != null)
                    {
                        var exception = new APIRequestException(errorBody.Value, httpResponseMessage.StatusCode, httpResponseMessage.ReasonPhrase);
                        throw exception;
                    }
                }

                httpResponseMessage.EnsureSuccessStatusCode();
            }
            
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            options.Converters.Add(new PostJsonConverter());
            options.Converters.Add(new CursorConverter<Comment>());
            options.Converters.Add(new CursorConverter<Post>());
            return await httpResponseMessage.Content.ReadFromJsonAsync<T>(options, token);
        
            } catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
}
        }

        private async Task<T?> Delete<T>(string endpoint, CancellationToken Token = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, endpoint);

            if (!String.IsNullOrEmpty(this.CSRFtoken))
                request.Headers.Add("X-Csrf-Token", this.CSRFtoken);

            var httpResponseMessage = await client.SendAsync(request);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                APIError? errorBody = await httpResponseMessage.Content.ReadFromJsonAsync<APIError?>();
                if (errorBody != null)
                {
                    var exception = new APIRequestException(errorBody.Value, httpResponseMessage.StatusCode, httpResponseMessage.ReasonPhrase);
                    throw exception;
                }

                httpResponseMessage.EnsureSuccessStatusCode();
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            options.Converters.Add(new PostJsonConverter());
            return await httpResponseMessage.Content.ReadFromJsonAsync<T>(options);

        }

    }
}