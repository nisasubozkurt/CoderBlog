using NUnit.Framework;
using ProgrammersBlog.Entities.Concrete;
using System.Collections.Generic;

namespace BirimTest.EntityTest
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void User_ConstructedCorrectly()
        {
            // Arrange
            var user = new User
            {
                Picture = "user.jpg",
                Articles = new List<Article>(), // Empty list for now
                About = "About John Doe",
                FirstName = "John",
                LastName = "Doe",
                YoutubeLink = "https://www.youtube.com/user/johndoe",
                TwitterLink = "https://twitter.com/johndoe",
                InstagramLink = "https://www.instagram.com/johndoe",
                FacebookLink = "https://www.facebook.com/johndoe",
                LinkedInLink = "https://www.linkedin.com/in/johndoe",
                GitHubLink = "https://github.com/johndoe",
                WebsiteLink = "https://www.johndoe.com"
            };

            // Assert
            Assert.AreEqual("user.jpg", user.Picture);
            Assert.IsNotNull(user.Articles);
            Assert.IsInstanceOf<List<Article>>(user.Articles);
            Assert.AreEqual("About John Doe", user.About);
            Assert.AreEqual("John", user.FirstName);
            Assert.AreEqual("Doe", user.LastName);
            Assert.AreEqual("https://www.youtube.com/user/johndoe", user.YoutubeLink);
            Assert.AreEqual("https://twitter.com/johndoe", user.TwitterLink);
            Assert.AreEqual("https://www.instagram.com/johndoe", user.InstagramLink);
            Assert.AreEqual("https://www.facebook.com/johndoe", user.FacebookLink);
            Assert.AreEqual("https://www.linkedin.com/in/johndoe", user.LinkedInLink);
            Assert.AreEqual("https://github.com/johndoe", user.GitHubLink);
            Assert.AreEqual("https://www.johndoe.com", user.WebsiteLink);
        }

        // Add more tests for other properties and scenarios if needed...
    }
}
