using DiscuitSharp.Core;
using DiscuitSharp.Test.Unauthenticated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DiscuitSharp.Test
{
    public abstract class FakeHttpTestFixture
    {
        protected IHttpClientFactory httpClientFactory;
        public FakeHttpTestFixture()
        {
            httpClientFactory = new FakeHttpClientFactory(this.Content);
        }
        public IDiscuitClient CreateClient()
        {
            var client = new DiscuitClient(httpClientFactory);

            return client;
        }
        public async Task<IDiscuitClient> InitializeClient()
        {
            var client = CreateClient();
            _ = await client.GetInitial();
            return client;
        }

        protected abstract (HttpStatusCode, string) Content(Uri? url, string method, JsonObject? json);

        protected const string User = """
            {
              "id": "17b264a01e464aba89939bf7",
              "username": "mmstiver"
            }
            """;
        
        protected const string Ask = """
            
              {
                "id": "176abdabbb1c2f5b97786d26",
                "userId": "17692e04a6576d682930a4f5",
                "name": "AskDiscuit",
                "nsfw": false,
                "about": "Ask and answer thought-provoking and fun questions for entertainment.",
                "noMembers": 5825,
                "proPic": null,
                "bannerImage": null,
                "createdAt": "2023-06-21T17:45:55Z",
                "deletedAt": null,
                "userJoined": null,
                "userMod": null,
                "isMuted": false,
                "mods": null,
                "rules": null,
                "ReportsDetails": null
              },
              {
                "id": "17b3672cbe7950704285731a",
                "userId": "17692e04a6576d682930a4f5",
                "name": "ShittyAskDiscuit",
                "nsfw": false,
                "about": null,
                "noMembers": 10,
                "proPic": null,
                "bannerImage": null,
                
                "createdAt": "2024-02-13T11:02:05Z",
                "deletedAt": null,
                "userJoined": null,
                "userMod": null,
                "isMuted": false,
                "mods": null,
                "rules": null,
                "ReportsDetails": null
              }
            
            """;

        protected const string HomePosts =
            """
                [{
                  "id": "a6a945f8-fac7-441d-a170-937870d03605",
                  "type": "text",
                  "publicId": "P3T4GERf",
                  "title": "Home Title 1",
                  "createdAt": "2024-03-07T01:06:18Z",
                  "lastActivityAt": "2024-03-07T07:06:18Z"
                },
                {
                  "id": "d612b7c1-16fd-4a7d-a7c1-665d2871af21",
                  "type": "text",
                  "publicId": "9l5Om_AV",
                  "title": "Home Title 2",
                  "createdAt": "2023-09-29T08:06:18Z",
                  "lastActivityAt": "2023-09-29T12:06:18Z"
                }
                ]
             """;

        protected const string CommunitiesPosts = """
                        [
              [
                {
                  "id": "f436996e-08e6-44e8-8b7f-6619c62b29c0",
                  "type": "text",
                  "publicId": "60cd1a7c",
                  "title": "Community Title 1",
                  "createdAt": "2024-02-14T17:05:03Z",
                  "lastActivityAt": "2024-07-15T17:05:03Z"
                },
                {
                  "id": "3db0ffbf-84ba-4ad8-857c-e2a705a766ce",
                  "type": "text",
                  "publicId": "47b3be91",
                  "title": "Community Title 2",
                  "createdAt": "2023-04-19T17:05:03Z",
                  "lastActivityAt": "2025-01-02T17:05:03Z"
                }
              ]
              """;

      

        
    }
}
