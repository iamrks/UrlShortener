using Swashbuckle.AspNetCore.Filters;
using UrlShortener.Models;

namespace UrlShortener.SwaggerResponse
{
    public class GithubUserResponse : IExamplesProvider<GithubUser>
    {
        public GithubUser GetExamples()
        {
            return new GithubUser
            {
                Login = "iamrks",
                Id = 10129062,
                NodeId = "MDQ6VXNlcjEwMTI5MDYy",
                AvatarUrl = "https://avatars.githubusercontent.com/u/10129062?v=4",
                GravatarId = "",
                Url = "https://api.github.com/users/iamrks",
                HtmlUrl = "https://github.com/iamrks",
                FollowersUrl = "https://api.github.com/users/iamrks/followers",
                FollowingUrl = "https://api.github.com/users/iamrks/following{/other_user}",
                GistsUrl = "https://api.github.com/users/iamrks/gists{/gist_id}",
                StarredUrl = "https://api.github.com/users/iamrks/starred{/owner}{/repo}",
                SubscriptionsUrl = "https://api.github.com/users/iamrks/subscriptions",
                OrganizationsUrl = "https://api.github.com/users/iamrks/orgs",
                ReposUrl = "https://api.github.com/users/iamrks/repos",
                EventsUrl = "https://api.github.com/users/iamrks/events{/privacy}",
                ReceivedEventsUrl = "https://api.github.com/users/iamrks/received_events",
                Type = "User",
                SiteAdmin = false,
                Name = "Ravi Kant Sharma",
                Company = "Careerbuilder",
                Blog = "https://stackoverflow.com/users/9379576/ravi",
                Location = "New Delhi, India",
                Email = "iamrks@outlook.com",
                Hireable = true,
                Bio = "\r\n    \r\n    Full Stack developer (.Net Core, Java Script, Angular, TypeScript).\r\n\r\n",
                TwitterUsername = null,
                PublicRepos = 15,
                PublicGists = 27,
                Followers = 8,
                Following = 4,
                CreatedAt = DateTime.Parse("2014-12-09T12:05:10Z").ToString(),
                UpdatedAt = DateTime.Parse("2024-10-21T11:25:16Z").ToString()
            };
        }
    }
}
