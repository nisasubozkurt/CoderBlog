using System;
using NUnit.Framework;
using Moq;
using AutoMapper;
using ProgrammersBlog.Data.Abstract;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.Services.Concrete;
using ProgrammersBlog.Shared.Utilities.Results.Abstract;
using ProgrammersBlog.Shared.Utilities.Results.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Shared.Utilities.Results.ComplexTypes;
using static ProgrammersBlog.Services.Utilities.Messages;
using System.Linq.Expressions;
using Category = ProgrammersBlog.Entities.Concrete.Category;
using Article = ProgrammersBlog.Entities.Concrete.Article;

namespace CRUDTest
{ 
    [TestFixture]
    public class CategoryManagerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private CategoryManager _categoryManager;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _categoryManager = new CategoryManager(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Test]
        public async Task GetAsync_CategoryExists_ReturnCategoryDtoSuccessResult()
        {
            // Arrange
            var categoryId = 1;
            var category = new Category { Id = categoryId, Name = "Test Category" };
            _mockUnitOfWork.Setup(x => x.Categories.GetAsync(It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Expression<Func<Category, object>>[]>())).ReturnsAsync(new Category { Id = categoryId });

            // Act
            var result = await _categoryManager.GetAsync(categoryId);

            // Assert
            Assert.That(result.ResultStatus, Is.EqualTo(ResultStatus.Success));
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task GetAsync_CategoryNotExists_ReturnErrorResult()
        {
            // Arrange

            _mockUnitOfWork.Setup(x => x.Categories.GetAsync(It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Expression<Func<Category, object>>[]>())).ReturnsAsync((Category)null);

            // Act
            var result = await _categoryManager.GetAsync(0);

            // Assert
            Assert.That(result.ResultStatus, Is.EqualTo(ResultStatus.Error));
            Assert.IsNull(result.Data.Category);
        }

        [Test]
        public async Task GetAllAsync_WhenCalled_ReturnCategoryListDtoSuccessResult()
        {
            // Arrange
            List<Category> categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category 1" },
                new Category { Id = 2, Name = "Category 2" }
            };
            _mockUnitOfWork.Setup(x => x.Categories.GetAllAsync(null)).ReturnsAsync(categories);

            // Act
            var result = await _categoryManager.GetAllAsync();

            // Assert
            Assert.That(result.ResultStatus, Is.EqualTo(ResultStatus.Success));
            Assert.That(result.Data.Categories.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task AddAsync_ValidCategory_ReturnCategoryDtoSuccessResult()
        {
            // Arrange
            CategoryAddDto categoryAddDto = new CategoryAddDto { Name = "New Category", Description = "New Description" };
            Category category = new Category { Id = 1, Name = categoryAddDto.Name, Description = categoryAddDto.Description };
            _mockMapper.Setup(x => x.Map<Category>(It.IsAny<CategoryAddDto>())).Returns(category);
            _mockUnitOfWork.Setup(x => x.Categories.AddAsync(It.IsAny<Category>())).ReturnsAsync(category);

            // Act
            var result = await _categoryManager.AddAsync(categoryAddDto, "TestUser");

            // Assert
            Assert.That(result.ResultStatus, Is.EqualTo(ResultStatus.Success));
            Assert.That(result.Data.Category.Name, Is.EqualTo(categoryAddDto.Name));
        }

        [Test]
        public async Task UpdateAsync_ValidCategory_ReturnCategoryDtoSuccessResult()
        {
            // Arrange
            CategoryUpdateDto categoryUpdateDto = new CategoryUpdateDto { Id = 1, Name = "Updated Category", Description = "Updated Description" };
            Category category = new Category { Id = categoryUpdateDto.Id, Name = categoryUpdateDto.Name, Description = categoryUpdateDto.Description };
            _mockUnitOfWork.Setup(x => x.Categories.GetAsync(It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Expression<Func<Category, object>>[]>())).ReturnsAsync(category);
            _mockMapper.Setup(x => x.Map<CategoryUpdateDto, Category>(It.IsAny<CategoryUpdateDto>(), It.IsAny<Category>())).Returns(category);
            _mockUnitOfWork.Setup(x => x.Categories.UpdateAsync(It.IsAny<Category>())).ReturnsAsync(category);

            // Act
            var result = await _categoryManager.UpdateAsync(categoryUpdateDto, "TestUser");

            // Assert
            Assert.That(result.ResultStatus, Is.EqualTo(ResultStatus.Success));
            Assert.That(result.Data.Category.Name, Is.EqualTo(categoryUpdateDto.Name));
        }

        [Test]
        public async Task DeleteAsync_CategoryNotExists_ReturnErrorResult()
        {
            // Arrange

            _mockUnitOfWork.Setup(x => x.Categories.GetAsync(It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Expression<Func<Category, object>>[]>())).ReturnsAsync((Category)null);

            // Act
            var result = await _categoryManager.DeleteAsync(0, "TestUser");

            // Assert
            Assert.That(result.ResultStatus, Is.EqualTo(ResultStatus.Error));
        }
        [Test]
        public async Task DeleteAsync_CategoryExists_ReturnsSuccessResult()
        {
            Article article = new Article();
            // Arrange
            var categoryId = 1;
            var category = new Category { Id = categoryId, Name = "Test", Articles = null};

                _mockUnitOfWork.Setup(x => x.Categories.GetAsync(It.IsAny<Expression<System.Func<Category, bool>>>(),
                It.IsAny<Expression<Func<Category, object>>[]>())).ReturnsAsync(category);

            // Act
            var result = await _categoryManager.DeleteAsync(categoryId, "testUser");

            // Assert
            Assert.That(result.ResultStatus, Is.EqualTo(ResultStatus.Success));
        }
    }
}