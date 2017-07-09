using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApplication1.Web.Controllers.Api;
using WebApplication1.Web.Models;
using WebApplication1.Web.Services;

namespace WebApplication1.Tests.Controllers.Api
{
    [TestClass]
    public sealed class CategoriesControllerTests
    {
        private Mock<ICategoriesDataService> _categoriesDataServiceMock;
        private Mock<ILoggerFactory> _loggerFactoryMock;

        [TestInitialize]
        public void Init()
        {
            _categoriesDataServiceMock = new Mock<ICategoriesDataService>(MockBehavior.Strict);
            _loggerFactoryMock = new Mock<ILoggerFactory>(MockBehavior.Strict);

            var loggerMock = new Mock<ILogger<CategoriesController>>();
            _loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(loggerMock.Object);
        }

        [TestMethod]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggerFactoryIsNull()
        {
            // Arrange
            _loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(null as ILogger);

            // Act
            // ReSharper disable once ObjectCreationAsStatement
            Action action = () => new CategoriesController(_categoriesDataServiceMock.Object, _loggerFactoryMock.Object);

            // Assert
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void Constructor_ShouldThrowArgumentNullException_WhenCreateLoggerReturnsNull()
        {
            // Arrange
            // Act
            // ReSharper disable once ObjectCreationAsStatement
            Action action = () => new CategoriesController(_categoriesDataServiceMock.Object, null);

            // Assert
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void Constructor_ShouldThrowArgumentNullException_WhenDataServiceIsNull()
        {
            // Arrange
            // Act
            // ReSharper disable once ObjectCreationAsStatement
            Action action = () => new CategoriesController(null, _loggerFactoryMock.Object);

            // Assert
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public async Task Get_ShouldReturnInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            _categoriesDataServiceMock.Setup(x => x.GetCategories())
                .ThrowsAsync(new Exception("Exception"));
            var controller = new CategoriesController(_categoriesDataServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = await controller.Get();

            // Assert
            result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            result.Should().BeOfType<ObjectResult>()
                .Which.Value.ToString().Should().Contain("Exception");
            _categoriesDataServiceMock.VerifyAll();
        }

        [TestMethod]
        public async Task Get_ShouldReturnOkResult_WhenDataServiceReturnsCategories()
        {
            // Arrange
            _categoriesDataServiceMock.Setup(x => x.GetCategories())
                .ReturnsAsync(new List<Category> { new Category() });
            var controller = new CategoriesController(_categoriesDataServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = await controller.Get();

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<List<Category>>()
                .Which.Should().HaveCount(1);
            _categoriesDataServiceMock.VerifyAll();
        }

        [TestMethod]
        public async Task Post_ShouldReturnBadResult_WhenModelInvalid()
        {
            // Arrange
            var controller = new CategoriesController(_categoriesDataServiceMock.Object, _loggerFactoryMock.Object);
            controller.ModelState.AddModelError("test", "test");

            // Act
            var result = await controller.Post(null);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeOfType<string>()
                .Which.Should().Contain("Invalid model");
        }

        [TestMethod]
        public async Task Post_ShouldReturnInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var category = new Category { CategoryId = 1 };
            _categoriesDataServiceMock.Setup(x => x.AddCategory(It.Is<Category>(a => a == category)))
                .ThrowsAsync(new Exception("Exception"));
            var controller = new CategoriesController(_categoriesDataServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = await controller.Post(category);

            // Assert
            result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            result.Should().BeOfType<ObjectResult>()
                .Which.Value.ToString().Should().Contain("Exception");
            _categoriesDataServiceMock.VerifyAll();
        }

        [TestMethod]
        public async Task Post_ShouldReturnOkResult_WhenDataServiceReturnsCategory()
        {
            // Arrange
            var category = new Category { CategoryId = 1 };
            _categoriesDataServiceMock.Setup(x => x.AddCategory(It.Is<Category>(a => a == category)))
                .ReturnsAsync(category);
            var controller = new CategoriesController(_categoriesDataServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = await controller.Post(category);

            // Assert
            result.Should().BeOfType<CreatedResult>()
                .Which.Value.Should().BeOfType<Category>()
                .Which.Should().Be(category);
            _categoriesDataServiceMock.VerifyAll();
        }
    }
}
