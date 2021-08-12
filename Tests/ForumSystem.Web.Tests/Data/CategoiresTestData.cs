namespace ForumSystem.Web.Tests.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using ForumSystem.Data.Models;

    using MyTested.AspNetCore.Mvc;

    public static class CategoiresTestData
    {
        public static List<Category> GetApprovedCategories(int count)
        {
            var user = new ApplicationUser
            {
                Id = TestUser.Identifier,
                UserName = TestUser.Username,
            };

            var messages = Enumerable
                            .Range(1, 5)
                            .Select(j => new Message
                            {
                                Id = j,
                                User = user,
                                Content = $"TestContent{j}",
                            }).ToList();

            var categories = Enumerable
                   .Range(1, count)
                   .Select(i => new Category
                   {
                       Id = i,
                       Name = $"TestName{i}",
                       Description = $"TestDescription{i}",
                       ImageUrl = "TestImageURl",
                       Owner = user,
                       Messages = messages,
                       IsApprovedByAdmin = true,
                   }).ToList();

            return categories;
        }

        public static List<Category> GetFamousCategories(int count = 10)
        {
            var categories = Enumerable
                .Range(1, count)
                .Select(index =>
                {
                    var posts = new List<Post>();

                    for (int i = count; i < index; i++)
                    {
                        posts.Add(new Post { Id = (i + index) * index });
                    }

                    var category = new Category
                    {
                        Id = index,
                        Name = $"TestName{index}",
                        ImageUrl = $"TestImage{index}",
                        Posts = posts,
                        IsApprovedByAdmin = true,
                    };

                    return category;
                }).ToList();

            return categories;
        }
    }
}
