
using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Mvc.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.Shared.Utilities.Results.ComplexTypes;
using ProgrammersBlog.Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using ProgrammersBlog.Mvc.Helpers.Abstract;
using ProgrammersBlog.Shared.Utilities.Results.Concrete;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AdminControllerTest
{
    [TestFixture]
    public class CategoryControllerTests
    {
        private Mock<ICategoryService> _categoryServiceMock;
        private Mock<UserManager<User>> _userManagerMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IImageHelper> _imageHelperMock;
        private CategoryController _categoryController;

        [SetUp]
        public void Setup()
        {
            _categoryServiceMock = new Mock<ICategoryService>();
            var store = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _mapperMock = new Mock<IMapper>();
            _imageHelperMock = new Mock<IImageHelper>();
            _categoryController = new CategoryController(_categoryServiceMock.Object, _userManagerMock.Object, _mapperMock.Object, _imageHelperMock.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "username")
            }));

            _categoryController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Test]
        public async Task Add_Post_InvalidModel_ReturnsJson()
        {
            // Arrange
            _categoryController.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _categoryController.Add(new CategoryAddDto());

            // Assert
            Assert.That(result, Is.InstanceOf<JsonResult>());
        }

        [Test]
        public async Task Add_Post_ValidModel_ReturnsJson()
        {
            var user = new User
            {
                Id = 1,
                UserName = "test",
                Email = "test@gmail.com"
            };
            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            // Arrange
            var categoryAddDto = new CategoryAddDto { Name = "Test", Description = "Test Description" };
            _categoryServiceMock.Setup(service => service.AddAsync(It.IsAny<CategoryAddDto>(), It.IsAny<string>())).ReturnsAsync(new DataResult<CategoryDto>(ResultStatus.Success, new CategoryDto()));

            // Act
            var result = await _categoryController.Add(categoryAddDto);

            // Assert
            Assert.That(result, Is.InstanceOf<JsonResult>());
        }

        [Test]
        public async Task Update_Get_ReturnsPartialView()
        {
            // Arrange
            _categoryServiceMock.Setup(service => service.GetCategoryUpdateDtoAsync(It.IsAny<int>())).ReturnsAsync(new DataResult<CategoryUpdateDto>(ResultStatus.Success, new CategoryUpdateDto()));

            // Act
            var result = await _categoryController.Update(1);

            // Assert
            Assert.That(result, Is.InstanceOf<PartialViewResult>());
        }

        [Test]
        public async Task Update_Post_InvalidModel_ReturnsJson()
        {
            // Arrange
            _categoryController.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _categoryController.Update(new CategoryUpdateDto());

            // Assert
            Assert.That(result, Is.InstanceOf<JsonResult>());
        }

        [Test]
        public async Task Update_Post_ValidModel_ReturnsJson()
        {
            var user = new User
            {
                Id = 1,
                UserName = "test",
                Email = "test@gmail.com"
            };
            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            // Arrange
            var categoryUpdateDto = new CategoryUpdateDto { Id = 1, Name = "Updated Test", Description = "Updated Description" };
            _categoryServiceMock.Setup(service => service.UpdateAsync(It.IsAny<CategoryUpdateDto>(), It.IsAny<string>()))
                .ReturnsAsync(new DataResult<CategoryDto>(ResultStatus.Success, new CategoryDto()));

            // Act
            var result = await _categoryController.Update(categoryUpdateDto);

            // Assert
            Assert.That(result, Is.InstanceOf<JsonResult>());
        }
    }
}
