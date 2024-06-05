
using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Mvc.Controllers;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos;
using Microsoft.Extensions.Options;
using NToastNotify;
using System.Threading.Tasks;
using ProgrammersBlog.Shared.Utilities.Helpers.Abstract;
using ProgrammersBlog.Shared.Utilities.Results.Concrete;
using ProgrammersBlog.Shared.Utilities.Results.Abstract;
using System.Reflection.PortableExecutable;
using ProgrammersBlog.Tests;

namespace Entegrasyon.UserControllerTest
{
    [TestFixture]
    public class HomeControllerTests
    {
        private Mock<IArticleService> _mockArticleService;
        private Mock<IOptionsSnapshot<AboutUsPageInfo>> _mockAboutUsOptions;
        private Mock<IMailService> _mockMailService;
        private Mock<IToastNotification> _mockToastNotification;
        private Mock<IWritableOptions<AboutUsPageInfo>> _mockAboutUsPageInfoWriter;
        private HomeController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockArticleService = new Mock<IArticleService>();
            _mockAboutUsOptions = new Mock<IOptionsSnapshot<AboutUsPageInfo>>();
            _mockMailService = new Mock<IMailService>();
            _mockToastNotification = new Mock<IToastNotification>();
            _mockAboutUsPageInfoWriter = new Mock<IWritableOptions<AboutUsPageInfo>>();

            _controller = new HomeController(
                _mockArticleService.Object,
                _mockAboutUsOptions.Object,
                _mockMailService.Object,
                _mockToastNotification.Object,
                _mockAboutUsPageInfoWriter.Object
            );
        }

        [Test]
        public void Contact_Get_ReturnsViewResult()
        {
            // Act
            var result = _controller.Contact();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public void Contact_Post_InvalidModel_ReturnsViewWithModel()
        {
            // Arrange
            _controller.ModelState.AddModelError("Email", "Email is required");
            EmailSendDto emailSendDto = new EmailSendDto();

            // Act
            var result = _controller.Contact(emailSendDto);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult.Model, Is.EqualTo(emailSendDto));
        }

        [Test]
        public void Contact_Post_ValidModel_ReturnsView(){
            EmailSendDto emailSendDto = new EmailSendDto { Email = "test@example.com",
                                                           Subject = "Test", 
                                                           Message = "Test message" };
            _mockMailService.Setup(service => service.SendContactEmail(It.IsAny<EmailSendDto>()))
                            .Returns(new EmailSendResultDto { IsSuccessful = true, 
                                                              Message = "Success" });
            var result = _controller.Contact(emailSendDto);
            Assert.That(result, Is.TypeOf<ViewResult>());
            _mockToastNotification.Verify(x => x.AddSuccessToastMessage(It.IsAny<string>(),
                                               It.IsAny<ToastrOptions>()), Times.Once);
        }
    }

}
