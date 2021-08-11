namespace ForumSystem.Web.Tests.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using ForumSystem.Data.Models;

    using MyTested.AspNetCore.Mvc;

    public class PostsTestData
    {
        public static List<Post> GetPosts(int count)
        {
            var user = new ApplicationUser
            {
                Id = TestUser.Identifier,
                UserName = TestUser.Username,
            };

            var posts = Enumerable
                    .Range(1, count)
                    .Select(i => new Post
                    {
                        Id = i,
                        Title = $"TestTitle{i}",
                        Content = $"TestContent{i}",
                        Category = new Category
                        {
                            Id = i,
                        },
                    }).ToList();

            return posts;
        }
    }
}
