using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitSharp.Test.Unauthenticated
{
    public partial class NoAuthTestFixture
    {
        protected const string DetailedCommunity = """
                 {
                  "id": "17692e122def73f25bd757e0",
                  "userId": "17692e04a6576d682930a4f5",
                  "name": "general",
                  "nsfw": false,
                  "about": "General chat community. For everything that doesn't belong in other communities.",
                  "noMembers": 5990,
                  "proPic": {
                    "id": "17c0becfc4aacacd57e276b5",
                    "format": "jpeg",
                    "mimetype":"image/jpeg",
                    "url": "/images/17c0becfc4aacacd57e276b5.jpeg?sig=_Oj5E6isIecCt_9eQ-3asAi1JlDB6dBfd6bzlLTTAL8"
                  },
                  "bannerImage": {
                    "id": "17c0c06dba3d07cdde20d30b",
                    "format": "jpeg",  
                    "mimetype":"image/jpeg",
                    "url": "/images/17c0c06dba3d07cdde20d30b.jpeg?sig=g8j5VXaVXu5LFe_HZHESIbmeY7BAionS8YrohJKn284"
                  },
                  "createdAt": "2024-03-27T09:15:45Z",
                  "deletedAt": null,
                  "isDefault": false,
                  "userJoined": true,
                  "userMod": true,
                  "isMuted": false,
                  "mods": [
                    {
                        "id": "17692e04a6576d682930a4f5",
                        "username": "previnder"
                    }
                  ],
                  "rules": [
                    {
                        "id": 35,
                        "rule": "No bigotry",
                        "description": "",
                        "communityId": "17692e122def73f25bd757e0",
                        "createdBy": "176c98ce3fb02f05a7f3a24d",
                        "createdAt": "2023-06-30T18:05:59Z"
                    }
                  ],
                  "ReportsDetails": null
                }
                """;

        protected const string AskDiscuit =
           """
            {
               "id": "176abdabbb1c2f5b97786d26",
                "userId": "17692e04a6576d682930a4f5",
                "name": "AskDiscuit",
                "nsfw": false,
                "about": "Ask and answer thought-provoking and fun questions for entertainment."
            }
            """;

        protected const string Discuit =
            """
            {
                "id": "176ef2e09e28701c14c0c148",
                "userId": "17692e04a6576d682930a4f5",
                "name": "Discuit",
                "nsfw": false,
                "about": "The most official Discuit community of all official Discuit communities. Your go-to place for Discuit updates, announcements, and news.\n\n[Click here to see the Discuit Roadmap](https://discuit.net/Discuit/post/9l5Om_AV)"
            }
            """;

        protected const string Technology =
            """
            {
                "id": "176abd743c6f7fa625da48d8",
                "userId": "17692e04a6576d682930a4f5",
                "name": "technology",
                "nsfw": false,
                "about": "this is a place to share and discuss the latest developments, happenings and curiosities in the world of technology"
            }
            """;

        protected const string Funny =
            """
         {
            "id": "176abc4a51bad7a2313961e3",
            "userId": "17692e04a6576d682930a4f5",
            "name": "funny",
            "nsfw": false,
            "about": "Jokes, funny pictures and videos you find on the internet that are not memes share them here."
         }
         """;

        protected const string General = """
            {
                    "id": "17692e122def73f25bd757e0",
                    "userId": "17692e04a6576d682930a4f5",
                    "name": "general",
                    "about": "General chat community. For everything that doesn't belong in other communities."
                }
            """;

        protected const string Canada = """
                    {
                        "id": "176ee861085b94ae1af2569c",
                        "userId": "17692e04a6576d682930a4f5",
                        "name": "Canada",
                        "about": "News, events, and general discussions about or involving Canada"
                    }
                    """;

        protected const string DiscuitDev = """

                 {
                    "id": "17b41605558f441a4ca05ffb",
                    "userId": "176c98ce3fb02f05a7f3a24d",
                    "name": "DiscuitDev",
                    "about": "A community built around the open-source development of discuit.net. Documentation of the API is available at [docs.discuit.net](https://docs.discuit.net)."
                }
                """;

        protected const string Selfposting = """
                {
                    "id": "177a1c7b8456269646d3c5ba",
                    "userId": "17692e04a6576d682930a4f5",
                    "name": "Selfposting",
                    "about": "Welcome to Selfposting! Post about your thoughts, your likes, your fears, or anything you like. Selfies, achievements, failures, or anything about yourself you don't think will fit in another disc. Enjoy your stay!"
                }
                """;

        protected const string LGBTQ = """
                                     {
                                         "id": "176e6ff2ea3b3dcaac6987da",
                                         "userId": "17692e04a6576d682930a4f5",
                                         "name": "LGBTQ",
                                         "nsfw": false,
                                         "about": "A safe space for anyone in the LGBTQ+ community to discuss their experiences."
                                     }
                                    """;

        protected const string DefaultCommunities = "[" + AskDiscuit + "," + Discuit + "," + Technology + "]";
        protected const string SubscribedCommunities = "[" + Funny + "," + General + "," + DiscuitDev + "]";
        protected const string OtherCommunities = "[" + Canada + "," + Selfposting + "," + LGBTQ + "]";
        protected const string AllCommunities = "[" + Canada + "," + Selfposting + "," + LGBTQ +"," + AskDiscuit 
            + "," + Discuit + "," + Technology + "," + Funny + "," + General + "," + DiscuitDev + "]";

    }
}