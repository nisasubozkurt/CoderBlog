
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ProgrammersBlog.Data.Abstract;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Services.Concrete;
using ProgrammersBlog.Shared.Utilities.Results.Abstract;
using ProgrammersBlog.Shared.Utilities.Results.ComplexTypes;
using ProgrammersBlog.Shared.Utilities.Results.Concrete;
using static ProgrammersBlog.Services.Utilities.Messages;
using Article = ProgrammersBlog.Entities.Concrete.Article;
using Comment = ProgrammersBlog.Entities.Concrete.Comment;

namespace CRUDTest
{
    [TestFixture]
    public class CommentManagerTests
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
        public async Task GetAsync_ExistingCommentId_ReturnSuccessWithData()
        {
            // Arrange
            int commentId = 1;
            Comment fakeComment = new Comment { Id = commentId };

            _mockUnitOfWork.Setup(x => x.Comments.GetAsync(It.IsAny<Expression<Func<Comment, bool>>>(),
                It.IsAny<Expression<Func<Comment, object>>[]>())).ReturnsAsync(fakeComment);


            // Act
            var result = await _commentService.GetAsync(commentId);

            // Assert
            Assert.That(result.ResultStatus, Is.EqualTo(ResultStatus.Success));
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task GetAsync_NonExistingCommentId_ReturnError()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.Comments.GetAsync(It.IsAny<Expression<Func<Comment, bool>>>(),
                It.IsAny<Expression<Func<Comment, object>>[]>())).ReturnsAsync((Comment)null);

            // Act
            var result = await _commentService.GetAsync(0);


            // Assert
            Assert.That(result.ResultStatus, Is.EqualTo(ResultStatus.Error));

            Assert.IsNull(result.Data.Comment);
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

        [Test]
        public async Task DeleteAsync_ExistingCommentId_ReturnsSuccess()
        {
            // Arrange
            int commentId = 1;
            int articleId = 1;
            var user = new User { Id = 1 };
            var articleAddDto = new ArticleAddDto { Title = "Test Title" };
            var article = new Article { Id = 1, UserId = user.Id, CommentCount = 0 };
            Comment comment = new Comment { Id = commentId, CreatedByName = "TestUser", ArticleId = 1, Article = article};
            _mockMapper.Setup(m => m.Map<Article>(articleAddDto)).Returns(article);
            _mockUnitOfWork.Setup(uow => uow.Articles.AddAsync(It.IsAny<Article>())).ReturnsAsync(article);
            _mockUnitOfWork.Setup(x => x.Comments.GetAsync(It.IsAny<Expression<Func<Comment, bool>>>(),
                It.IsAny<Expression<Func<Comment, object>>[]>())).ReturnsAsync(comment);
            // Act
            var result = await _commentService.DeleteAsync(commentId, "TestUser");

            // Assert
            Assert.That(result.ResultStatus, Is.EqualTo(ResultStatus.Success));
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task DeleteAsync_NonExistingCommentId_ReturnsError()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.Comments.GetAsync(It.IsAny<Expression<Func<Comment, bool>>>(),
                It.IsAny<Expression<Func<Comment, object>>[]>())).ReturnsAsync((Comment)null);
            // Act
            var result = await _commentService.DeleteAsync(-1, "");

            // Assert
            Assert.That(result.ResultStatus, Is.EqualTo(ResultStatus.Error));
            Assert.IsNull(result.Data.Comment);
        }
        
    }
}
