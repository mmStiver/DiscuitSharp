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
    public interface IDiscuitClient
    {
        string? Token { get; }
        Task<Initial?> GetInitial();
        Task<DiscuitUser?> Authenticate(Credentials creds);
        Task<bool?> InvalidateAuth();
        Task<DiscuitUser?> GetAuthenticatedUser();
        Task<DiscuitUser?> GetUser(string UserName);
        Task<Post?> Get(PublicPostId postId);
        Task<Cursor<Post>> GetPosts(CursorIndex? cursor = null, Feed? feed = null, Sort? sort = null, int? limit = null);
        Task<Cursor<Post>> GetPosts(CommunityId Id, int? limit = null);
        Task<Cursor<Comment?>?> GetComments(PublicPostId Id);
        Task<Cursor<Comment?>?> GetComments(PublicPostId Id, CursorIndex? page);
        Task<Cursor<Comment?>?> GetComments(PublicPostId Id, CommentId? parentId = null, CursorIndex? page = null);
        Task<TextPost?> Create(TextPost post);
        Task<LinkPost?> Create(LinkPost post);
        Task<ImagePost?> Create(ImagePost post);
        Task<Comment?> Create(PublicPostId postId, Comment post);
        Task<Comment?> Create(PublicPostId postId, CommentId? parentId, Comment comment);
        Task<Post?> Delete(PublicPostId postId, bool? deleteContent = null);
        Task<Comment?> Delete(PublicPostId postId, CommentId commentId);
        Task<Community?> Get(CommunityId Id);
        Task<Community?> GetCommunity(string Name);
        Task<IEnumerable<Community>?> GetCommunities();
        Task<IEnumerable<Community>?> GetCommunities(QueryParams Set);
        Task<IEnumerable<Community>?> GetCommunities(string searchQuery);
        Task<IEnumerable<Community>?> GetCommunities(string? searchQuery, QueryParams? Set);
        Task<Post?> Update(Post post);
        Task<Post?> Update(PostVote Vote);
        Task<Comment?> Update(PublicPostId postId, Comment comment);
        Task<Comment?> Update(CommentVote Vote);
        Task<Post?> Update(PublicPostId postId, PostAction? Action, UserGroup? userGroup);
    };
    public class DiscuitClient : IDiscuitClient
    {
        HttpClient client;

        public string Token { get; private set; } = String.Empty;

        public DiscuitClient()
        {
            this.client = new();
            this.client.BaseAddress = new Uri("https://discuit.net/api/");

        }

        public DiscuitClient(HttpClient client)
        {
            this.client = client;
        }
        public DiscuitClient(IHttpClientFactory factory)
        {
            this.client = factory.CreateClient();
            this.client.BaseAddress = new Uri("https://discuit.net/api/");

        }

        public async Task<Initial?> GetInitial()
        {
            var httpResponseMessage = await client.GetAsync("_initial");
            httpResponseMessage.EnsureSuccessStatusCode();
            if (httpResponseMessage.Headers != null)
                StoreResponseHeader(httpResponseMessage.Headers);
            return await httpResponseMessage.Content.ReadFromJsonAsync<Initial>();
        }
        public async Task<DiscuitUser?> Authenticate(Credentials creds)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "_login");

            request.Headers.Add("X-Csrf-Token", this.Token);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new LowercaseNamingPolicy(),
            };
            var body = JsonSerializer.Serialize(creds, options);
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            var httpResponseMessage = await client.SendAsync(request);
            httpResponseMessage.EnsureSuccessStatusCode();
            var content = await httpResponseMessage.Content.ReadFromJsonAsync<DiscuitUser?>();
            return content;
        }

        private void StoreResponseHeader(System.Net.Http.Headers.HttpResponseHeaders header)
        {
            var cookiieHeader2 = header.GetValues("Set-Cookie")
                .Select(cookie => cookie.Split(";")[0].Split('=', 2, StringSplitOptions.RemoveEmptyEntries))
                .Where(cookie => cookie.Length == 2)
                .Select(cookie => new KeyValuePair<string, string>(cookie[0], cookie[1]));
            this.Token = cookiieHeader2.FirstOrDefault(c => c.Key == "csrftoken").Value;
        }

        public async Task<DiscuitUser?> GetAuthenticatedUser()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "_user");
            request.Headers.Add("X-Csrf-Token", this.Token);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new LowercaseNamingPolicy(),
            };
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
            var content = await httpResponseMessage.Content.ReadFromJsonAsync<DiscuitUser?>();
            return content;
        }

        public async Task<DiscuitUser?> GetUser(string UserName)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"users/{UserName}");
            request.Headers.Add("X-Csrf-Token", this.Token);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new LowercaseNamingPolicy(),
            };
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
            var content = await httpResponseMessage.Content.ReadFromJsonAsync<DiscuitUser?>();
            return content;
        }

        public async Task<bool?> InvalidateAuth()
        {
            await Send(HttpMethod.Post, "_login?action=logout");
            this.Token = String.Empty;
            return true;
        }

        public async Task<IEnumerable<Community>?> GetCommunities()
            => await GetCommunities(null, null);
        public async Task<IEnumerable<Community>?> GetCommunities(QueryParams Set)
           => await GetCommunities(null, Set);
        public async Task<IEnumerable<Community>?> GetCommunities(string searchQuery)
          => await GetCommunities(searchQuery, null);
        public async Task<IEnumerable<Community>?> GetCommunities(string? searchQuery = null, QueryParams? Set = null)
        {
            string endpoint = "communities";
            NameValueCollection queryParams = new();
            if (Set != null)
                queryParams.Add("set", Set.ToString());
            if (searchQuery != null)
                queryParams.Add("q", searchQuery);

            if (queryParams.Count > 0)
                endpoint += "?" + queryParams.ToUriQuery();

            return await Send<List<Community>?>(HttpMethod.Get, endpoint);
        }

        public async Task<Community?> Get(CommunityId Id)
            => await Send<Community?>(HttpMethod.Get, $"communities/{Id.ToString()}");

        public async Task<Community?> GetCommunity(string Name)
            => await Send<Community?>(HttpMethod.Get, $"communities/{Name}?byName=true");


        public async Task<Post?> Get(PublicPostId Id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"posts/{Id.ToString()}");
            request.Headers.Add("X-Csrf-Token", this.Token);


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
            var strContent = await httpResponseMessage.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions();
            options.Converters.Add(new PostJsonConverter());
            //options.Converters.Add(new DiscuitUserJsonConverter());
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            var content = await httpResponseMessage.Content.ReadFromJsonAsync<Post?>(options);
            return content;
        }
        public async Task<Cursor<Post>> GetPosts(CursorIndex? cursor = null, Feed? feed = null, Sort? sort = null, int? limit = null)
        {
            string endpoint = "posts";
            NameValueCollection queryParams = new();
            if (cursor != null)
                queryParams.Add("next", cursor.Value.Value);
            if (feed != null)
                queryParams.Add("feed",  feed.ToString()?.ToLowerInvariant());
            if (sort != null)
                queryParams.Add("sort",  sort.ToString()?.ToLowerInvariant());
            if (limit != null)
                queryParams.Add("limit", limit.ToString()?.ToLowerInvariant());
            if (queryParams.Count > 0)
                endpoint += "?" + queryParams.ToUriQuery();

            return await Send<Cursor<Post>?>(HttpMethod.Get, endpoint);
        }

        public async Task<Cursor<Post>> GetPosts(CommunityId Id, int? limit = null)
        {
            string endpoint = $"posts";
            NameValueCollection queryParams = new();
            queryParams.Add("CommunityId", Id.ToString());

            if (limit != null)
                queryParams.Add("limit", limit.ToString());

            if (queryParams.Count > 0)
                endpoint += "?" + queryParams.ToUriQuery();

            return await Send<Cursor<Post>?>(HttpMethod.Get, endpoint);
        }

        public async Task<Cursor<Comment?>?> GetComments(PublicPostId Id)
            => await GetComments(Id, null, null);
        public async Task<Cursor<Comment?>?> GetComments(PublicPostId Id, CursorIndex? index)
            => await GetComments(Id, null, index);
        public async Task<Cursor<Comment?>?> GetComments(PublicPostId Id, CommentId? parentId, CursorIndex? index)
        {
            string endpoint = $"posts/{Id}/comments";
            NameValueCollection queryParams = new();
            if (index.HasValue)
                queryParams.Add("next", (string)index);
            
            if (queryParams.Count > 0)
                endpoint += "?" + queryParams.ToUriQuery();

            return await Send<Cursor<Comment?>?>(HttpMethod.Get, endpoint);
        }

        public async Task<TextPost?> Create(TextPost post)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new LowercaseNamingPolicy(),
            };

            var body = JsonSerializer.Serialize(new
            {
                type = post.Type,
                title = post.Title,
                body = post.Body,
                community = post.CommunityName
            }, options);

            return await Post<TextPost?>("posts", body);
        }
        public async Task<LinkPost?> Create(LinkPost post)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new LowercaseNamingPolicy(),
            };

            var body = JsonSerializer.Serialize(new
            {
                type = post.Type,
                title = post.Title,
                url = post.Link.Url,
                community = post.CommunityName
            }, options);

            return await Post<LinkPost?>("posts", body);
        }
        public async Task<ImagePost?> Create(ImagePost post)
        {

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var body = JsonSerializer.Serialize(new
            {
                type = post.Type,
                title = post.Title,
                community = post.CommunityName,
                ImageId = post.Image.Id
            }, options);

            return await Post<ImagePost?>("posts", body);
        }
        public async Task<Comment?> Create(PublicPostId postId, Comment comment)
            => await Create(postId, null, comment);
        
        public async Task<Comment?> Create(PublicPostId postId, CommentId? parentId, Comment comment)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var body = (parentId is CommentId parent)
                ? JsonSerializer.Serialize(new { parentCommentId = parent.Value, body = comment.Body}, options)
                : JsonSerializer.Serialize(new { body = comment.Body}, options);

            return await Post<Comment?>($"posts/{postId.Value}/comments", body);
        }

        public async Task<Post?> Update(Post post)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var body = GetMutatedState(post, options);

            var strContent = new StringContent(body, Encoding.UTF8, "application/json");
            return await Send<Post?>(HttpMethod.Put, $"posts/{post.PublicId}", strContent);
        }

        public async Task<Post?> Update(PublicPostId post, PostAction? action, UserGroup? userGroup)
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


            return await Send<Post?>(HttpMethod.Put, endpoint);
        }

        public async Task<Post?> Update(PostVote Vote)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            options.Converters.Add(new PostJsonConverter());
            var jsonBody = JsonSerializer.Serialize(new { postId = Vote.Id.Value, up = Vote.Vote }, options);
            StringContent content = new(jsonBody, Encoding.UTF8, "application/json");
            ;
            return await Send<Post>(HttpMethod.Post, "_postVote", content);
        }
       public async Task<Comment?> Update(PublicPostId postId, Comment comment)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var body = GetMutatedState<string>(comment, options);

            return await Post<Comment?>($"posts/{postId.Value}/comments", body);
        }

        public async Task<Comment?> Update(CommentVote Vote) { 
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            options.Converters.Add(new PostJsonConverter());
                var jsonBody = JsonSerializer.Serialize(new { commentId = Vote.Id.Value, up = Vote.Vote }, options);
            StringContent content = new(jsonBody, Encoding.UTF8, "application/json");
            ;
            return await Send<Comment?>(HttpMethod.Post, "_commentVote", content);
        }

        public async Task<Post?> Delete(PublicPostId postId, bool? deleteContent = null)
            => await Delete<Post>(
                deleteContent switch
                {
                    true => $"posts/{postId.Value}?deleteContent=true",
                    false => $"posts/{postId.Value}?deleteContent=false",
                    _ => $"posts/{postId.Value}"
                }
                );

        public async Task<Comment?> Delete(PublicPostId postId, CommentId commentId)
            => await Delete<Comment?>( $"posts/{postId.Value}/comments/{commentId.Value}");

         private string GetMutatedState<T>(IMutableState<T> state, JsonSerializerOptions options)
            => JsonSerializer.Serialize(state.MutatedState
                            .Where(kvp => kvp.Value != null && kvp.Value is string s ? !string.IsNullOrWhiteSpace(s) : true)
                            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value), options);

        private async Task<T?> Post<T>(string endpoint, string? contentBody = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);

            if (!String.IsNullOrEmpty(this.Token))
                request.Headers.Add("X-Csrf-Token", this.Token);

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

        private async Task Send(HttpMethod method, string endpoint, StringContent? contentBody = null)
        {
            var request = new HttpRequestMessage(method, endpoint);

            if (!String.IsNullOrEmpty(this.Token))
                request.Headers.Add("X-Csrf-Token", this.Token);

            if (contentBody != null)
                request.Content = contentBody;

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
        }


        private async Task<T?> Send<T>(HttpMethod method, string endpoint, StringContent? contentBody = null)
        {
            var request = new HttpRequestMessage(method, endpoint);

            if (!String.IsNullOrEmpty(this.Token))
                request.Headers.Add("X-Csrf-Token", this.Token);

            if (contentBody != null)
                request.Content = contentBody;

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
            options.Converters.Add(new CursorConverter<Comment>());
            options.Converters.Add(new CursorConverter<Post>());
            return await httpResponseMessage.Content.ReadFromJsonAsync<T>(options);
        }

        private async Task<T?> Delete<T>(string endpoint)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, endpoint);

            if (!String.IsNullOrEmpty(this.Token))
                request.Headers.Add("X-Csrf-Token", this.Token);

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