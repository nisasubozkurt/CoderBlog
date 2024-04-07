using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using ProgrammersBlog.Data.Abstract;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.Mvc.Controllers;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Services.Concrete;
using ProgrammersBlog.Shared.Utilities.Results.ComplexTypes;
using ProgrammersBlog.Shared.Utilities.Results.Concrete;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Entegrasyon.UserControllerTest
{
    [TestFixture]
    public class CommentControllerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private ICommentService _commentService;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _commentService = new CommentManager(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Test]
        public async Task AddAsync_ValidComment_ReturnsSuccess()
        {
            // Arrange
            CommentAddDto commentAddDto = new CommentAddDto { ArticleId = 1, CreatedByName = "TestUser" };
            Comment createdComment = new Comment { Id = 1 };
            _mockUnitOfWork.Setup(u => u.Articles.GetAsync(It.IsAny<Expression<Func<Article, bool>>>(),
                It.IsAny<Expression<Func<Article, object>>[]>())).ReturnsAsync(new Article());
            _mockMapper.Setup(m => m.Map<Comment>(It.IsAny<CommentAddDto>())).Returns(createdComment);
            _mockUnitOfWork.Setup(u => u.Comments.AddAsync(It.IsAny<Comment>())).ReturnsAsync(createdComment);

            // Act
            var result = await _commentService.AddAsync(commentAddDto);

            // Assert
            Assert.That(result.ResultStatus, Is.EqualTo(ResultStatus.Success));
            Assert.AreEqual(createdComment, result.Data.Comment);
        }

        [Test]
        public async Task AddAsync_InvalidArticleId_ReturnsError()
        {
            // Arrange
            CommentAddDto commentAddDto = new CommentAddDto { ArticleId = 1, CreatedByName = "TestUser" };
            Comment createdComment = new Comment { Id = 1 };
            _mockUnitOfWork.Setup(u => u.Articles.GetAsync(It.IsAny<Expression<Func<Article, bool>>>(), null)).ReturnsAsync((Article)null);

            // Act
            var result = await _commentService.AddAsync(commentAddDto);

            // Assert
            Assert.That(result.ResultStatus, Is.EqualTo(ResultStatus.Error));
            Assert.IsNull(result.Data);
        }

        private T ConvertJsonResultToObject<T>(JsonResult jsonResult)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>((string)jsonResult.Value);
        }
    }
}