namespace ForumSystem.Web.Tests.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using ForumSystem.Data.Models;
    using MyTested.AspNetCore.Mvc;

    public class CommentsTestData
    {
        public static List<Comment> GetComments(int count)
        {
            var postWriter = new ApplicationUser
            {
                Id = "Pesho",
                UserName = "Pesho98",
            };

            var user = new ApplicationUser
            {
                Id = TestUser.Identifier,
                UserName = TestUser.Username,
            };

            var comments = Enumerable
                   .Range(1, count)
                   .Select(i => new Comment
                   {
                       Id = i,
                       Content = $"TestContent{i}",
                       Post = new Post
                       {
                           Id = i,
                           Title = $"TestTitle{i}",
                           User = postWriter,
                           Content = $"TestPostContent{i}",
                           Category = new Category
                           {
                               Id = i,
                               Name = "TestCategoryName",
                               Owner = postWriter,
                               ImageUrl = "testImage",
                               Description = "TestDescription",
                           },
                       },
                       User = user,
                   }).ToList();

            return comments;
        }
    }
}
