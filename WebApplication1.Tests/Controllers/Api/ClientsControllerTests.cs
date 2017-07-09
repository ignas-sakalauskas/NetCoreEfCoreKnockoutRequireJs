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
using WebApplication1.Web.Exceptions;
using WebApplication1.Web.Models;
using WebApplication1.Web.Services;

namespace WebApplication1.Tests.Controllers.Api
{
    [TestClass]
    public sealed class ClientsControllerTests
    {
        private Mock<IClientsDataService> _clientDataServiceMock;
        private Mock<ILoggerFactory> _loggerFactoryMock;

        [TestInitialize]
        public void Init()
        {
            _clientDataServiceMock = new Mock<IClientsDataService>(MockBehavior.Strict);
            _loggerFactoryMock = new Mock<ILoggerFactory>(MockBehavior.Strict);

            var loggerMock = new Mock<ILogger<ClientsController>>();
            _loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(loggerMock.Object);
        }

        [TestMethod]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggerFactoryIsNull()
        {
            // Arrange
            _loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(null as ILogger);

            // Act
            // ReSharper disable once ObjectCreationAsStatement
            Action action = () => new ClientsController(_clientDataServiceMock.Object, _loggerFactoryMock.Object);

            // Assert
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void Constructor_ShouldThrowArgumentNullException_WhenCreateLoggerReturnsNull()
        {
            // Arrange
            // Act
            // ReSharper disable once ObjectCreationAsStatement
            Action action = () => new ClientsController(_clientDataServiceMock.Object, null);

            // Assert
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void Constructor_ShouldThrowArgumentNullException_WhenDataServiceIsNull()
        {
            // Arrange
            // Act
            // ReSharper disable once ObjectCreationAsStatement
            Action action = () => new ClientsController(null, _loggerFactoryMock.Object);

            // Assert
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public async Task Get_ShouldReturnInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            _clientDataServiceMock.Setup(x => x.GetClients())
                .ThrowsAsync(new Exception("Exception"));
            var controller = new ClientsController(_clientDataServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = await controller.Get();

            // Assert
            result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            result.Should().BeOfType<ObjectResult>()
                .Which.Value.ToString().Should().Contain("Exception");
            _clientDataServiceMock.VerifyAll();
        }

        [TestMethod]
        public async Task Get_ShouldReturnOkResult_WhenDataServiceReturnsClients()
        {
            // Arrange
            _clientDataServiceMock.Setup(x => x.GetClients())
                .ReturnsAsync(new List<Client> { new Client() });
            var controller = new ClientsController(_clientDataServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = await controller.Get();

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<List<Client>>()
                .Which.Should().HaveCount(1);
            _clientDataServiceMock.VerifyAll();
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public async Task GetById_ShouldReturnBadResult_WhenIdParamIsInvalid(int id)
        {
            // Arrange
            var controller = new ClientsController(_clientDataServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = await controller.Get(id);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeOfType<string>()
                .Which.Should().Contain("Invalid ID");
        }

        [TestMethod]
        public async Task GetById_ShouldReturnInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            const int clientId = 1;
            _clientDataServiceMock.Setup(x => x.GetClient(It.Is<int>(a => a == clientId)))
                .ThrowsAsync(new Exception("Exception"));
            var controller = new ClientsController(_clientDataServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = await controller.Get(clientId);

            // Assert
            result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            result.Should().BeOfType<ObjectResult>()
                .Which.Value.ToString().Should().Contain("Exception");
            _clientDataServiceMock.VerifyAll();
        }

        [TestMethod]
        public async Task GetById_ShouldReturnNotFoundResult_WhenClientNotFoundExceptionIsThrown()
        {
            // Arrange
            const int clientId = 1;
            _clientDataServiceMock.Setup(x => x.GetClient(It.Is<int>(a => a == clientId)))
                .ThrowsAsync(new ClientNotFoundException("ClientNotFoundException"));
            var controller = new ClientsController(_clientDataServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = await controller.Get(clientId);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>()
                .Which.Value.Should().BeOfType<string>()
                .Which.Should().Contain("ClientNotFoundException");
            _clientDataServiceMock.VerifyAll();
        }

        [TestMethod]
        public async Task GetById_ShouldReturnOkResult_WhenDataServiceReturnsClient()
        {
            // Arrange
            const int clientId = 1;
            _clientDataServiceMock.Setup(x => x.GetClient(It.Is<int>(a => a == clientId)))
                .ReturnsAsync(new Client { ClientId = clientId });
            var controller = new ClientsController(_clientDataServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = await controller.Get(clientId);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<Client>()
                .Which.ClientId.Should().Be(clientId);
            _clientDataServiceMock.VerifyAll();
        }

        [TestMethod]
        public async Task Post_ShouldReturnBadResult_WhenModelInvalid()
        {
            // Arrange
            var controller = new ClientsController(_clientDataServiceMock.Object, _loggerFactoryMock.Object);
            controller.ModelState.AddModelError("test", "test");

            // Act
            var result = await controller.Post(null);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeOfType<string>()
                .Which.Should().Contain("Invalid model");
        }

        [TestMethod]
        public async Task Post_ShouldReturnBadResult_WhenExceptionIsThrown()
        {
            // Arrange
            var client = new Client { ClientId = 1 };
            _clientDataServiceMock.Setup(x => x.AddClient(It.Is<Client>(a => a == client)))
                .ThrowsAsync(new Exception("Exception"));
            var controller = new ClientsController(_clientDataServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = await controller.Post(client);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeOfType<string>()
                .Which.Should().Contain("Exception");
            _clientDataServiceMock.VerifyAll();
        }

        [TestMethod]
        public async Task Post_ShouldReturnOkResult_WhenDataServiceReturnsClient()
        {
            // Arrange
            var client = new Client { ClientId = 1 };
            _clientDataServiceMock.Setup(x => x.AddClient(It.Is<Client>(a => a == client)))
                .ReturnsAsync(client);
            var controller = new ClientsController(_clientDataServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = await controller.Post(client);

            // Assert
            result.Should().BeOfType<CreatedResult>()
                .Which.Value.Should().BeOfType<Client>()
                .Which.Should().Be(client);
            _clientDataServiceMock.VerifyAll();
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public async Task Put_ShouldReturnBadResult_WhenIdParamIsInvalid(int id)
        {
            // Arrange
            var controller = new ClientsController(_clientDataServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = await controller.Put(id, null);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeOfType<string>()
                .Which.Should().Contain("Invalid ID");
        }

        [TestMethod]
        public async Task Put_ShouldReturnBadResult_WhenModelInvalid()
        {
            // Arrange
            var controller = new ClientsController(_clientDataServiceMock.Object, _loggerFactoryMock.Object);
            controller.ModelState.AddModelError("test", "test");

            // Act
            var result = await controller.Put(1, null);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeOfType<string>()
                .Which.Should().Contain("Invalid model");
            _clientDataServiceMock.VerifyAll();
        }

        [TestMethod]
        public async Task Put_ShouldReturnNotFoundResult_WhenClientNotFoundExceptionIsThrown()
        {
            // Arrange
            const int clientId = 1;
            var client = new Client { ClientId = 1 };
            _clientDataServiceMock.Setup(x => x.UpdateClient(It.Is<int>(a => a == clientId), It.Is<Client>(a => a == client)))
                .ThrowsAsync(new ClientNotFoundException("ClientNotFoundException"));
            var controller = new ClientsController(_clientDataServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = await controller.Put(clientId, client);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>()
                .Which.Value.Should().BeOfType<string>()
                .Which.Should().Contain("ClientNotFoundException");
            _clientDataServiceMock.VerifyAll();
        }

        [TestMethod]
        public async Task Put_ShouldReturnBadResult_WhenExceptionIsThrown()
        {
            // Arrange
            const int clientId = 1;
            var client = new Client { ClientId = 1 };
            _clientDataServiceMock.Setup(x => x.UpdateClient(It.Is<int>(a => a == clientId), It.Is<Client>(a => a == client)))
                .ThrowsAsync(new Exception("Exception"));
            var controller = new ClientsController(_clientDataServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = await controller.Put(clientId, client);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeOfType<string>()
                .Which.Should().Contain("Exception");
            _clientDataServiceMock.VerifyAll();
        }

        [TestMethod]
        public async Task Put_ShouldReturnOkResult_WhenDataServiceReturnsClient()
        {
            // Arrange
            const int clientId = 1;
            var client = new Client { ClientId = 1 };
            _clientDataServiceMock.Setup(x => x.UpdateClient(It.Is<int>(a => a == clientId), It.Is<Client>(a => a == client)))
                .ReturnsAsync(client);
            var controller = new ClientsController(_clientDataServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = await controller.Put(clientId, client);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<Client>()
                .Which.Should().Be(client);
            _clientDataServiceMock.VerifyAll();
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public async Task Delete_ShouldReturnBadResult_WhenIdParamIsInvalid(int id)
        {
            // Arrange
            var controller = new ClientsController(_clientDataServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = await controller.Delete(id);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeOfType<string>()
                .Which.Should().Contain("Invalid ID");
        }

        [TestMethod]
        public async Task Delete_ShouldReturnNotFoundResult_WhenClientNotFoundExceptionIsThrown()
        {
            // Arrange
            const int clientId = 1;
            _clientDataServiceMock.Setup(x => x.DeleteClient(It.Is<int>(a => a == clientId)))
                .Throws(new ClientNotFoundException("ClientNotFoundException"));
            var controller = new ClientsController(_clientDataServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = await controller.Delete(clientId);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>()
                .Which.Value.Should().BeOfType<string>()
                .Which.Should().Contain("ClientNotFoundException");
            _clientDataServiceMock.VerifyAll();
        }

        [TestMethod]
        public async Task Delete_ShouldReturnBadResult_WhenExceptionIsThrown()
        {
            // Arrange
            const int clientId = 1;
            _clientDataServiceMock.Setup(x => x.DeleteClient(It.Is<int>(a => a == clientId)))
                .Throws(new Exception("Exception"));
            var controller = new ClientsController(_clientDataServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = await controller.Delete(clientId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeOfType<string>()
                .Which.Should().Contain("Exception");
            _clientDataServiceMock.VerifyAll();
        }

        [TestMethod]
        public async Task Delete_ShouldReturnNoContent_WhenDataServiceCalled()
        {
            // Arrange
            const int clientId = 1;
            _clientDataServiceMock.Setup(x => x.DeleteClient(It.Is<int>(a => a == clientId)))
                .Returns(Task.CompletedTask);
            var controller = new ClientsController(_clientDataServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = await controller.Delete(clientId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _clientDataServiceMock.VerifyAll();
        }
    }
}
