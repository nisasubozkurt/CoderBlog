
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.Mvc.Controllers;
using ProgrammersBlog.Mvc.Models;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Shared.Utilities.Results.Abstract;
using ProgrammersBlog.Shared.Utilities.Results.ComplexTypes;
using System.Threading.Tasks;

namespace Entegrasyon.UserControllerTest
{
    [TestFixture]
    public class ArticleControllerTest
    {
        private Mock<IArticleService> _mockArticleService;
        private Mock<IOptionsSnapshot<ArticleRightSideBarWidgetOptions>> _mockOptions;
        private ArticleController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockArticleService = new Mock<IArticleService>();
            _mockOptions = new Mock<IOptionsSnapshot<ArticleRightSideBarWidgetOptions>>();
            _controller = new ArticleController(_mockArticleService.Object, _mockOptions.Object);
        }

        [Test]
        public async Task Search_WithValidKeyword_ReturnsViewWithArticles()
        {
            // Arrange
            var keyword = "test";
            var mockResult = new Mock<IDataResult<ArticleListDto>>();
            mockResult.Setup(m => m.ResultStatus).Returns(ResultStatus.Success);
            _mockArticleService.Setup(s => s.SearchAsync(keyword, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(mockResult.Object);

            // Act
            var result = await _controller.Search(keyword);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsInstanceOf<ArticleSearchViewModel>(viewResult.Model);
        }

        [Test]
        public async Task Search_WithInvalidKeyword_ReturnsNotFound()
        {
            // Arrange
            var keyword = "nonexist";
            var mockResult = new Mock<IDataResult<ArticleListDto>>();
            mockResult.Setup(m => m.ResultStatus).Returns(ResultStatus.Error);
            _mockArticleService.Setup(s => s.SearchAsync(keyword, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(mockResult.Object);

            // Act
            var result = await _controller.Search(keyword);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Detail_WithInvalidArticleId_ReturnsNotFound()
        {
            // Arrange
            var articleId = 99;
            var mockResult = new Mock<IDataResult<ArticleDto>>();
            mockResult.Setup(m => m.ResultStatus).Returns(ResultStatus.Error);
            _mockArticleService.Setup(s => s.GetAsync(articleId)).ReturnsAsync(mockResult.Object);

            // Act
            var result = await _controller.Detail(articleId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}
