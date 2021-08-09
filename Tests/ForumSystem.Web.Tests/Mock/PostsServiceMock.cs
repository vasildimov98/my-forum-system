namespace ForumSystem.Web.Tests.Mock
{
    using ForumSystem.Data;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Comments;
    using ForumSystem.Web.ViewModels.Posts;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;
    using Moq;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PostsServiceMock
    {
        public static IPostsService Create
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                     .UseInMemoryDatabase(Guid.NewGuid().ToString())
                     .Options;

                var mock = new Mock<IPostsService> { CallBase = true };

                mock.Setup(x => x
                    .GetById<PostViewModel>(1))
                    .Returns(new PostViewModel
                    {
                        Id = 1,
                        UserUserName = TestUser.Username,
                        Comments = Enumerable.Empty<PostCommentViewModel>(),
                    });

                return mock.Object;
            }
        }
    }
}
