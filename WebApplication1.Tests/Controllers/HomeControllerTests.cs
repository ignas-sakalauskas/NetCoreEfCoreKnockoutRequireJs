using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Web.Controllers;
using WebApplication1.Web.ViewModels;

namespace WebApplication1.Tests.Controllers
{
    [TestClass]
    public sealed class HomeControllerTests
    {
        [TestMethod]
        public void Index_ShouldReturnIndexView_Always()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Index();

            // Assert
            result.Should().BeOfType<ViewResult>()
                .Which.ViewName.Should().Be("Index");
        }

        [TestMethod]
        public void Edit_ShouldReturnEditView_Always()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Edit(0);

            // Assert
            result.Should().BeOfType<ViewResult>()
                .Which.ViewName.Should().Be("Edit");
        }

        [TestMethod]
        public void Edit_ShouldReturnNonEmptyEnumJsonInModel_Always()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Edit(0);

            // Assert
            ((ViewResult)result).Model.Should().BeOfType<HomeViewModel>()
                .Which.ClientStatusJson.Should().NotBeNullOrWhiteSpace();
        }

        [TestMethod]
        public void Edit_ShouldPassIdParamIntoViewModel_Always()
        {
            // Arrange
            const int clientId = 1;
            var controller = new HomeController();

            // Act
            var result = controller.Edit(clientId);

            // Assert
            ((ViewResult)result).Model.Should().BeOfType<HomeViewModel>()
                .Which.ClientId.Should().Be(clientId);
        }

        [TestMethod]
        public void Add_ShouldReturnAddView_Always()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Add();

            // Assert
            result.Should().BeOfType<ViewResult>()
                .Which.ViewName.Should().Be("Add");
            result.Should().BeOfType<ViewResult>().Which.Model.Should().BeOfType<HomeViewModel>().And.NotBeNull();
        }

        [TestMethod]
        public void Add_ShouldReturnNonEmptyEnumJsonInModel_Always()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Add();

            // Assert
            ((ViewResult)result).Model.Should().BeOfType<HomeViewModel>()
                .Which.ClientStatusJson.Should().NotBeNullOrWhiteSpace();
        }
    }
}
