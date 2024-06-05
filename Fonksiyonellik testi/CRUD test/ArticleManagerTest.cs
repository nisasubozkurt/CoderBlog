using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NToastNotify;
using NUnit.Framework;
using ProgrammersBlog.Data.Abstract;
using ProgrammersBlog.Entities.ComplexTypes;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.Mvc.Areas.Admin.Controllers;
using ProgrammersBlog.Mvc.Areas.Admin.Models;
using ProgrammersBlog.Mvc.Helpers.Abstract;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Services.Concrete;
using ProgrammersBlog.Shared.Utilities.Results.Abstract;
using ProgrammersBlog.Shared.Utilities.Results.ComplexTypes;
using ProgrammersBlog.Shared.Utilities.Results.Concrete;
using static ProgrammersBlog.Services.Utilities.Messages;
using Article = ProgrammersBlog.Entities.Concrete.Article;

namespace CRUDTest
{
    [TestFixture]
    public class ArticleManagerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private Mock<UserManager<User>> _mockUserManager;

        private ArticleManager _articleManager;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();

            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            _articleManager = new ArticleManager(_mockUnitOfWork.Object, _mockMapper.Object, _mockUserManager.Object);
        }

        [Test]
        public async Task GetAsync_ExistingArticleId_ReturnsSuccessWithData()
        {
            // Arrange
            int articleId = 1;
            var comment = new ProgrammersBlog.Entities.Concrete.Comment {Id=1, ArticleId = articleId, CreatedByName = "TestUser" };
            var article = new Article { Id = articleId, Title = "Test Category", Comments = null, CommentCount=0 };
            _mockUnitOfWork.Setup(u => u.Articles.GetAsync(It.IsAny<Expression<Func<Article, bool>>>(),
                It.IsAny<Expression<Func<Article, object>>[]>())).ReturnsAsync(article);

            // Act
            var result = await _articleManager.GetAsync(articleId);

            // Assert
            Assert.That(result.ResultStatus, Is.EqualTo(ResultStatus.Success));
            Assert.That(result.Data.Article.Id, Is.EqualTo(articleId));
        }

        [Test]
        public async Task GetAsync_NonExistingArticleId_ReturnsError()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.Articles.GetAsync(It.IsAny<Expression<Func<Article, bool>>>(),
                                                           It.IsAny<Expression<Func<Article, object>>[]>()))
                                                            .ReturnsAsync(() => null);

            // Act
            var result = await _articleManager.GetAsync(0);
            Assert.That(result.ResultStatus, Is.EqualTo(ResultStatus.Error));
        }

        [Test]
        public async Task GetAllAsync_WithArticles_ReturnsSuccessWithData(){
            _mockUnitOfWork.Setup(u => u.Articles.GetAllAsync(null,
                It.IsAny<Expression<Func<Article, object>>[]>()))
                .ReturnsAsync(new List<Article> { new Article(), new Article() });
            var result = await _articleManager.GetAllAsync();
            Assert.That(result.ResultStatus, Is.EqualTo(ResultStatus.Success));
            Assert.That(result.Data.Articles.Count, Is.GreaterThan(0));
        }

        [Test]
        public async Task GetAllAsync_NoArticles_ReturnsError()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.Articles.GetAllAsync(null,
                It.IsAny<Expression<Func<Article, object>>[]>())).ReturnsAsync(new List<Article>());

            // Act
            var result = await _articleManager.GetAllAsync();

            // Assert
            Assert.That(result.ResultStatus, Is.EqualTo(ResultStatus.Success));
        }

        // Example for one negative case

        [Test]
        public async Task AddAsync_ReturnsSuccessResult()
        {
            // Arrange
            var articleAddDto = new ArticleAddDto { Title = "Test Title" };
            var user = new User { Id = 1 };
            var article = new Article { Id = 1, UserId = user.Id, CommentCount = 0};
            _mockMapper.Setup(m => m.Map<Article>(articleAddDto)).Returns(article);
            _mockUnitOfWork.Setup(uow => uow.Articles.AddAsync(It.IsAny<Article>())).ReturnsAsync(article);

            // Act
            var result = await _articleManager.AddAsync(articleAddDto, "user1", user.Id);

            // Assert
            Assert.That(result.ResultStatus, Is.EqualTo(ResultStatus.Success));
        }

        [Test]
        public async Task DeleteAsync_CategoryNotExists_ReturnErrorResult()
        {
            // Arrange

            _mockUnitOfWork.Setup(x => x.Articles.GetAsync(It.IsAny<Expression<Func<Article, bool>>>(),
                It.IsAny<Expression<Func<Article, object>>[]>())).ReturnsAsync((Article)null);

            // Act
            var result = await _articleManager.DeleteAsync(0, "TestUser");

            // Assert
            Assert.That(result.ResultStatus, Is.EqualTo(ResultStatus.Error));
        }
        /*
        [Test]
        public async Task DeleteAsync_CategoryExists_ReturnsSuccessResult()
        {
            // Arrange
            var articleId = 1;
            var articleAddDto = new ArticleAddDto { Title = "Test Title" };
            var user = new User { Id = 1 };
            var article = new Article { Id = 1, UserId = user.Id, CommentCount = 0 };
            _mockMapper.Setup(m => m.Map<Article>(articleAddDto)).Returns(article);
            _mockUnitOfWork.Setup(uow => uow.Articles.AddAsync(It.IsAny<Article>())).ReturnsAsync(article);

            _mockUnitOfWork.Setup(x => x.Articles.GetAsync(It.IsAny<Expression<System.Func<Article, bool>>>(),
            It.IsAny<Expression<Func<Article, object>>[]>())).ReturnsAsync(article);

            // Act
            var result = await _articleManager.DeleteAsync(articleId, "testUser");

            // Assert
            Assert.That(result.ResultStatus, Is.EqualTo(ResultStatus.Success));
        }*/
    }
}
