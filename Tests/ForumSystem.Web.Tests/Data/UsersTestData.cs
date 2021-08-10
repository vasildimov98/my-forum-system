namespace ForumSystem.Web.Tests.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using ForumSystem.Data.Models;
    using MyTested.AspNetCore.Mvc;

    public class UsersTestData
    {
        public static List<ApplicationUser> GetUsers(int count)
        {
            var users = Enumerable
                    .Range(1, count)
                    .Select(i => new ApplicationUser
                    {
                        Id = $"TestUserId{i}",
                        UserName = $"TestUserName{i}",
                        ProfileImage = new ProfileImage
                        {
                            Id = $"TestProfileImage{i}",
                            Extention = "png",
                        },
                    }).ToList();

            users.Add(new ApplicationUser
            {
                Id = TestUser.Identifier,
                UserName = TestUser.Username,
            });

            return users;
        }
    }
}
