namespace ForumSystem.Web.Tests.Controllers.Admin
{
    using System.Linq;

    using ForumSystem.Data.Models;
    using ForumSystem.Web.Areas.Administration.Controllers;
    using ForumSystem.Web.ViewModels.Administration.Categories;

    using MyTested.AspNetCore.Mvc;
    using Shouldly;
    using Xunit;

    using static ForumSystem.Common.GlobalConstants;
    using static ForumSystem.Web.Tests.Data.CategoiresTestData;

    public class CategoriesAdminControllerTests
    {
        [Theory]
        [InlineData(15, 5, 1, 3, 1)]
        [InlineData(10, 5, 2, 2, 2)]
        [InlineData(5, 5, 0, 1, 1)]
        public void GetIndexShouldBeRestrictedOnlyForAdministrationAndReturnCorrectResult(
            int totalCategories,
            int cateogryPerPage,
            int currentPage,
            int totalPages,
            int expectedCurrPage)
            => MyController<CategoriesController>
                .Instance(instance => instance
                        .WithUser()
                        .WithData(GetCategories(totalCategories)))
                .Calling(c => c.Index(currentPage))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests(withAllowedRoles: AdministratorRoleName))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CategoryCrudModelList>()
                    .Passing(categoryCrudModel =>
                    {
                        categoryCrudModel.Categories
                            .Count()
                            .ShouldBe(cateogryPerPage);
                        categoryCrudModel.PaginationModel.CurrentPage
                            .ShouldBe(expectedCurrPage);
                        categoryCrudModel.PaginationModel.TotalPages
                            .ShouldBe(totalPages);
                    }));

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
        [InlineData("TestName", "TestDescriptionTestDescriptionTestDescription", "test.png", 1)]
        public void PostCreateShouldBeForAuthorizeUserAndReturnCorrectView(
            string name,
            string description,
            string imageUrl,
            int id)
            => MyController<CategoriesController>
                .Instance()
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
                    .To<ForumSystem.Web.Controllers.CategoriesController>(c => c.ByName(name, id)));

        [Theory]
        [InlineData("TestName1", "TestDescriptionTestDescriptionTestDescription", "test.png", 1)]
        public void PostCreateShouldBeForAuthorizeUserAndReturnToSameViewIfCategoryNameIsTaken(
            string name,
            string description,
            string imageUrl,
            int page)
            => MyController<CategoriesController>
                .Instance(instance => instance
                    .WithData(GetCategories(page)))
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
                    .WithSet<Category>(categories => !categories
                        .Any(c => c.Name == name &&
                                  c.Description == description &&
                                  c.ImageUrl == imageUrl)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(InvalidMessageKey))
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
        [InlineData(1, "EditName", "EditDesctiption", "EditImage.jpg")]
        public void PostEditShouldBeOnlyForAuthorizeUsersAndShouldReturnCorrectResult(
            int categoryId,
            string editName,
            string editDescription,
            string editImageUrl)
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
                    .To<ForumSystem.Web.Controllers.CategoriesController>(c => c.ByName(editName, categoryId)));

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
        [InlineData(1, "TestName1", "TestDescription1")]
        public void GetDeleteShouldReturnCorrectViewIfIdIsCorrect(
            int categoryId,
            string name,
            string description)
            => MyController<CategoriesController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetCategories(categoryId)))
                .Calling(c => c.Delete(categoryId))
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
        [InlineData(null)]
        public void GetDeleteShouldReturnNotFoundIfIdIsNull(
            int? categoryId)
            => MyController<CategoriesController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetCategories(1)))
                .Calling(c => c.Delete(categoryId))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .NotFound();

        [Theory]
        [InlineData(1)]
        public void GetDeleteShouldReturnNotFoundIfCategoryDoesntExits(
            int categoryId)
            => MyController<CategoriesController>
                .Instance(instance => instance
                    .WithUser())
                .Calling(c => c.Delete(categoryId))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .NotFound();

        [Theory]
        [InlineData(1, 1)]
        public void PostDeleteShouldRedirectIfSuccessfullyDeletesCategory(
            int categoryId,
            int page)
            => MyController<CategoriesController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetCategories(categoryId)))
                .Calling(c => c.DeleteConfirmed(categoryId))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<ForumSystem.Web.Controllers.CategoriesController>(c => c.All(page)));

        [Theory]
        [InlineData(1, 2)]
        public void PostDeleteShouldReturnNotFoundIfCategoryDoesntExists(
            int categoryId,
            int invalidCategoryId)
            => MyController<CategoriesController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetCategories(categoryId)))
                .Calling(c => c.DeleteConfirmed(invalidCategoryId))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .NotFound();
    }
}
