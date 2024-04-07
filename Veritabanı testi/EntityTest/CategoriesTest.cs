using NUnit.Framework;
using ProgrammersBlog.Entities.Concrete;
using System.Collections.Generic;

namespace VeritabaniTesti.EntityTest
{
    [TestFixture]
    public class CategoriesTest
    {
        [Test]
        public void Category_ConstructedCorrectly()
        {
            // Arrange
            var category = new Category
            {
                Name = "Technology",
                Description = "Category for technology-related articles",
                Articles = new List<Article>(), // Empty list for now
            };

            // Assert
            Assert.AreEqual("Technology", category.Name);
            Assert.AreEqual("Category for technology-related articles", category.Description);
            Assert.IsNotNull(category.Articles);
            Assert.IsInstanceOf<List<Article>>(category.Articles);
        }

        // Add more tests for other properties and scenarios if needed...
    }
}
