using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Mvc.Areas.Admin.Controllers;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Mvc.Helpers.Abstract;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using ProgrammersBlog.Shared.Utilities.Results.ComplexTypes;
using System.Collections.Generic;
using ProgrammersBlog.Shared.Utilities.Results.Concrete;
using ProgrammersBlog.Shared.Utilities.Results.Abstract;

namespace Entegrasyon.AdminControllerTest
{
    [TestFixture]
    public class CommentControllerTests
    {
        private Mock<ICommentService> _commentServiceMock;
        private CommentController _commentController;
        private Mock<UserManager<User>> _userManagerMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IImageHelper> _imageHelperMock;

        [SetUp]
        public void Setup()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            _commentServiceMock = new Mock<ICommentService>();
            _mapperMock = new Mock<IMapper>();
            _imageHelperMock = new Mock<IImageHelper>();
            _commentController = new CommentController(_userManagerMock.Object, _mapperMock.Object, _imageHelperMock.Object, _commentServiceMock.Object);

            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
            _commentController.TempData = tempData;

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Name, "testUser")
            }));

            _commentController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };
        }

        [Test]
        public async Task GetDetail_ValidId_ReturnsPartialViewWithComment()
        {
            // Arrange
            _commentServiceMock.Setup(service => service.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(new DataResult<CommentDto>(ResultStatus.Success, new CommentDto()));

            // Act
            var result = await _commentController.GetDetail(1);

            // Assert
            Assert.IsInstanceOf<PartialViewResult>(result);
        }

        [Test]
        public async Task GetDetail_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _commentServiceMock.Setup(service => service.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(new DataResult<CommentDto>(ResultStatus.Error, null));

            // Act
            var result = await _commentController.GetDetail(-1);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

    }
}