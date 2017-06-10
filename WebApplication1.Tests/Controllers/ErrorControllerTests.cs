using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApplication1.Web.Controllers;

namespace WebApplication1.Tests.Controllers
{
    [TestClass]
    public sealed class ErrorControllerTests
    {
        private Mock<ILoggerFactory> _loggerFactoryMock;

        [TestInitialize]
        public void Init()
        {
            _loggerFactoryMock = new Mock<ILoggerFactory>(MockBehavior.Strict);

            var loggerMock = new Mock<ILogger<ErrorController>>();
            _loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(loggerMock.Object);
        }

        [TestMethod]
        public void Index_ShouldReturnIndexView_Always()
        {
            // Arrange
            var controller = new ErrorController(_loggerFactoryMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

            // Act
            var result = controller.Index();

            // Assert
            result.Should().BeOfType<ViewResult>()
                .Which.ViewName.Should().Be("Index");
        }

        [TestMethod]
        public void Index404_ShouldReturnNotFoundView_Always()
        {
            // Arrange
            var controller = new ErrorController(_loggerFactoryMock.Object);

            // Act
            var result = controller.Index(404);

            // Assert
            result.Should().BeOfType<ViewResult>()
                .Which.ViewName.Should().Be("NotFound");
        }

        [DataTestMethod]
        [DataRow(400)]
        [DataRow(500)]
        public void IndexOther_ShouldReturnNotFoundView_Always(int code)
        {
            // Arrange
            var controller = new ErrorController(_loggerFactoryMock.Object);

            // Act
            var result = controller.Index(code);

            // Assert
            result.Should().BeOfType<ViewResult>()
                .Which.ViewName.Should().Be("Unknown");
        }
    }
}
