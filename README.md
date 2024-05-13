
# DiscuitSharp
Discuit Sharp (D#) is a .NET Standard library that provides easy access to the Discuit API with no low-level network or HTTP config required.

[![Buy me a coffee](https://img.shields.io/badge/Buy%20Me%20A%20Coffee-donate-yellow.svg)](https://www.buymeacoffee.com/mmstiver)
[![PayPal Donate](https://img.shields.io/badge/Donate-PayPal-blue)](https://paypal.me/mmstiver?country.x=CA&locale.x=en_US)
[![License: LGPL v3](https://img.shields.io/badge/License-LGPL%20v3-blue.svg)](https://www.gnu.org/licenses/lgpl-3.0)

## Overview


## Installation

Add the following package to your project using NuGet:

`dotnet add package DiscuitSharp`

Before you can use DiscuitClient, you need to set up an HttpClient with a cookie storage container, and initialize the session:

```
var handler = new HttpClientHandler() { CookieContainer = new CookieContainer() };
var httpClient = new HttpClient(handler)
{
    BaseAddress = new Uri("https://discuit.net/api/")
};
var discClient = new DiscuitClient(httpClient);
_ = discClient.GetInitial();
```

**Note:** It is important to manage the Discuit client as you would an HttpClient, since it is a wrapper. Otherwise, you may be at risk for socket exhaustion and DNS changes not being respected. Reuse the Discuit Client Instances and configure with an HttpClientFactory.


## Usage
### User Authentication and Management

Log in a user using their credentials, necessary for any actions that require an authenticated DiscuitUser context.

```
var credentials = new Credentials("mmstiver", "password123");
var user = await client.Authenticate(credentials);
```

Invalidate the current user session, effectively logging the user out of the application.

`await client.InvalidateAuth();`

**Additional Actions**:

-GetAuthenticatedUser(): Retrieves details about the currently authenticated user.
-GetUser(string UserName): Fetches information about a specific user by their username, useful for viewing profiles.

## Communities

Search for communities with a query string or Set (` All|Default|Subscribed`), Re

```
var communitiesByName = await client.GetCommunities("Discuit");
var communities = await client.GetCommunities(QueryParams.Subscribed);
```

Or get a Community by Id

```
//Id for 'general'
CommunityId Id = new CommunityId("176ef2e09e28701c14c0c148");
Community? community = await client.Get(Id);
```

## Posts 

### Fetching Posts with Specific Feed and Sorting

Fetch posts from a specific feed, such as 'Featured' or 'New', and apply sorting criteria like 'Recent' or 'Popular'..

```
var posts = await client.GetPosts(feed: Feed.Featured, sort: Sort.Recent, limit: 10);

foreach (var post in posts.Items)
{
    Console.WriteLine($"{post.Title} - {post.Content}");
}
```

### Fetching Posts with Pagination

To fetch posts in a paginated manner, you can pass a `CursorIndex` to the `GetPosts` method. This cursor typically represents a pointer to a specific place in the list of posts, allowing you to retrieve the next set of posts from that point. Here is how you can use it:

```
// Assuming 'lastCursor' is retrieved from the previous fetch operation
var lastCursor = new CursorIndex("cursor_value_here");
var nextPosts = await client.GetPosts(cursor: lastCursor);

foreach (var post in nextPosts.Items)
{
    Console.WriteLine($"{post.Title} - {post.Content}");
}
```

### Fetching Posts by Id


```
// posts for general
CommunityId Id = new CommunityId("176ef2e09e28701c14c0c148");
var post = await GetPosts(Id);
```

Create, Update and Delete accept a Post model (TextPost | LinkPost | ImagePost). Changes to a post does not have to be tracked, as each model will track it's own changes. Any modifications will be persisted when the model is passed into the Update.

```
TextPost newPost = new("First Post", "general", "Hi Mom!");
newPost = await client.Create(newPost);
newPost!.Title = "updated title"
newPost = await client.Update(newPost);
newPost = await client.Delete(newPost, deleteContent: true);
```


## Comments 

Comment has the same actions as Posts:

```
PublicId publicId = new PublicPostId("d3viYYpm")
CommentId parentId = new CommentId("54135hfg43")
newComment =  await client.Create(publicId, parentId, newComment);
newComment!.Body = "updated Comment"
newComment = await client.Update(publicId, newComment.Id);
_ = await client.Delete(publicId, newComment.Id);
```


   
