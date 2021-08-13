namespace ForumSystem.Web.Tests.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ForumSystem.Data.Models;

    using MyTested.AspNetCore.Mvc;

    public static class CategoiresTestData
    {
        public static List<Category> GetCategories(int count, bool isApproved = true, bool isDiffUser = false)
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
                       Owner = isDiffUser ? new ApplicationUser 
                       {
                           Id = $"DiffTestUserId{i}",
                           UserName = $"DiffTestUsername{i}",
                       }
                       : user,
                       Messages = messages,
                       IsApprovedByAdmin = isApproved,
                   }).ToList();

            return categories;
        }

        public static List<Category> GetMixedCategories(int total, int approved)
        {
            var categories = Enumerable
                   .Range(1, total)
                   .Select(i => new Category
                   {
                       Id = i,
                       Name = $"TestName{i}",
                       Description = $"TestDescription{i}",
                       ImageUrl = "TestImageURl",
                       IsApprovedByAdmin = i <= approved,
                       OwnerId = TestUser.Identifier,
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
