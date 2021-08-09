namespace ForumSystem.Web.Tests.Mock
{
    using System.Security.Claims;

    using ForumSystem.Data.Models;

    using Microsoft.AspNetCore.Identity;
    using Moq;
    using MyTested.AspNetCore.Mvc;

    public class UserManagerMock
    {
        public static UserManager<ApplicationUser> Create
        {
            get
            {
                var store = new Mock<IUserStore<ApplicationUser>>();
                var mgr = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
                mgr.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
                mgr.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());

                mgr.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new ApplicationUser
                {
                    Id = TestUser.Identifier,
                    UserName = TestUser.Username,
                });
                mgr.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser
                {
                    Id = TestUser.Identifier,
                    UserName = TestUser.Username,
                });

                return mgr.Object;
            }
        }
    }
}
