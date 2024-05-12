using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitSharp.Test.Authenticated
{
    public partial class AuthTestFixture
    {
        const string ReferenceUnvotedPost = """
                        {
              "id": "17c68bb8829688ba13844398",
              "type": "text",
              "publicId": "Rg8TkGaE",
              "upvotes": 50,
              "downvotes": 0,
              "userVoted": null,
              "userVotedUp": null
            }
            """;
        const string UpvotedPost = """
                        {
              "id": "17c68bb8829688ba13844398",
              "type": "text",
              "publicId": "Rg8TkGaE",
              "upvotes": 51,
              "downvotes": 0,
              "userVoted": true,
              "userVotedUp": true
            }
            """;
        const string DownvotedPost = """
                        {
              "id": "17c68bb8829688ba13844398",
              "type": "text",
              "publicId": "Rg8TkGaE",
              "upvotes": 50,
              "downvotes": 1,
              "userVoted": true,
              "userVotedUp": false
            }
            """;

        const string ReferenceUnvotedComment = """
                        {
              "id": "17c68bb8829688ba13844398",
              "upvotes": 50,
              "downvotes": 0,
              "userVoted": null,
              "userVotedUp": null
            }
            """;
        const string UpvotedComment = """
                        {
              "id": "17c68bb8829688ba13844398",
               "upvotes": 51,
              "downvotes": 0,
              "userVoted": true,
              "userVotedUp": true
            }
            """;
        const string DownvotedComment = """
                        {
              "id": "17c68bb8829688ba13844398",
             "upvotes": 50,
              "downvotes": 1,
              "userVoted": true,
              "userVotedUp": false
            }
            """;
    }
}
