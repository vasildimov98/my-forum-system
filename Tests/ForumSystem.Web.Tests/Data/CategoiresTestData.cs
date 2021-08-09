namespace ForumSystem.Web.Tests.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using ForumSystem.Data.Models;

    using MyTested.AspNetCore.Mvc;

    public static class CategoiresTestData
    {
        public static List<Category> GetCategories(int count)
        {
            var user = new ApplicationUser
            {
                Id = TestUser.Identifier,
                UserName = TestUser.Username,
            };

            var categories = Enumerable
                   .Range(1, count)
                   .Select(i => new Category
                   {
                       Id = i,
                       Name = $"TestedName{i}",
                       Description = $"TestDescription{i}",
                       ImageUrl = "TestImageURl",
                       Owner = user,
                       Messages = Enumerable
                            .Range(1, 5)
                            .Select(j => new Message
                            {
                                Id = j,
                                User = user,
                                Content = $"TestContent{j}",
                            }).ToList(),
                   }).ToList();

            return categories;
        }
    }
}
