using DiscuitSharp.Core;
using DiscuitSharp.Core.Auth;
using DiscuitSharp.Core.Content;
using DiscuitSharp.Core.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DiscuitSharp.Test.Authenticated
{
    public partial class AuthTestFixture : FakeHttpTestFixture
    {
        public async Task<IDiscuitClient> AuthenticatedClient()
        {
            var client = await InitializeClient();
            var credentials = new Credentials("mmstiver", "password123");
            _ = await client.Authenticate(credentials);
            return client;
        }


        protected override (HttpStatusCode, string) Content(Uri? url, string Method, JsonObject? json)
            =>  (url?.PathAndQuery, Method) switch
            {
                { PathAndQuery: "/api/_initial", Method: "GET" } => (HttpStatusCode.OK, Initial),
                { PathAndQuery: "/api/_user", Method: "GET" } => (HttpStatusCode.OK, User),
                { PathAndQuery: "/api/_login", Method: "POST" } => checkCredentials(json!["username"]?.GetValue<string>(), json!["password"]?.GetValue<string>()),
                { PathAndQuery: "/api/communities?set=Subscribed", Method: "GET" } => (HttpStatusCode.OK, $"{SubscribedCommunities}"),
                { PathAndQuery: "/api/_uploads", Method: "GET" } => (HttpStatusCode.OK, $"{ImageUpload}"),
                { PathAndQuery: "/api/posts/Rg8TkGaE", Method: "GET" } => (HttpStatusCode.OK, AuthTestFixture.ReferenceTextPost),
                { PathAndQuery: "/api/posts/dfghfgh", Method: "GET" } => (HttpStatusCode.OK, AuthTestFixture.ReferenceLinkPostContent),
                { PathAndQuery: "/api/posts/vclkj85d", Method: "GET" } => (HttpStatusCode.OK, AuthTestFixture.ReferenceImagePostContent),
                { PathAndQuery: "/api/posts/kvzM1JLq", Method: "GET" } => (HttpStatusCode.OK, @"{ ""id"":""s5a4654dsf54c16a5s4df654as"", ""publicId"" : ""kvzM1JLq"", ""type"":""text"", ""comments"":[{""commentId"":""17c93f8221b8ded65ca6dcce""}] }"),
                { PathAndQuery: "/api/posts", Method: "POST" } => CommunityContent(json!["type"]?.GetValue<int>(),
                                                                        json!["title"]?.GetValue<string>(),
                                                                        json!["community"]?.GetValue<string>(),
                                                                        json!["body"]?.GetValue<string>(),
                                                                        json!["url"]?.GetValue<string>(),
                                                                        json!["imageId"]?.GetValue<string>()),
                { PathAndQuery: "/api/posts/0000000000", Method: "GET" } => (HttpStatusCode.OK, @"{{""status"":404,""code"":""post/not-found"",""message"":""Post(s) not found.""}}"),
                { PathAndQuery: "/api/posts/RakY4sqi", Method: "GET" } => (HttpStatusCode.OK, GetPost),
                { PathAndQuery: "/api/posts/RakY4sqi?fetchCommunity=true", Method: "GET" } => (HttpStatusCode.OK, GetPost),
                { PathAndQuery: "/api/posts/RakY4sqi?fetchCommunity=false", Method: "GET" } => (HttpStatusCode.OK, GetPostWithoutFetch),
                { PathAndQuery: "/api/posts/A3c39F9", Method: "GET" } => (HttpStatusCode.OK, GetPostWithComments),

                { PathAndQuery: "/api/posts/0000000000", Method: "DELETE" } => (HttpStatusCode.OK, @"{{""status"":404,""code"":""post/not-found"",""message"":""Post(s) not found.""}}"),
                { PathAndQuery: "/api/posts/fL0ounHq", Method: "DELETE" } => (HttpStatusCode.OK, $"{DeleteTextPost}"),
                { PathAndQuery: "/api/posts/fL0ounHq?DeleteAs=mods", Method: "DELETE" } => (HttpStatusCode.BadRequest, @"{ ""status"":400,""code"":""user/invalid-group"",""message"":""Invalid user-group.""}"),
                { PathAndQuery: "/api/posts/fL0ounHq?DeleteAs=admins", Method: "DELETE" } => (HttpStatusCode.BadRequest, @"{ ""status"":400,""code"":""user/invalid-group"",""message"":""Invalid user-group.""}"),
                { PathAndQuery: "/api/posts/fL0ounHq?deleteContent=false", Method: "DELETE" } => (HttpStatusCode.OK, $"{DeleteTextPost}"),
                { PathAndQuery: "/api/posts/fL0ounHq?deleteContent=true", Method: "DELETE" } => (HttpStatusCode.OK, $"{DeleteTextPostContent}"),
                { PathAndQuery: "/api/posts/dfghfgh?deleteContent=true", Method: "DELETE" } => (HttpStatusCode.OK, $"{DeleteLinkPostContent}"),
                { PathAndQuery: "/api/posts/vclkj85d?deleteContent=true", Method: "DELETE" } => (HttpStatusCode.OK, $"{DeleteImagePostContent}"),
                
                { PathAndQuery: "/api/posts/Rg8TkGaE", Method: "PUT" } => PostContent(json!["title"]?.GetValue<string>(),
                                                                        json!["body"]?.GetValue<string>(),
                                                                        json!["url"]?.GetValue<string>(),
                                                                        json!["imageId"]?.GetValue<string>()),
                { PathAndQuery: "/api/posts/dfghfgh", Method: "PUT" } => PostContent(json!["title"]?.GetValue<string>(),
                                                                        json!["body"]?.GetValue<string>(),
                                                                        json!["url"]?.GetValue<string>(),
                                                                        json!["imageId"]?.GetValue<string>()),
                { PathAndQuery: "/api/posts/vclkj85d", Method: "PUT" } => PostContent(json!["title"]?.GetValue<string>(),
                                                                        json!["b9l5Om_AVody"]?.GetValue<string>(),
                                                                        json!["url"]?.GetValue<string>(),
                                                                        json!["imageId"]?.GetValue<string>()),
                { PathAndQuery: "/api/posts/Rg8TkGaE?action=lock&lockAs=mods", Method: "PUT" } => (HttpStatusCode.Forbidden, @"{ ""status"":403,""code"":""not_mod"",""message"":""You are not a moderator.""}"),
                { PathAndQuery: "/api/posts/Rg8TkGaE?action=unlock&lockAs=mods", Method: "PUT" } => (HttpStatusCode.Forbidden, @"{ ""status"":403,""code"":""not_mod"",""message"":""You are not a moderator.""}"),
                { PathAndQuery: "/api/posts/Rg8TkGaE?action=changeAsUser&userGroup=mods", Method: "PUT" } => (HttpStatusCode.Forbidden, @"{ ""status"":403,""code"":""not_mod"",""message"":""You are not a moderator.""}"),
                { PathAndQuery: "/api/posts/Rg8TkGaE?action=lock&lockAs=admins", Method: "PUT" } => (HttpStatusCode.Forbidden, @"{ ""status"":403,""code"":""not_admin"",""message"":""You are not a administrator.""}"),
                { PathAndQuery: "/api/posts/Rg8TkGaE?action=unlock&lockAs=admins", Method: "PUT" } => (HttpStatusCode.Forbidden, @"{ ""status"":403,""code"":""not_admin"",""message"":""You are not a administrator.""}"),
                { PathAndQuery: "/api/posts/Rg8TkGaE?action=changeAsUser&userGroup=admins", Method: "PUT" } => (HttpStatusCode.Forbidden, @"{ ""status"":403,""code"":""not_admin"",""message"":""You are not a administrator.""}"),

                { PathAndQuery: "/api/_postVote", Method: "POST" } => upboatContent(json!["postId"]?.GetValue<string?>(), json!["up"]?.GetValue<bool?>()),
                { PathAndQuery: "/api/_commentVote", Method: "POST" } => upboatContent(json!["commentId"]?.GetValue<string?>(), json!["up"]?.GetValue<bool?>()),

                { PathAndQuery: "/api/posts/kvzM1JLq/comments", Method: "POST" } => commentContent(json!["parentCommentId"]?.GetValue<string>(), json!["body"]?.GetValue<string>()),
                { PathAndQuery: "/api/posts/kvzM1JLq/comments", Method: "PUT" } => commentContent(json!["parentCommentId"]?.GetValue<string>(), json!["body"]?.GetValue<string>()),

                { PathAndQuery: "/api/posts/icFQXwoe/comments/17cb1e8e4cac3e6c78a697a1", Method: "DELETE" } => (HttpStatusCode.Forbidden, @"{""status"":403,""code"":""comment_deleted"",""message"":""Comment(s) deleted.""}"),
                { PathAndQuery: "/api/posts/kvzM1JLq/comments/72547b41d89423318430fdeb", Method: "DELETE" } => (HttpStatusCode.OK, UserDeletedComment),
                
                _ => (HttpStatusCode.NotFound, "")
            };

        private (HttpStatusCode, string) PostContent(string? title, string? body, string? url, string? imageId)
            =>(title, body, url, imageId) switch
            {
                { title: null, body: null, url: null, imageId: null }   => (HttpStatusCode.OK, ReferenceTextPost),
                { title: "updated title", body: null, url: null, imageId: null }        => (HttpStatusCode.OK, UpdateTitlePostContent),
                { title: null, body: "updated value", url: null, imageId: null }    => (HttpStatusCode.OK, UpdateTextPostContent),
                { title: "updated title", body: "updated value", url: null, imageId: null } => (HttpStatusCode.OK, UpdateContent),

                { title: null, body:null, url: "https://discuit.net/page", imageId: null } => (HttpStatusCode.OK, ReferenceLinkPostContent),
                { title: null, body: null, url: "https://www.example.net/page", imageId: null } => (HttpStatusCode.OK, UpdateLinkPostContent),

                { title: null, body: null, url: null, imageId: "0f92u3jfodslfj3" } => (HttpStatusCode.OK, ReferenceImagePostContent),
                { title: null, body :null, url: null, imageId: "j09K8xasdsfYlkj3jf2" } => (HttpStatusCode.OK, UpdateImagePostContent),

                _ => throw new Exception("unknown error")
            };
        private (HttpStatusCode OK, string) CommunityContent(int? type, string? title, string? community, string? body, string? url, string? ImageId)
            => (type, title, community) switch
            {
                { type: _, title: not null, community: null } => (HttpStatusCode.BadRequest, @"{""status"":400,""code"":""community/not-found"",""message"":""Community not found.""}"),
                { type: _, title: null or "", community: not null } => (HttpStatusCode.BadRequest, @"{""status"":400,""code"":""community/not-found"",""message"":""Title too short.""}"),
                { type: 1, title: not null, community: not null } => (HttpStatusCode.OK, NewTextPost),
                { type: 2, title: not null, community: not null } => (ImageId == "17c67e1be2b732bfd47") ? (HttpStatusCode.OK, NewImagePost) : (HttpStatusCode.BadRequest, @"{""status"":400,""code"":""invalid_image_id"",""message"":""Invalid image ID.""}"),
                { type: 3, title: not null, community: not null } => (HttpStatusCode.OK, NewLinkPost),
                //{"status":400,"code":"invalid_image_id","message":"Invalid image ID."}
                { type: _, title: not null, community: not null } => (HttpStatusCode.BadRequest, @"{""status"":400,""code"":""post-type/unsupported"",""message"":""Unsupported post type.""}"),
                _ => throw new Exception("unknown error")
            };

        (HttpStatusCode, string) checkCredentials(string? username, string? password)
           => (username, password) switch
           {
               { username: "mmstiver", password: "password123" } => (HttpStatusCode.OK, User),
               { username: "CodyRhodes" } => (HttpStatusCode.Forbidden, String.Empty),
               { username: "RheaRipley", password: not "password123" } => (HttpStatusCode.Unauthorized, String.Empty),
               _ => (HttpStatusCode.NotFound, userNotFound)
           };

        (HttpStatusCode, string) upboatContent(string? Id, bool? vote)
          => (Id, vote) switch
          {
              { Id: "G7YMijpa", vote: true} => (HttpStatusCode.OK, UpvotedPost),
              { Id: "G7YMijpa", vote: false } => (HttpStatusCode.OK, DownvotedPost),
              { Id: "G7YMijpa", vote: null } => (HttpStatusCode.OK, ReferenceUnvotedPost),
             
              { Id: "17c68df98f91eeea40da0934", vote: true } => (HttpStatusCode.OK, UpvotedPost),
              { Id: "17c68df98f91eeea40da0934", vote: false } => (HttpStatusCode.OK, DownvotedPost),
              { Id: "17c68df98f91eeea40da0934", vote: null } => (HttpStatusCode.OK, ReferenceUnvotedPost),
              { Id: "4d4s5sd4f6s4dee6f54s88dff", vote : _ } => (HttpStatusCode.Forbidden, @"{ ""status"":403,""code"":""comment_deleted"",""message"":""Comment(s) deleted.""}"),

              _ => (HttpStatusCode.NotFound, @"{""status"":404,""code"":""post/not-found"",""message"":""Post(s) not found.""}")
          };

        (HttpStatusCode, string) commentContent(string? parentId, string? txt)
          => (parentId, txt) switch
          {
              { parentId: null, txt: null } => (HttpStatusCode.OK, NewComment),
              { parentId: null, txt: "Newly Created Comment" } => (HttpStatusCode.OK, NewComment),
              { parentId: null, txt: "Freshly Updated Body" } => (HttpStatusCode.OK, UpdatedComment),
              { parentId: "00000", txt: "100" } => (HttpStatusCode.InternalServerError, "{\"status\":500,\"message\":\"Internal Server Error\"}"),
              { parentId: "2242", txt: "1" } => (HttpStatusCode.OK, UpvotedPost),
              { parentId: "17c93f8221b8ded65ca6dcce", txt: "Created Nested Comment" } => (HttpStatusCode.OK, NestedComment),
              _ => (HttpStatusCode.NotFound, @"{""status"":404,""code"":""post/not-found"",""message"":""Post(s) not found.""}")
          };

        const string userNotFound = @"{""status"":404,""code"":""user_not_found"",""message"":""User not found.""}";

        const string Initial = """
            {"reportReasons":
                [
                    {"id":1,"title":"Breaks community rules","description":null},
                    {"id":2,"title":"Copyright violation","description":null},
                    {"id":3,"title":"Spam","description":null},
                    {"id":4,"title":"Pornography","description":null}
                ]
            ,"user":null,
            "communities":
                [
                    {
                    "id":"176abdabbb1c2f5b97786d26","userId":"17692e04a6576d682930a4f5","name":"AskDiscuit","nsfw":false,"about":"Ask and answer thought-provoking and fun questions for entertainment.","noMembers":5734,
                        "proPic":{"id":"17a369c3478c2b3ae91d9e0a","format":"jpeg","mimetype":"image/jpeg","width":300,"height":295,"size":33809,"averageColor":"rgb(85,85,85)","url":"/images/17a369c3478c2b3ae91d9e0a.jpeg?sig=YiskekdJ8uMJu903e3P-gFbo8s1wARrgXzx1RtlxsLU",
                            "copies":[
                                        {"name":"tiny","width":50,"height":50,"boxWidth":50,"boxHeight":50,"objectFit":"cover","format":"jpeg","url":"/images/17a369c3478c2b3ae91d9e0a.jpeg?fit=cover\u0026sig=hQZrf2Cz_Ff5riJywnmH3Ryzjx_HbdkT24OswVOgIPM\u0026size=50"},
                                        {"name":"small","width":120,"height":120,"boxWidth":120,"boxHeight":120,"objectFit":"cover","format":"jpeg","url":"/images/17a369c3478c2b3ae91d9e0a.jpeg?fit=cover\u0026sig=vdqL5-4teMVZkGnRUlX53vv1L1sHtiz-kxeKk5EMsJ8\u0026size=120"},
                                        {"name":"medium","width":200,"height":200,"boxWidth":200,"boxHeight":200,"objectFit":"cover","format":"jpeg","url":"/images/17a369c3478c2b3ae91d9e0a.jpeg?fit=cover\u0026sig=cKN6UdXBZDvEx1VpRW27PUm6kVros4TdNCHbT9lOA9Q\u0026size=200"}
                                     ]
                                  },
                                  "bannerImage":{"id":"17a369c34879088f3e968e39","format":"jpeg","mimetype":"image/jpeg","width":1920,"height":1080,"size":54516,"averageColor":"rgb(62,3,188)",
                                  "url":"/images/17a369c34879088f3e968e39.jpeg?sig=8NLnaOk4R0BXUaU6TRwzVqD6GvzpyvHuk0X9N3ZRu0c",
                                  "copies":[
                                    {"name":"small","width":720,"height":240,"boxWidth":720,"boxHeight":240,"objectFit":"cover","format":"jpeg","url":"/images/17a369c34879088f3e968e39.jpeg?fit=cover\u0026sig=wCI-YpIFZmGbCjcFqaPgD_pKY0ZKv7bcnKM2LHcuNng\u0026size=720x240"},
                                    {"name":"large","width":1440,"height":480,"boxWidth":1440,"boxHeight":480,"objectFit":"cover","format":"jpeg","url":"/images/17a369c34879088f3e968e39.jpeg?fit=cover\u0026sig=veJgTXeIh-N1X7Y-rXPUtMop6Q1bB5_Y8wEN6vqm9W0\u0026size=1440x480"}]}
                                    ,"createdAt":"2023-06-21T17:45:55Z","deletedAt":null,"userJoined":null,"userMod":null,"isMuted":false,"mods":null,"rules":null,"ReportsDetails":null
                    }
                ],
            "noUsers":6111,
            "bannedFrom":null,
            "vapidPublicKey":"BKXheW4qETlYKwBdCnpc8bGGjZnNFCAOIorzalJgee_tocXboYJyqoxhxkswWyAPnu7amWegukc2u5I7z773QtM",
            "mutes":{"communityMutes":[],"userMutes":[]}}
            """;

        const string NewTextPost = """
            {
            "id":"17c68636766fdade1af11c4d",
            "type":"text",
            "title":"First Post",
            "body":"Hi Mom!",
            "author":
            {
                "id":"17b264a01e464aba89939bf7",
                "username":"mmstiver"
            },
            "publicId":"fL0ounHq",
            "userId":"17b264a01e464aba89939bf7",
            "username":"mmstiver",
            "communityId":"177e4d1ddede4a7920e6a4e1",
            "communityName":"general"
            }
            """;
        const string NewLinkPost = """
            {
              "id": "17c6a0cfdcdc0def6296c2b1",
              "type": "link",
              "publicId": "RTF6SCil",
              "userId": "17b264a01e464aba89939bf7",
              "username": "mmstiver",
              "communityId": "176abc2fad2abe7ce1c190a8",
              "communityName": "programming",
              "title": "First Link",
              "body": null,
              "image": null,
              "link": {
                "url": "https://www.example.com/9iLxR1h2208",
                "hostname": "www.example.com"
              },
              "upvotes": 1,
              "author": {
                "id": "17b264a01e464aba89939bf7",
                "username": "mmstiver"
              }
            }
            """;
        const string NewImagePost = """
            {
                "id" : "17c67e1be2b732bfd47",
                "type" : "image",
                "publicId" : "GMVnX33z",
                "userId" : "176fb6c266a2d10402a2b",
                "username" : "test",
                "communityId" : "1775f028fe0eed6d399b7f26",
                "communityName" : "DiscuitMeta",
                "communityProPic" : null,
                "communityBannerImage" : null,
                "title" : "First Image",
                "body" : null,
                "image" : null,
                "author" : { "id": "176fb67a0c266a2d10402a2b", "username": "mmstiver"} ,
                "image" : { "id": "17c67e1be2b732bfd47", "url": "/images/17c6a2e6d55990e34c0b1978.jpeg?sig=D-XMcoQPjPPcUEO1EpBwdmQM5shL-wfw9tE2RmA-sjs"} 
            }
            """;

        const string DeleteTextPost = """
            {
                "id":"17c68636766fdade1af11c4d",
                "type":"text",
                "publicId":"fL0ounHq",
                "deleted":true,
                "deletedAt":"2024-04-16T19:09:30.7075647Z",
                "deletedAs":"normal",
                "deletedContent":false,
                "body":"Text Content"
            }
            """;

        const string DeleteTextPostContent = """
            {
                "id":"17c68636766fdade1af11c4d",
                "type":"text",
                "publicId":"fL0ounHq",
                "deleted":true,
                "deletedAt":"2024-04-16T19:09:30.7075647Z",
                "deletedAs":"normal",
                "deletedContent":true,
                "body":null
            }
            """;

        const string DeleteLinkPostContent = """
            {
                "id":"17c68636766fdade1af11c4d",
                "type":"link",
                "publicId":"dfghfgh",
                "deleted":true,
                "deletedAt":"2024-04-16T19:09:30.7075647Z",
                "deletedAs":"normal",
                "deletedContent":true,
                "url":null
            }
            """;

        const string DeleteImagePostContent = """
            {
                "id":"17c68636766fdade1af11c4d",
                "type":"image",
                "publicId":"vclkj85d",
                "deleted":true,
                "deletedAt":"2024-04-16T19:09:30.7075647Z",
                "deletedAs":"normal",
                "deletedContent":true,
                "image":null
            }
            """;
        
        const string ReferenceTextPost = """
                        {
              "id": "17c68bb8829688ba13844398",
              "type": "text",
              "publicId": "Rg8TkGaE",
              "title": "original title",
              "body": "Oh, Canada",
              "editedAt": "2024-04-15T19:45:32Z",
              "image": null,
              "link": null
            }
            """;

        const string UpdateContent = """
            {
                 "id": "17c68bb8829688ba13844398",
                "type": "text",
                "publicId": "Rg8TkGaE",
                "title": "updated title",
                "body": "updated value",
                "editedAt": "2024-04-17T12:05:11Z",
            
                "image": null,
                "link": null
            }
            """;

        const string UpdateTextPostContent = """
            {
                 "id": "17c68bb8829688ba13844398",
                "type": "text",
                "publicId": "Rg8TkGaE",
                "title": "original title",
                "body": "updated value",
                "editedAt": "2024-04-17T12:05:11Z",
            
                "image": null,
                "link": null
            }
            """;

        const string UpdateTitlePostContent = """
            {
                "id": "17c68bb8829688ba13844398",
                "type": "text",
                "publicId": "Rg8TkGaE",
                "title": "updated title",
                "body": "Oh, Canada", 
                "editedAt": "2024-04-17T12:05:11Z",
            
                "image": null,
                "link": null
            }
            """;

        const string ReferenceLinkPostContent = """
                        {
              "id": "17c68bb8829688ba13844398",
              "type": "link",
              "publicId": "dfghfgh",
              "title": "original title",
              "editedAt": "2024-04-15T19:45:32Z",
              "body": null,
              "image": null,
              "link": {
                "url": "https://discuit.net/page",
                "host": "www.discuit.net"
              }
            }
            """;

        const string UpdateLinkPostContent = """
            {
                "id":"17c68636766fdade1af11c4d",
                "type":"link",
                "publicId":"dfghfgh",
                "editedAt": "2024-04-17T12:05:11Z",
            
                "link": {
                   url: "https://www.example.net/page",
                   host: "www.example.net"
                }
            }
            """;

        const string ReferenceImagePostContent = """
                        {
              "id": "17c68bb8829688ba13844398",
              "type": "image",
              "publicId": "vclkj85d",
              "title": "original title",
              "editedAt": "2024-04-15T19:45:32Z",
            
              "body": null,
              "image": null,
              "image": {
                    "id": "0f92u3jfodslfj3",
                    "url": "https://discuit.net/image2.jpg"
                }
            }
            """;

        const string UpdateImagePostContent = """
            {
                "id":"17c68636766fdade1af11c4d",
                "type":"image",
                "publicId":"vclkj85d",
                "editedAt": "2024-04-17T12:05:11Z",
            
                "image": {
                    "id": "j09K8xasdsfYlkj3jf2",
                    "url": "https://example.net/image2.jpg"
                }
            }
            """;
             const string CreatedComment = """
                        {
              "id": "dlfjdf283839joidfjdf",
              "postId": "17c81a0e3f77cfb3fcea5a11",
              "postPublicId": "W9hs3X0T",
              "body": "Newly created test Body.",
              "createdAt": "2024-04-20T22:18:34Z",
              }
            """;

        const string ImageUpload = """
            {"id":"17c6a2e6d55990e34c0b1978",
            "format":"jpeg",
            "mimetype":"image/jpeg",
            "width":1199,
            "height":915,
            "size":82778,
            "averageColor":"rgb(85,85,85)",
            "url":"/images/17c6a2e6d55990e34c0b1978.jpeg?sig=D-XMcoQPjPPcUEO1EpBwdmQM5shL-wfw9tE2RmA-sjs",
            "copies":[]}
            
            """;

        protected const string DiscuitDev = """

                 {
                    "id": "17b41605558f441a4ca05ffb",
                    "userId": "176c98ce3fb02f05a7f3a24d",
                    "name": "DiscuitDev",
                    "userJoined":true,
                    "about": "A community built around the open-source development of discuit.net. Documentation of the API is available at [docs.discuit.net](https://docs.discuit.net)."
                }
                """;
        protected const string Funny =
        """
         {
            "id": "176abc4a51bad7a2313961e3",
            "userId": "17692e04a6576d682930a4f5",
            "name": "funny",
                "userJoined":true,
         
            "about": "Jokes, funny pictures and videos you find on the internet that are not memes share them here."
         }
         """;

        protected const string General = """
            {
                    "id": "17692e122def73f25bd757e0",
                    "userId": "17692e04a6576d682930a4f5",
                    "name": "general",
                    "userJoined":true,
            
                    "about": "General chat community. For everything that doesn't belong in other communities."
                }
            """;
        protected const string SubscribedCommunities = "[" + Funny + "," + General + "," + DiscuitDev + "]";

    }
}

/**
 * 
 *\/_uploads => 
 * return on upload: { "id":"17c6a2e6d55990e34c0b1978","format":"jpeg","mimetype":"image/jpeg","width":1199,"height":915,"size":82778,"averageColor":"rgb(85,85,85)","url":"/images/17c6a2e6d55990e34c0b1978.jpeg?sig=D-XMcoQPjPPcUEO1EpBwdmQM5shL-wfw9tE2RmA-sjs","copies":[]}

*
* **/
