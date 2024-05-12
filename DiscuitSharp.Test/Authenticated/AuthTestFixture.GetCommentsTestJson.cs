using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitSharp.Test.Authenticated
{
    public partial class AuthTestFixture
    {
       

        const string GetComments = """
            {
                "id": "0d2a1013aa8d757edd37cef9",
                "type": "text",
                "publicId": "A3c39F9",
                "comments":[
                    {
                        "id":"6d26b73a3d93e52479a2d952",
                        "body":"Test Comment 1!!",
                        "postId":"17c2f0da16acc6f4ed121aa7",
                        "postPublicId":"icFQXwoe",
                        "communityId":"17c09453adc68f9d5c932808",
                        "communityName":"dotnet",
                        "userId":"176fb67a0c266a2d10402a2b",
                        "username":"asyncrosaurus",
                        "userGroup":"normal",
                        "userDeleted":false,
                        "parentId":null,
                        "depth":0,
                        "noReplies":0,
                        "noRepliesDirect":0,
                        "ancestors":null,
                        "upvotes":1,
                        "downvotes":0,
                        "createdAt":"2024-04-24T15:02:41Z",
                        "editedAt":null,
                        "deleted":false,
                        "deletedAt":null,
                        "author":{"id":"176fb67a0c266a2d10402a2b","username":"asyncrosaurus"},
                        "userVoted":true,
                        "userVotedUp":true,
                        "postDeleted":false
                    },
                    {
                       "id":"d1801e698d68cd3e2b74bbe8",
                        "body":"Test Comment 2!!",
                        "ancestors":null,
                        "parentId":null   ,
                        "parentId":null,
                        "depth":0,
                        "noReplies":0,
                        "noRepliesDirect":0,
                        "ancestors":null,
                        "upvotes":1,
                        "downvotes":0,
                        "postDeleted":false
                    },
                    {
                        "id":"ecf5cd18ec2ffc582e8ca614",
                        "body":"Test Comment 3!!",
                        "ancestors":null,
                        "parentId":null        ,
                        "parentId":null,
                        "depth":0,
                        "noReplies":0,
                        "noRepliesDirect":0,
                        "ancestors":null,
                        "upvotes":1,
                        "downvotes":0,
                        "postDeleted":false
                    }
                ]
            }
            """;


        const string NestedCommentsWithAncestors = """
          [{
          "id":"17ca8aefc3c16df691edade5",
          "postId":"17c2f0da16acc6f4ed121aa7",
          "postPublicId":"icFQXwoe",
          "ancestors":["17c93f68a36ece261f6ce336","17c93f6dd54807d149629e5c"],
          "communityId":"17c09453adc68f9d5c932808",
          "communityName":"test"          
          },
         {
         "id":"17caae386d17fb4fc8d3c793",
         "postId":"17c2f0da16acc6f4ed121aa7",
         "parentId":null,
         "ancestors":null
         },
         {
         "id":"17caadf7b6b0f3b15e9123f4",
         "postId":"17c2f0da16acc6f4ed121aa7",
         "parentId":null,
         "ancestors":["17caae386d17fb4fc8d3c793"]
         },
         {
         "id":"0a6dc6d94e4c5ee2e024f33a",
         "postId":"17c2f0da16acc6f4ed121aa7",
         "parentId":"17ca8aefc3c16df691edade5",  
         "ancestors":["17ca8aefc3c16df691edade5"]
         },
         {
         "id":"0a6dc6d94e4c5ee2e024f33a",
         "postId":"17c2f0da16acc6f4ed121aa7",
         "parentId":"17ca8aefc3c16df691edade5",  
         "ancestors":["17ca8aefc3c16df691edade5","0a6dc6d94e4c5ee2e024f33a"]
         }
         
         ]
         """;
    }
}
