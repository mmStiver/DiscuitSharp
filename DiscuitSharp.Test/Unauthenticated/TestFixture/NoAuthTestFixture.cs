using DiscuitSharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DiscuitSharp.Test.Unauthenticated
{
    public partial class NoAuthTestFixture : FakeHttpTestFixture
    {
        public NoAuthTestFixture() : base()
        {}
       
        protected override (HttpStatusCode, string) Content(Uri? url, string Method, JsonObject? json)
             => (url?.PathAndQuery, Method) switch
             {
                 { PathAndQuery: "/api/_initial", Method: "GET" } => (HttpStatusCode.OK, Initial),
                 { PathAndQuery: "/api/_user", Method: "GET" } => (HttpStatusCode.Unauthorized, @"{""status"":401,""code"":""not_logged_in"",""message"":""User is not logged in.""}"),
                 { PathAndQuery: "/api/_login?action=logout", Method: "POST" } => (HttpStatusCode.InternalServerError, @"{""status"":500,""message"":""Internal Server Error""}"),
                 { PathAndQuery: "/api/_login", Method: "POST" } => checkCredentials(json!["username"]?.GetValue<string>(), json!["password"]?.GetValue<string>()),
                 { PathAndQuery: "/api/users/previnder", Method: "GET" } => (HttpStatusCode.OK, OtherUser),
                 { PathAndQuery: "/api/users/RomanReigns", Method: "GET" } => (HttpStatusCode.NotFound, userNotFound),
                 { PathAndQuery: "/api/communities?q=gen", Method: "GET" } => (HttpStatusCode.OK,   General),
                 { PathAndQuery: "/api/communities?set=All", Method: "GET" } => (HttpStatusCode.OK, AllCommunities),
                 { PathAndQuery: "/api/communities?set=Default&q=ask", Method: "GET" } => (HttpStatusCode.OK, $"[{FakeHttpTestFixture.Ask}]"),
                 { PathAndQuery: "/api/communities?set=Default", Method: "GET" } => (HttpStatusCode.OK, $"{DefaultCommunities}"),
                 { PathAndQuery: "/api/communities?set=Subscribed", Method: "GET" } => (HttpStatusCode.Unauthorized, @"{""status"":401,""code"":""not_logged_in"",""message"":""User is not logged in.""}"),
                 { PathAndQuery: "/api/communities", Method: "GET" } => (HttpStatusCode.OK, $"{AllCommunities}"),
                 { PathAndQuery: "/api/communities/176ef2e09e28701c14c0c148", Method: "GET" } => (HttpStatusCode.OK, DetailedCommunity),
                 { PathAndQuery: "/api/communities/general?byName=true", Method: "GET" } => (HttpStatusCode.OK, $"{ DetailedCommunity }"),
                 { PathAndQuery: "/api/communities/notExist?byName=true", Method: "GET" } => (HttpStatusCode.NotFound, @"{""status"":404,""code"":""community/not-found"",""message"":""Community not found.""}"),
                 { PathAndQuery: "/api/posts/j4ji0rd", Method: "GET" } => (HttpStatusCode.OK, DetailedTextPost),
                 { PathAndQuery: "/api/posts/9l5Om_AV", Method: "GET" } => (HttpStatusCode.OK, TextPost),
                 { PathAndQuery: "/api/posts/04f99cf5", Method: "GET" } => (HttpStatusCode.OK, LinkPost),
                 { PathAndQuery: "/api/posts/mTf2fgYj", Method: "GET" } => (HttpStatusCode.OK, ImagePost),
                 { PathAndQuery: "/api/posts", Method: "GET" } => (HttpStatusCode.OK, $"{{\"posts\":{FakeHttpTestFixture.HomePosts},\"next\":1715874445000000000}}" ),
                 { PathAndQuery: "/api/posts?feed=home", Method: "GET" } => (HttpStatusCode.OK, $"{{\"posts\":{FakeHttpTestFixture.HomePosts},\"next\":1715874445000000000}}"),
                 { PathAndQuery: "/api/posts?feed=all&sort=activity", Method: "GET" } => (HttpStatusCode.OK, $"{{\"posts\":{FakeHttpTestFixture.HomePosts},\"next\":1715875936000000000 }}"),
                 { PathAndQuery: "/api/posts?feed=home&sort=hot", Method: "GET" } => (HttpStatusCode.OK, $"{{\"posts\":{FakeHttpTestFixture.HomePosts},\"next\":1715874445000000000}}"),
                 { PathAndQuery: "/api/posts?communityId=17692e122def73f25bd757e0", Method: "GET" } => (HttpStatusCode.OK, $"{{\"posts\":{PostsFeedByCommunity},\"next\":1715874445000000000}}"),
                 { PathAndQuery: "/api/posts?communityId=192ejld4kjdldfd77e0&next=1715874445000000000&limit=1", Method: "GET" } => (HttpStatusCode.OK, $"{{\"posts\":{PostPageTwoByCommunityId},\"next\":17306789341774}}"),
                 { PathAndQuery: "/api/posts?limit=1", Method: "GET" } => (HttpStatusCode.OK, $"{{\"posts\":[{ImagePost}],\"next\":1715874445000000000}}"),
                 { PathAndQuery: "/api/posts?next=17692e122def73f25bd757e0", Method: "GET" } => (HttpStatusCode.OK, $"{{\"posts\":[{ImagePost}],\"next\":1715874445000000000}}"),

                 { PathAndQuery: "/api/posts", Method: "POST" } => (HttpStatusCode.Unauthorized, @"{""status"":401,""code"":""not_logged_in"",""message"":""User is not logged in.""}"),
                 
                 { PathAndQuery: "/api/posts/000000?deleteAs=Normal", Method: "DELETE" } => (HttpStatusCode.Unauthorized, @"{""status"":401,""code"":""not_logged_in"",""message"":""User is not logged in.""}"),
                 { PathAndQuery: "/api/posts/000000?deleteContent=true", Method: "DELETE" } => (HttpStatusCode.Unauthorized, @"{""status"":401,""code"":""not_logged_in"",""message"":""User is not logged in.""}"),
                 { PathAndQuery: "/api/posts/000000?deleteContent=false", Method: "DELETE" } => (HttpStatusCode.Unauthorized, @"{""status"":401,""code"":""not_logged_in"",""message"":""User is not logged in.""}"),
                 { PathAndQuery: "/api/posts/fL0ounHq?deleteAs=Normal", Method: "DELETE" } => (HttpStatusCode.Unauthorized, @"{""status"":401,""code"":""not_logged_in"",""message"":""User is not logged in.""}"),
                 
                 { PathAndQuery: "/api/posts/G7YMijpa/comments?next=2aa28ac7329270fff34f04a", Method: "GET" } => (HttpStatusCode.OK, $"{{ \"comments\": {NextPageDetailedComment}, \"next\": null }}"),
                 { PathAndQuery: "/api/posts/G7YMijpa/comments", Method: "GET" } => (HttpStatusCode.OK, $"{{ \"comments\": [{DetailedComment}], \"next\": \"2aa28ac7329270fff34f04a\" }}"),
                 { PathAndQuery: "/api/posts/MoiuhIkK/comments", Method: "GET" } => (HttpStatusCode.OK, $"{{ \"comments\": [{DeletedComment}], \"next\": \"2aa28ac7329270fff34f04a\" }}"),
                 _ => (HttpStatusCode.NotFound, "")
             };

        (HttpStatusCode, string) checkCredentials(string? username, string? password)
            => (username, password) switch
                {
                    { username:"mmstiver", password:"password123"  } => (HttpStatusCode.OK, FakeHttpTestFixture.User),
                    { username: "CodyRhodes"} => (HttpStatusCode.Forbidden, String.Empty),
                    { username: "RheaRipley", password: not "password123" } => (HttpStatusCode.Unauthorized, String.Empty),
                    _ => (HttpStatusCode.NotFound, userNotFound)
                };


        (HttpStatusCode, string) checkPost(string? type, string? title, string? body, string? community, string? url)
        {
            //if
            //=> (type) switch
            //   {
            //       { username: "mmstiver", password: "password123" } => (HttpStatusCode.OK, FakeHttpTestFixture.User),
            //       { username: "CodyRhodes" } => (HttpStatusCode.Forbidden, String.Empty),
            //       { username: "RheaRipley", password: not "password123" } => (HttpStatusCode.Unauthorized, String.Empty),
            //       _ => (HttpStatusCode.NotFound, userNotFound)
            //   };

            return (HttpStatusCode.OK, "");
        }

        const string userNotFound = @"{""status"":404,""code"":""user_not_found"",""message"":""User not found.""}";

        const string Initial = $$"""
            {
                "reportReasons":
                    [
                        {"id":1,"title":"Breaks community rules","description":null},
                        {"id":2,"title":"Copyright violation","description":null},
                        {"id":3,"title":"Spam","description":null},
                        {"id":4,"title":"Pornography","description":null}
                    ]
                ,"user":null,
                "communities": {{AllCommunities}},
                "noUsers":6111,
                "bannedFrom":null,
                "vapidPublicKey":"BKXheW4qETlYKwBdCnpc8bGGjZnNFCAOIorzalJgee_tocXboYJyqoxhxkswWyAPnu7amWegukc2u5I7z773QtM",
                "mutes":{"communityMutes":[],"userMutes":[]}
            }
            """;

        const string OtherUser = """
            {"id":"17692e04a6576d682930a4f5","username":"previnder","email":null,"emailConfirmedAt":null,"aboutMe":"I'm the developer of this site. (He/Him).\n\nEmail: discuit@previnder.com\n\nTwitter: [@previnderx](https://twitter.com/previnderx)\n\nDiscord: previnder","points":1,"isAdmin":false,"proPic":null,"badges":[],"noPosts":0,"noComments":0,"createdAt":"2023-06-16T15:42:12Z","deleted":false,"deletedAt":null,"upvoteNotificationsOff":false,"replyNotificationsOff":false,"homeFeed":"all","rememberFeedSort":false,"embedsOff":false,"hideUserProfilePictures":false,"bannedAt":null,"isBanned":false,"notificationsNewCount":0,"moddingList":null}
            """;

        const string DeletedComment = """
        {
            "id": "17ce2ef6d1b02d8b15c2722c",
            "postId": "17c68ccf474d60e4745b660c",
            "postPublicId": "MoiuhIkK",
            "communityId": "17b3658bdffc74c79d928aad",
            "communityName": "clowntesting",
            "userId": "000000000000000000000000",
            "username": "[Hidden]",
            "userGroup": "null",
            "userDeleted": false,
            "parentId": null,
            "depth": 0,
            "noReplies": 0,
            "noRepliesDirect": 0,
            "ancestors": null,
            "body": "[Deleted comment]",
            "upvotes": 1,
            "downvotes": 0,
            "createdAt": "2024-05-10T16:55:45Z",
            "editedAt": null,
            "contentStripped": true,
            "deleted": true,
            "deletedAt": "2024-05-10T17:20:16Z",
            "deletedAs": "normal",
            "userVoted": null,
            "userVotedUp": null,
            "postDeleted": false
        }
        """;
    }
}