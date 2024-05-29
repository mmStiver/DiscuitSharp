using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitSharp.Test.Unauthenticated
{
    public partial class NoAuthTestFixture
    {
        protected const string TextPost = """
            {
              "id": "ba7b4dad8e80bad97bb7",
              "type": "text",
              "publicId": "777e7240",
              "userId": "87cceac9-eba3-4460-837b-643298da552d",
              "title": "Random Title 65",
              "body": "This is a test"
            }
            """;

        protected const string LinkPost = """
            {
                 "id": "17c35c1fae47c7c7ebd18d08",
                 "type": "link",
                 "publicId": "04f99cf5",
                 "userId": "176a093b2ec0e5ebcbead8d0",
                 "title": "Title for a Link Post",
                 "image": null,
                 "link": {
                   "url": "https://www.example.ca/blog/post",
                   "hostname": "www.example.ca"
                   }
                }
            """;

        protected const string ImagePost = """
        { 
            "id": "17c3614a1bbb7509626b5e8c",
            "type": "image",
            "publicId": "mTf2fgYj",
            "title": "NK Mods renewing their contracts",
            "image": {
                 "id": "17c36146d6ba09fd32c01816",
                 "format": "jpeg",
                 "mimetype": "image/jpeg",
                 "width": 546,
                 "height": 831,
                 "size": 64702,
                 "averageColor": "rgb(84,83,86)",
                 "url": "/images/17c36146d6ba09fd32c01816.jpeg?sig=MWMwJRDXGfPfirNMFw_3fQYNJmtRwSf2SwB9dj67cgE"
             }
        }
        """;

        protected const string DetailedTextPost = """
            {
              "id": "ba7b4dad8e80bad97bb7",
              "type": "text",
              "publicId": "777e7240",
              "userId": "87cceeba608643298da552d",
              "username": "TestAuthor",
              "title": "Random Title 65",
              "body": "This is a test",
              "communityId": "17b45e9e85367e0a16ac9119",
              "communityName": "CrazyVideos",
              "author" : {
                "id":"87cceeba608643298da552d",
                "username": "TestAuthor"
              },
              "isPinned": false,
              "isPinnedSite": false,
              "title": "Noir",
              "body": null,
              "locked": false,
              "lockedBy": null,
              "lockedAt": null,
              "upvotes": 46,
              "downvotes": 0,
              "hotness": 380179868967,
              "createdAt": "2024-03-17T20:04:24Z",
              "editedAt": null,
              "lastActivityAt": "2024-03-17T20:18:13Z",
              "deleted": false,
              "deletedAt": null,
              "deletedContent": false,
              "noComments": 1,
              "comments": [
                {
                  "id": "17bda6ac79023e752bf42053",
                  "postId": "ba7b4dad8e80bad97bb7",
                  "postPublicId": "777e7240",
                  "communityId": "17b45e9e85367e0a16ac9119",
                  "communityName": "CrazyVideos",
                  "userId": "1781b7443a2ffc18b5cbedf4",
                  "username": "Hardpawns",
                  "userGroup": "normal",
                  "userDeleted": false,
                  "parentId": null,
                  "depth": 0,
                  "noReplies": 0,
                  "noRepliesDirect": 0,
                  "ancestors": null,
                  "body": "Comment Content Body Text",
                  "upvotes": 10,
                  "downvotes": 0,
                  "createdAt": "2024-03-17T20:18:13Z",
                  "editedAt": "2024-03-17T20:18:25Z",
                  "deleted": false,
                  "deletedAt": null,
                  "author": {
                    "id": "1781b7443a2ffc18b5cbedf4",
                    "username": "testCommentor"
                  },
                  "userVoted": false,
                  "userVotedUp": null,
                  "postDeleted": false
                }
              ],
              "commentsNext": null,
              "userVoted": false,
              "userVotedUp": null,
              "isAuthorMuted": false,
              "isCommunityMuted": false
            }
            """;

        protected const string PostsFeedByCommunity = """
            [
              {
                "id": "17a2ae34de8303dea3c54a72",
                "type": "image",
                "publicId": "wGXYeKnl",
                "communityId": "17692e122def73f25bd757e0",
                "communityName": "general",
                "title": "Title for an art Post",
                "community":{
                    "id": "17692e122def73f25bd757e0",
                    "name": "general"
                },
                "image": {
                  "id": "17a369be13ff8ac23bb8df3e",
                  "format": "jpeg",
                  "url": "/images/17a369be13ff8ac23bb8df3e.jpeg?sig=sIH6sk0WJDrsOkqcajQNoOk4ZwHZGWu0M_Aoy07e84I"
                }
            },
            {
                "id": "17977138c89990dab7682cfd",
                "type": "text",
                "publicId": "AkxbHnol",
                "community":{
                    "id": "17692e122def73f25bd757e0",
                    "name": "general"
                },
                "title": "Title for a text Post",
                "body": "Test date"
            },
            {
                 "id": "17c35c1fae47c7c7ebd18d08",
                 "type": "link",
                 "publicId": "04f99cf5",
                  "community":{
                    "id": "17692e122def73f25bd757e0",
                    "name": "general"
                    },
                 "title": "Title for a Link Post",
                 "link": {
                   "url": "https://www.example.ca/blog/post",
                   "hostname": "www.example.ca"
                  }
             }
            ]
            """;

        protected const string PostPageTwoByCommunityId = """
            [
              {
                "id": "177aecc7e4f97bf1381b4d42177aecc7e4f97bf1381b4d42",
                "type": "image",
                "publicId": "wGXYeKnl",
                "communityId": "192ejld4kjdldfd77e0",
                "communityName": "general",
                "title": "Title for an art Post",
                "community":{
                    "id": "192ejld4kjdldfd77e0",
                    "name": "general"
                }
            }
            ]
        """;
    }
}