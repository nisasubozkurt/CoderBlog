using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using ProgrammersBlog.Entities.Concrete;

namespace BirimTest.EntityTest
{
    [TestFixture]
    public class ArticleTest
    {
        [Test]
        public void Article_ConstructedCorrectly()
        {
            // Arrange
            var categoryId = 1;
            var userId = 1;

            // Act
            var article = new Article
            {
                Title = "Test Article",
                Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                Thumbnail = "thumbnail.jpg",
                Date = DateTime.Now,
                ViewCount = 10,
                CommentCount = 5,
                SeoAuthor = "John Doe",
                SeoDescription = "This is a test article",
                SeoTags = "test, article",
                CategoryId = categoryId,
                UserId = userId,
                Comments = new List<Comment>() // Empty list for now
            };

            // Assert
            Assert.AreEqual("Test Article", article.Title);
            Assert.AreEqual("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", article.Content);
            Assert.AreEqual("thumbnail.jpg", article.Thumbnail);
            Assert.AreEqual(DateTime.Now.Date, article.Date.Date);
            Assert.AreEqual(10, article.ViewCount);
            Assert.AreEqual(5, article.CommentCount);
            Assert.AreEqual("John Doe", article.SeoAuthor);
            Assert.AreEqual("This is a test article", article.SeoDescription);
            Assert.AreEqual("test, article", article.SeoTags);
            Assert.AreEqual(categoryId, article.CategoryId);
            Assert.AreEqual(userId, article.UserId);
            Assert.IsNotNull(article.Comments);
            Assert.IsInstanceOf<List<Comment>>(article.Comments);
        }

        // Add more tests for other properties and scenarios if needed...
    }
}
