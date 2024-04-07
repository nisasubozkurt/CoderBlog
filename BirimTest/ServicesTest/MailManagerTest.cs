
using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Options;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Services.Concrete;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.Shared.Utilities.Results.Abstract;
using ProgrammersBlog.Shared.Utilities.Results.ComplexTypes;

namespace BirimTest.ServicesTest
{
    [TestFixture]
    public class MailManagerTests
    {
        private readonly Mock<IOptions<SmtpSettings>> _smtpSettingsMock = new();
        private MailManager _mailManager;
        private SmtpSettings _smtpSettings = new SmtpSettings
        {
            SenderEmail = "traversalcore61@gmail.com",
            Server = "smtp.gmail.com",
            Port = 587,
            Username = "traversalcore61@gmail.com",
            Password = "cvstoxepwdiciwpz"
        };

        [SetUp]
        public void Setup()
        {
            _smtpSettingsMock.Setup(s => s.Value).Returns(_smtpSettings);
            _mailManager = new MailManager(_smtpSettingsMock.Object);
        }

        [Test]
        public void Send_ShouldReturnSuccess_WhenMailIsSent()
        {
            var emailSendDto = new EmailSendDto
            {
                Email = "nisasubozkurt@gmail.com",
                Subject = "Test Subject",
                Message = "Test message"
            };

            var result = _mailManager.Send(emailSendDto);

            Assert.That(result.ResultStatus, Is.EqualTo(ResultStatus.Success));
            Assert.That(result.Message, Does.Contain("başarıyla"));
        }

        [Test]
        public void SendContactEmail_ShouldReturnSuccess_WhenMailIsSent()
        {
            var emailSendDto = new EmailSendDto
            {
                Name = "Su Bozkurt",
                Email = "nisasubozkurt@gmail.com",
                Subject = "Contact Subject",
                Message = "Contact message"
            };

            var result = _mailManager.SendContactEmail(emailSendDto);

            Assert.That(result.ResultStatus, Is.EqualTo(ResultStatus.Success));
            Assert.That(result.Message, Does.Contain("başarıyla"));
        }

        [Test]
        public void Send_ShouldFail_WhenEmailIsInvalid()
        {
            var emailSendDto = new EmailSendDto
            {
                Email = "not-an-email",
                Subject = "Test Subject",
                Message = "Test message"
            };

            IResult result = null;

            Assert.Throws<System.FormatException>(() => result = _mailManager.Send(emailSendDto));
            Assert.IsNull(result);
        }
    }
}
