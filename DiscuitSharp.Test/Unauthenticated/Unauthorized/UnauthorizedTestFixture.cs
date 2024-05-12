using DiscuitDotNet.Core;
using DiscuitDotNet.Test.Unauthenticated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DiscuitDotNet.Test.Unauthenticated.Unauthorized
{
    public class UnauthorizedTestFixture
    {
        IHttpClientFactory httpClientFactory;
        public UnauthorizedTestFixture()
        {
            httpClientFactory = new FakeHttpClientFactory(this.Content);
        }

        public IDiscuitClient CreateClient()
           => new DiscuitClient(httpClientFactory);

        (HttpStatusCode, string) Content(Uri? url, JsonObject? obj)
            => url.Segments switch
            {
                [_, _, "_initial"] => (HttpStatusCode.Unauthorized, Initial),
                [_, _, "_login"] => (HttpStatusCode.Unauthorized, User),
                [_, _, "_login", "?", "logout"] => (HttpStatusCode.Unauthorized, ""),
                _ => (HttpStatusCode.Unauthorized, "")
            };

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

        readonly string User = String.Empty;
    }
}
