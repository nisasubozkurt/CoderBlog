
using NUnit.Framework;
using Moq;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Entities.ComplexTypes;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.Mvc.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using NToastNotify;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ProgrammersBlog.Mvc.Helpers.Abstract;
using System.Security.Claims;
using System.Collections.Generic;
using System.Security.Principal;
using System;
using ProgrammersBlog.Shared.Utilities.Results.ComplexTypes;
using ProgrammersBlog.Shared.Utilities.Results.Concrete;
using ProgrammersBlog.Mvc.Areas.Admin.Models;

namespace Entegrasyon.AdminControllerTest
{
    [TestFixture]
    public class ArticleControllerTests
    {
        private Mock<IArticleService> _mockArticleService;
        private Mock<ICategoryService> _mockCategoryService;
        private Mock<IMapper> _mockMapper;
        private Mock<IImageHelper> _mockImageHelper;
        private Mock<IToastNotification> _mockToastNotification;
        private Mock<UserManager<User>> _mockUserManager;
        private ArticleController _controller;

        [SetUp]
        public void SetUp()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            _mockArticleService = new Mock<IArticleService>();
            _mockCategoryService = new Mock<ICategoryService>();
            _mockMapper = new Mock<IMapper>();
            _mockImageHelper = new Mock<IImageHelper>();
            _mockToastNotification = new Mock<IToastNotification>();

            _controller = new ArticleController(
                _mockArticleService.Object,
                _mockCategoryService.Object,
                _mockUserManager.Object,
                _mockMapper.Object,
                _mockImageHelper.Object,
                _mockToastNotification.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "username")
            }));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Test]
        public async Task Add_Get_ReturnsView_WhenSuccess()
        {
            _mockCategoryService.Setup(s => s.GetAllByNonDeletedAndActiveAsync()).ReturnsAsync(new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto { Categories = new List<Category>() }));

            var result = await _controller.Add();

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Add_Get_ReturnsNotFound_WhenFail()
        {
            _mockCategoryService.Setup(s => s.GetAllByNonDeletedAndActiveAsync()).ReturnsAsync(new DataResult<CategoryListDto>(ResultStatus.Error, null));

            var result = await _controller.Add();

            Assert.IsInstanceOf<NotFoundResult>(result);
        }



        [Test]
        public async Task Add_ValidModelState_ReturnsRedirectToActionIndex()
        {
            var articleAddViewModel = new ArticleAddViewModel
            {
                Title = "Title",
                Content = "content",
            };
            _mockMapper.Setup(x => x.Map<ArticleAddDto>(It.IsAny<ArticleAddViewModel>())).Returns(new ArticleAddDto());
            _mockArticleService.Setup(x => x.AddAsync(It.IsAny<ArticleAddDto>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(new Result(ResultStatus.Success, "Article added"));

            var result = await _controller.Add(articleAddViewModel);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectToActionResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [Test]
        public async Task Add_InvalidModelState_ReturnsViewWithModel()
        {

            var articleAddDto = new ArticleAddDto { Title = "Test Title" };
            var user = new User { Id = 1 };
            var article = new Article { Id = 1, UserId = user.Id, CommentCount = 0 };
            var category = new Category { Id = 1, Name = "test" };
            _controller.ModelState.AddModelError("Error", "ModelState is invalid");
            var articleAddViewModel = new ArticleAddViewModel
            {
                Title= "Title",
                CategoryId = 1
            };

            var result = await _controller.Add(articleAddViewModel);

            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(articleAddViewModel, viewResult.Model);
        }

        [Test]
        public async Task Update_ValidModelState_ReturnsRedirectToActionIndex()
        {
            var articleUpdateViewModel = new ArticleUpdateViewModel();
            _mockMapper.Setup(x => x.Map<ArticleUpdateDto>(It.IsAny<ArticleUpdateViewModel>())).Returns(new ArticleUpdateDto());
            _mockArticleService.Setup(x => x.UpdateAsync(It.IsAny<ArticleUpdateDto>(), It.IsAny<string>()))
                .ReturnsAsync(new Result(ResultStatus.Success, "Article updated"));

            var result = await _controller.Update(articleUpdateViewModel);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectToActionResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [Test]
        public async Task Update_InvalidModelState_ReturnsViewWithModel()
        {
            _controller.ModelState.AddModelError("Error", "ModelState is invalid");
            var articleUpdateViewModel = new ArticleUpdateViewModel();

            var result = await _controller.Update(articleUpdateViewModel);

            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(articleUpdateViewModel, viewResult.Model);
        }

    }
}
