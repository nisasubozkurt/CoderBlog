using NUnit.Framework;
using ProgrammersBlog.Entities.Concrete;

namespace VeritabaniTesti.EntityTest
{
    [TestFixture]
    public class CommentTests
    {
        [Test]
        public void Comment_ConstructedCorrectly()
        {
            // Arrange
            var article = new Article
            {
                Id = 1,
                Title = "Test Article",
                Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                // Set other properties as needed
            };

            // Act
            var comment = new Comment
            {
                Text = "This is a test comment",
                ArticleId = article.Id,
                Article = article
            };

            // Assert
            Assert.AreEqual("This is a test comment", comment.Text);
            Assert.AreEqual(1, comment.ArticleId);
            Assert.AreEqual(article, comment.Article);
        }

        // Add more tests for other properties and scenarios if needed...
    }
}
