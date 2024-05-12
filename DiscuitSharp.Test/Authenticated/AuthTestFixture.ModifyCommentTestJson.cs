using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitSharp.Test.Authenticated
{
    public partial class AuthTestFixture
    {
        const string NewComment = """
        {
          "id": "17c93f8221b8ded65ca6dcce",
          "postId": "17c2f0da16acc6f4ed121aa7",
          "postPublicId": "kvzM1JLq",
          "communityId": "17c09453adc68f9d5c932808",
          "communityName": "test",
          "userId": "176fb67a6a2d10402a2b",
          "username": "TestUser",
          "userGroup": "normal",
          "userDeleted": false,
          "depth": 2,
          "noReplies": 0,
          "noRepliesDirect": 0,
          "ancestors": ["17c93f68a36ece261f6ce336"],
          "body": "Newly Created Comment",
          "upvotes": 1,
          "downvotes": 0,
          "createdAt": "2024-04-24T15:02:41Z",
          "editedAt": null,
          "deleted": false,
          "deletedAt": null,
          "author": {"id": "176fb67a6a2d10402a2b", "username": "TestUser"},
          "userVoted": true,
          "userVotedUp": true,
          "postDeleted": false
        }
        """;

        const string NestedComment = """
        {
          "id": "17c93f8221b8ded65ca6dcce",
          "postId": "17c2f0da16acc6f4ed121aa7",
          "postPublicId": "kvzM1JLq",
          "communityId": "17c09453adc68f9d5c932808",
          "communityName": "test",
          "userId": "176fb67a6a2d10402a2b",
          "username": "TestUser",
          "userGroup": "normal",
          "userDeleted": false,
          "parentId": "17c93f6dd54807d149629e5c",
          "depth": 2,
          "noReplies": 0,
          "noRepliesDirect": 0,
          "ancestors": ["17c93f68a36ece261f6ce336", "17c93f6dd54807d149629e5c"],
          "body": "Created Nested Comment",
          "upvotes": 1,
          "downvotes": 0,
          "createdAt": "2024-04-24T15:02:41Z",
          "editedAt": null,
          "deleted": false,
          "deletedAt": null,
          "author": {"id": "176fb67a6a2d10402a2b", "username": "TestUser"},
          "userVoted": true,
          "userVotedUp": true,
          "postDeleted": false
        }
        """;

        const string UpdatedComment = """
                        {
              "id": "17c81cf908cc7edb4f5de499",
              "postId": "17c81a0e3f77cfb3fcea5a11",
              "postPublicId": "kvzM1JLq",
              "body": "Freshly Updated Body",
              "createdAt": "2024-04-24T15:02:41Z",
              "editedAt": "2024-04-20T22:48:34Z",
              "deleted": false,
              "deletedAt": null,
              "userDeleted": false
              }
            """;
        const string UserDeletedComment = """
                        {
              "id": "72547b41d89423318430fdeb",
              "postId": "17c81a0e3f77cfb3fcea5a11",
              "postPublicId": "kvzM1JLq",
              "body": "Currently Deleted Comment",
              "createdAt": "2024-04-20T22:18:34Z",
              "editedAt": null,
              "deleted": true,
              "deletedAt": "2024-04-21T00:10:52Z",
              "userDeleted": true
              }
            """;
        const string DeletedComment = """
                        {
              "id": "72547b41d89423318430fdeb",
              "postId": "17c81a0e3f77cfb3fcea5a11",
              "postPublicId": "W9hs3X0T",
              "body": "Currently Deleted Comment",
              "createdAt": "2024-04-20T22:18:34Z",
              'editedAt': null,
              'deleted': true,
              'deletedAt': "2024-04-21T00:10:52Z",
              'userDeleted': false
              }
            """;


    }
}
