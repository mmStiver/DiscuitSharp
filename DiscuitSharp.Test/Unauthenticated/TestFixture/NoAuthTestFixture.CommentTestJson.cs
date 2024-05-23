using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitSharp.Test.Unauthenticated
{
    public partial class NoAuthTestFixture
    {
        const string NextPageDetailedComment = """
          [{
          "id":"0a6dc6d94e4c5ee2e024f33a",
          "postId":"17c2f0da16acc6f4ed121aa7",
          "postPublicId":"icFQXwoe",
          "ancestors":["17c93f68a36ece261f6ce336","17c93f6dd54807d149629e5c"],
          "communityId":"17c09453adc68f9d5c932808",
          "communityName":"test"     ,
          "body":"nextpage Comment 1"
          },
         {
         "id":"h9d9v0hgf445ff62dd2445",
         "postId":"17c2f0da16acc6f4ed121aa7",
         "postPublicId":"icFQXwoe",  
         "ancestors":["17c93f68a36ece261f6ce336","17c93f6dd54807d149629e5c"],
         "communityId":"17c09453adc68f9d5c932808",
         "communityName":"test",
         "body":"nextpage Comment 2"         
         }]
         """;

        const string DetailedComment = """
          {
          "id":"17c93f8221b8ded65ca6dcce",
          "postId":"17c2f0da16acc6f4ed121aa7",
          "postPublicId":"icFQXwoe",
          "communityId":"17c09453adc68f9d5c932808",
          "communityName":"test",
          "userId":"176fb67a6a2d10402a2b",
          "username":"TestUser",
          "userGroup":"normal",
          "userDeleted":false,
          "parentId":"17c93f6dd54807d149629e5c",
          "depth":2,
          "noReplies":0,
          "noRepliesDirect":0,
          "ancestors":["17c93f68a36ece261f6ce336","17c93f6dd54807d149629e5c"],
          "body":"Third!",
          "upvotes":1,
          "downvotes":0,
          "createdAt":"2024-04-24T15:02:41Z",
          "editedAt":null,
          "deleted":false,
          "deletedAt":null,
          "author":{"id":"176fb67a6a2d10402a2b","username":"TestUser"},
          "userVoted":true,
          "userVotedUp":true,
          "postDeleted":false
          }
         """;
    }
}