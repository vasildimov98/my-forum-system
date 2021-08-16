namespace ForumSystem.Web.Tests.Controllers
{
    using System.Linq;

    using ForumSystem.Data.Models;
    using ForumSystem.Web.Areas.Administration.Controllers;
    using ForumSystem.Web.Controllers;
    using ForumSystem.Web.ViewModels.Categories;

    using MyTested.AspNetCore.Mvc;
    using Shouldly;
    using Xunit;

    using static ForumSystem.Common.GlobalConstants;
    using static ForumSystem.Web.Tests.Data.CategoiresTestData;
    using static ForumSystem.Web.Tests.Data.UsersTestData;

    public class CategoriesControllerTests
    {
        [Theory]
        [InlineData(10, 1, null, 2)]
        public void GetAllShouldReturnCorrectViewModel(
            int categoriesCount,
            int page,
            string searchTerm,
            int totalPages)
            => MyController<CategoriesController>
                .Instance(instance => instance
                    .WithData(GetCategories(12)))
                .Calling(c => c.All(page, searchTerm))
                .ShouldHave()
                .ActionAttributes(attr => attr
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CateogoriesListModel>()
                    .Passing(categoriesViewModel =>
                    {
                        categoriesViewModel.Categories.Count().ShouldBe(categoriesCount);
                        categoriesViewModel.PaginationModel.CurrentPage.ShouldBe(page);
                        categoriesViewModel.PaginationModel.TotalPages.ShouldBe(totalPages);
                    }));

        [Theory]
        [InlineData(10, 5, 1, null, 5, 1)]
        [InlineData(12, 10, 1, null, 10, 1)]
        [InlineData(20, 20, 2, null, 10, 2)]
        public void GetAllShouldReturnOnlyApprovedCateogries(
            int total,
            int approved,
            int page,
            string searchTerm,
            int expectedCategories,
            int expectedPages)
            => MyController<CategoriesController>
                .Instance(instance => instance
                    .WithData(GetMixedCategories(total, approved)))
                .Calling(c => c.All(page, searchTerm))
                .ShouldHave()
                .ActionAttributes(attr => attr
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CateogoriesListModel>()
                    .Passing(categoriesViewModel =>
                    {
                        categoriesViewModel.Categories.Count().ShouldBe(expectedCategories);
                        categoriesViewModel.PaginationModel.CurrentPage.ShouldBe(page);
                        categoriesViewModel.PaginationModel.TotalPages.ShouldBe(expectedPages);
                    }));

        [Theory]
        [InlineData(10, 5, 1, null, 5, 1)]
        [InlineData(12, 10, 1, null, 10, 1)]
        [InlineData(20, 20, 2, null, 10, 2)]
        public void GetByOwnerShouldReturnOnlyCurrentlyLoggInUserAndOnlyHisApprovedOne(
            int total,
            int approved,
            int page,
            string searchTerm,
            int expectedCategories,
            int expectedPages)
            => MyController<CategoriesController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetUsers(1))
                    .WithData(GetMixedCategories(total, approved)))
                .Calling(c => c.ByOwner(TestUser.Username, page, searchTerm))
                .ShouldHave()
                .ActionAttributes(attr => attr
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CategoriesByUserListModel>()
                    .Passing(categoriesViewModel =>
                    {
                        categoriesViewModel.Categories.Count().ShouldBe(expectedCategories);
                        categoriesViewModel.PaginationModel.CurrentPage.ShouldBe(page);
                        categoriesViewModel.PaginationModel.TotalPages.ShouldBe(expectedPages);
                    }));

        [Theory]
        [InlineData(10, 5, 1, null, "InvalidUser")]
        [InlineData(12, 10, 1, null, "InvalidUser1")]
        [InlineData(0, 0, 1, null, "InvalidUser2")]
        public void GetByOwnerShouldReturnUnauthorizeIfCurrectlyLogInUserIsNotTheOwnerOfCategories(
            int total,
            int approved,
            int page,
            string searchTerm,
            string invalidUserName)
            => MyController<CategoriesController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetUsers(1))
                    .WithData(GetMixedCategories(total, approved)))
                .Calling(c => c.ByOwner(invalidUserName, page, searchTerm))
                .ShouldHave()
                .ActionAttributes(attr => attr
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Unauthorized();

        [Fact]
        public void GetCreateShouldBeForAuthorizeUserAndReturnCorrectView()
            => MyController<CategoriesController>
                .Instance()
                .Calling(c => c.Create())
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Theory]
        [InlineData(
            "TestName",
            "TestDescriptionTestDescriptionTestDescription",
            "test.png",
            1,
            null,
            "TestUser")]
        public void PostCreateShouldBeForAuthorizeUserAndReturnCorrectView(
            string name,
            string description,
            string imageUrl,
            int id,
            string searchTerm,
            string username)
            => MyController<CategoriesController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetUsers(1)))
                .Calling(c => c.Create(new CategoryInputModel
                {
                    Name = name,
                    Description = description,
                    ImageUrl = imageUrl,
                }))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .Data(data => data
                    .WithSet<Category>(categories => categories
                        .Any(c => c.Name == name &&
                                  c.Description == description &&
                                  c.ImageUrl == imageUrl)))
                .ValidModelState()
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<CategoriesController>(c => c.ByOwner(username, id, searchTerm)));

        [Theory]
        [InlineData("TestName1", "TestDescriptionTestDescriptionTestDescription", "test.png", 1)]
        public void PostCreateShouldBeForAuthorizeUserAndReturnToSameViewIfCategoryNameIsTaken(
            string takenName,
            string description,
            string imageUrl,
            int page)
            => MyController<CategoriesController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetCategories(page)))
                .Calling(c => c.Create(new CategoryInputModel
                {
                    Name = takenName,
                    Description = description,
                    ImageUrl = imageUrl,
                }))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .Data(data => data
                    .WithSet<Category>(categories => !categories
                        .Any(c => c.Name == takenName &&
                                  c.Description == description &&
                                  c.ImageUrl == imageUrl)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(InvalidMessageKey))
                .AndAlso()
                .ShouldReturn()
                .View();

        [Theory]
        [InlineData(
            @"TestName1
                    with new line",
            "TestDescriptionTestDescriptionTestDescription",
            "test.png",
            1)]
        public void PostCreateShouldBeForAuthorizeUserAndReturnToSameViewIfModelStateIsIncorrect(
            string invalidName,
            string description,
            string imageUrl,
            int page)
            => MyController<CategoriesController>
                .Instance(instance => instance
                    .WithData(GetCategories(page)))
                .Calling(c => c.Create(new CategoryInputModel
                {
                    Name = invalidName,
                    Description = description,
                    ImageUrl = imageUrl,
                }))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .Data(data => data
                    .WithSet<Category>(categories => !categories
                        .Any(c => c.Name == invalidName &&
                                  c.Description == description &&
                                  c.ImageUrl == imageUrl)))
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View();

        [Theory]
        [InlineData(1, "TestName1", "TestDescription1")]
        public void GetEditShouldBeOnlyForAuthorizeUsersAndShouldReturnCorrectResult(
            int categoryId,
            string name,
            string description)
            => MyController<CategoriesController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetCategories(categoryId)))
                .Calling(c => c
                    .Edit(categoryId))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CategoryEditModel>()
                    .Passing(editModel =>
                    {
                        editModel.Name.ShouldBe(name);
                        editModel.Description.ShouldBe(description);
                    }));

        [Theory]
        [InlineData(1, true, true, "TestName1", "TestDescription1")]
        public void GetEditShouldReturnCorrectResultEvenIfUserIsNotTheOwnerButItIsTheAdministrator(
           int categoryId,
           bool isApprove,
           bool isDiffUser,
           string name,
           string content)
           => MyController<CategoriesController>
               .Instance(instance => instance
                   .WithUser(new string[] { AdministratorRoleName })
                   .WithData(GetCategories(categoryId, isApprove, isDiffUser)))
               .Calling(c => c
                   .Edit(categoryId))
               .ShouldHave()
               .ActionAttributes(attrs => attrs
                   .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .View(view => view
                   .WithModelOfType<CategoryEditModel>()
                   .Passing(editModel =>
                   {
                       editModel.Name.ShouldBe(name);
                       editModel.Description.ShouldBe(content);
                   }));

        [Theory]
        [InlineData(1)]
        public void GetEditShouldBeOnlyForAuthorizeUsersAndShouldReturnNotFountIfDoesntExists(
           int categoryId)
           => MyController<CategoriesController>
               .Instance()
               .Calling(c => c
                   .Edit(categoryId))
               .ShouldHave()
               .ActionAttributes(attrs => attrs
                   .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .NotFound();

        [Theory]
        [InlineData(1, true, true)]
        public void GetEditShouldBeOnlyForAuthorizeUsersAndThoesWhoOwnsThePostAndShouldReturnUnautorizeIfUserNotOwner(
           int categoryId,
           bool isApproved,
           bool isDiffOwner)
           => MyController<CategoriesController>
               .Instance(instance => instance
                    .WithUser()
                    .WithData(GetCategories(categoryId, isApproved, isDiffOwner)))
               .Calling(c => c
                   .Edit(categoryId))
               .ShouldHave()
               .ActionAttributes(attrs => attrs
                   .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .Unauthorized();

        [Theory]
        [InlineData(1, "EditName", "EditDesctiption", "EditImage.jpg", null)]
        public void PostEditShouldBeOnlyForAuthorizeUsersAndShouldReturnCorrectResult(
            int categoryId,
            string editName,
            string editDescription,
            string editImageUrl,
            string searchTerm)
            => MyController<CategoriesController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetCategories(categoryId)))
                .Calling(c => c
                    .Edit(new CategoryEditModel
                    {
                        Id = categoryId,
                        Name = editName,
                        Description = editDescription,
                        ImageUrl = editImageUrl,
                    }))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .Data(data => data
                    .WithSet<Category>(categories => categories
                        .Any(c => c.Id == categoryId &&
                                  c.Name == editName &&
                                  c.Description == editDescription &&
                                  c.ImageUrl == editImageUrl)))
                .ValidModelState()
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<CategoriesController>(c => c.ByName(editName, categoryId, searchTerm)));

        [Theory]
        [InlineData(1, true, true, "EditName", "EditDesctiption", "EditImage.jpg", 1, null)]
        public void PostEditShouldEditThePostEvenIfUserIsNotTheOwnerButItIsTheAdministrator(
          int categoryId,
          bool isApproved,
          bool isDiffOwner,
          string editName,
          string editDescription,
          string editImage,
          int page,
          string searchTerm)
          => MyController<CategoriesController>
              .Instance(instance => instance
                  .WithUser(new string[] { AdministratorRoleName })
                  .WithData(GetCategories(categoryId, isApproved, isDiffOwner)))
              .Calling(c => c
                  .Edit(new CategoryEditModel
                  {
                      Id = categoryId,
                      Name = editName,
                      Description = editDescription,
                      ImageUrl = editImage,
                  }))
              .ShouldHave()
              .ActionAttributes(attrs => attrs
                  .RestrictingForHttpMethod(HttpMethod.Post)
                  .RestrictingForAuthorizedRequests())
              .Data(data => data
                  .WithSet<Category>(post => post
                      .Any(p => p.Id == categoryId &&
                                p.Name == editName &&
                                p.Description == editDescription &&
                                p.ImageUrl == editImage)))
              .ValidModelState()
              .AndAlso()
              .ShouldReturn()
              .Redirect(redirect => redirect
                  .To<CategoriesController>(c => c.ByName(editName, page, searchTerm)));

        [Theory]
        [InlineData(1, "EditName", "EditDesctiption", "EditImage.jpg")]
        public void PostEditShouldBeOnlyForAuthorizeUsersAndShouldReturnNotFountIfDoesntExists(
           int categoryId,
           string editName,
           string editDescription,
           string editImageUrl)
           => MyController<CategoriesController>
               .Instance()
               .Calling(c => c
                    .Edit(new CategoryEditModel
                    {
                        Id = categoryId,
                        Name = editName,
                        Description = editDescription,
                        ImageUrl = editImageUrl,
                    }))
               .ShouldHave()
               .ActionAttributes(attrs => attrs
                   .RestrictingForHttpMethod(HttpMethod.Post)
                   .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .NotFound();

        [Theory]
        [InlineData(1, true, true, "EditName", "EditDesctiption", "EditImage.jpg")]
        public void PostEditShouldReturnUnauthorizeIfUserIsNotTheOwner(
          int categoryId,
          bool isApproved,
          bool isDiffOwner,
          string editName,
          string editDescription,
          string editImage)
           => MyController<CategoriesController>
               .Instance(instance => instance
                    .WithUser()
                    .WithData(GetCategories(categoryId, isApproved, isDiffOwner)))
               .Calling(c => c
                    .Edit(new CategoryEditModel
                    {
                        Id = categoryId,
                        Name = editName,
                        Description = editDescription,
                        ImageUrl = editImage,
                    }))
               .ShouldHave()
               .ActionAttributes(attrs => attrs
                   .RestrictingForHttpMethod(HttpMethod.Post)
                   .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .Unauthorized();

        [Theory]
        [InlineData(1, "editName", "EditDesctiption", "EditImage.jpg")]
        public void PostEditShouldReturnToSameViewIfStateOfNameNotValid(
           int categoryId,
           string invalidName,
           string editDescription,
           string editImageUrl)
           => MyController<CategoriesController>
               .Instance()
               .Calling(c => c
                    .Edit(new CategoryEditModel
                    {
                        Id = categoryId,
                        Name = invalidName,
                        Description = editDescription,
                        ImageUrl = editImageUrl,
                    }))
               .ShouldHave()
               .ActionAttributes(attrs => attrs
                   .RestrictingForHttpMethod(HttpMethod.Post)
                   .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .View(view => view
                    .WithModelOfType<int>()
                    .Passing(id => id.ShouldBe(categoryId)));

        [Theory]
        [InlineData(1, false, "TestName1", "TestDescription1")]
        public void GetDeleteShouldReturnCorrectViewIfIdIsCorrect(
            int categoryId,
            bool isFromAdminPanel,
            string name,
            string description)
            => MyController<CategoriesController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetCategories(categoryId)))
                .Calling(c => c.Delete(categoryId, isFromAdminPanel))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CategoryEditModel>()
                    .Passing(model =>
                    {
                        model.Name.ShouldBe(name);
                        model.Description.ShouldBe(description);
                    }));

        [Theory]
        [InlineData(1, true, true, false, "TestName1", "TestDescription1", "TestImageURl")]
        public void GetDeleteShouldReturnCorrectViewIfUserIsNotTheOwnerButIsTheAdministrator(
          int categoryId,
          bool isApproved,
          bool isDiffOwner,
          bool isFromAdminPanel,
          string name,
          string description,
          string image)
           => MyController<CategoriesController>
               .Instance(instance => instance
                   .WithUser(new string[] { AdministratorRoleName })
                   .WithData(GetCategories(categoryId, isApproved, isDiffOwner)))
               .Calling(c => c.Delete(categoryId, isFromAdminPanel))
               .ShouldHave()
               .ActionAttributes(attrs => attrs
                   .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .View(view => view
                   .WithModelOfType<CategoryEditModel>()
                   .Passing(model =>
                   {
                       model.Name.ShouldBe(name);
                       model.Description.ShouldBe(description);
                       model.ImageUrl.ShouldBe(image);
                   }));

        [Theory]
        [InlineData(null, false)]
        public void GetDeleteShouldReturnNotFoundIfIdIsNull(
            int? categoryId,
            bool isFromAdminPanel)
            => MyController<CategoriesController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetCategories(1)))
                .Calling(c => c.Delete(categoryId, isFromAdminPanel))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .BadRequest();

        [Theory]
        [InlineData(1, false)]
        public void GetDeleteShouldReturnNotFoundIfCategoryDoesntExits(
            int categoryId,
            bool isFromAdminPanel)
            => MyController<CategoriesController>
                .Instance(instance => instance
                    .WithUser())
                .Calling(c => c.Delete(categoryId, isFromAdminPanel))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .NotFound();

        [Theory]
        [InlineData(1, true, true, false)]
        [InlineData(1, true, true, true)]
        public void GetDeleteShouldReturnUnauthorizeIfLogInUserIsNotTheOwner(
          int categoryId,
          bool isApproved,
          bool isDiffOwner,
          bool isFromAdminPanel)
          => MyController<CategoriesController>
              .Instance(instance => instance
                  .WithUser()
                  .WithData(GetCategories(categoryId, isApproved, isDiffOwner)))
              .Calling(c => c.Delete(categoryId, isFromAdminPanel))
              .ShouldHave()
              .ActionAttributes(attrs => attrs
                  .RestrictingForAuthorizedRequests())
              .AndAlso()
              .ShouldReturn()
              .Unauthorized();

        [Theory]
        [InlineData(1, 1, false, "TestUser", null)]
        public void PostDeleteShouldRedirectToByOwnerCategoryIfSuccessfullyDeletesCategory(
            int categoryId,
            int page,
            bool isFromAdminPanel,
            string username,
            string searchTerm)
            => MyController<CategoriesController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetCategories(categoryId)))
                .Calling(c => c.DeleteConfirmed(categoryId, isFromAdminPanel))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<CategoriesController>(c => c.ByOwner(username, page, searchTerm)));

        [Theory]
        [InlineData(1, true, true, false, 1, null)]
        public void PostDeleteShouldDeleteEvenIfUserIsNotTheOwnerButIsTheAdministrator(
         int postId,
         bool isApproved,
         bool isDiffOwner,
         bool isFromAdminPanel,
         int page,
         string searchTerm)
         => MyController<CategoriesController>
             .Instance(instance => instance
                 .WithUser(new string[] { AdministratorRoleName })
                 .WithData(GetCategories(postId, isApproved, isDiffOwner)))
             .Calling(c => c.DeleteConfirmed(postId, isFromAdminPanel))
             .ShouldHave()
             .ActionAttributes(attrs => attrs
                 .RestrictingForHttpMethod(HttpMethod.Post)
                 .RestrictingForAuthorizedRequests())
             .AndAlso()
             .ShouldReturn()
             .Redirect(redirect => redirect
                 .To<CategoriesController>(c => c.ByOwner(TestUser.Username, page, searchTerm)));

        [Theory]
        [InlineData(1, 1, null, true)]
        public void PostDeleteShouldRedirectToAdminPanelIfAdministratorIsDeletingAndIfSuccessfullyDeletesCategory(
            int categoryId,
            int page,
            string searchTerm,
            bool isFromAdminPanel)
            => MyController<CategoriesController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetCategories(categoryId)))
                .Calling(c => c.DeleteConfirmed(categoryId, isFromAdminPanel))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<CategoriesAdminController>(c => c.Index(page, searchTerm)));

        [Theory]
        [InlineData(1, false, 2)]
        public void PostDeleteShouldReturnNotFoundIfCategoryDoesntExists(
            int categoryId,
            bool isFromAdminPanel,
            int invalidCategoryId)
            => MyController<CategoriesController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetCategories(categoryId)))
                .Calling(c => c.DeleteConfirmed(invalidCategoryId, isFromAdminPanel))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .NotFound();

        [Theory]
        [InlineData(1, true, true, false)]
        public void PostDeleteShouldReturnUnauthorizeIfCategoryDoesntExists(
           int postId,
           bool isApproved,
           bool isDiffOwner,
           bool isFromAdminPanel)
           => MyController<CategoriesController>
               .Instance(instance => instance
                   .WithUser()
                   .WithData(GetCategories(postId, isApproved, isDiffOwner)))
               .Calling(c => c.DeleteConfirmed(postId, isFromAdminPanel))
               .ShouldHave()
               .ActionAttributes(attrs => attrs
                   .RestrictingForHttpMethod(HttpMethod.Post)
                   .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .Unauthorized();
    }
}
