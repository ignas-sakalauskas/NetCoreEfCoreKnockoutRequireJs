using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Web.Enums;
using WebApplication1.Web.Models;

namespace WebApplication1.Tests.Models
{
    [TestClass]
    public sealed class ClientTests
    {
        private const string OneHundredAndOneChar = "01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";
        private Client _client;

        [TestInitialize]
        public void Init()
        {
            // create default valid model
            _client = new Client
            {
                ClientId = 1,
                Name = "123",
                Email = "email@email.com",
                Status = ClientStatus.Active,
                Fax = "1",
                Phone = "2",
                Address = "3"
            };
        }

        [TestMethod]
        public void Client_ShouldBeInvalid_WhenRequiredFieldsAreNullOrEmpty()
        {
            // Arrange
            var model = new Client(); // default new empty object
            var context = new ValidationContext(model, null, null);
            var results = new List<ValidationResult>();

            // Act
            var isModelStateValid = Validator.TryValidateObject(model, context, results, true);

            // Assert
            isModelStateValid.Should().BeFalse();
        }

        [DataTestMethod]
        [DataRow("a")]
        [DataRow("aa")]
        public void Client_ShouldBeInvalid_WhenNameIsShorterThanSpecified(string name)
        {
            // Arrange
            _client.Name = name;

            var context = new ValidationContext(_client, null, null);
            var results = new List<ValidationResult>();

            // Act
            var isModelStateValid = Validator.TryValidateObject(_client, context, results, true);

            // Assert
            isModelStateValid.Should().BeFalse();
        }

        [DataTestMethod]
        [DataRow("a")]
        [DataRow("aa.aa")]
        [DataRow("email-at-email.com")]
        public void Client_ShouldBeInvalid_WhenEmailDoesntMatchRegex(string email)
        {
            // Arrange
            _client.Email = email;
            var context = new ValidationContext(_client, null, null);
            var results = new List<ValidationResult>();

            // Act
            var isModelStateValid = Validator.TryValidateObject(_client, context, results, true);

            // Assert
            isModelStateValid.Should().BeFalse();
        }

        [TestMethod]
        public void Client_ShouldBeInvalid_WhenNameIsLongerThanSpecified()
        {
            // Arrange
            _client.Name = OneHundredAndOneChar;
            var context = new ValidationContext(_client, null, null);
            var results = new List<ValidationResult>();

            // Act
            var isModelStateValid = Validator.TryValidateObject(_client, context, results, true);

            // Assert
            isModelStateValid.Should().BeFalse();
        }

        [TestMethod]
        public void Client_ShouldBeInvalid_WhenEmailIsLongerThanSpecified()
        {
            // Arrange
            _client.Email = OneHundredAndOneChar;
            var context = new ValidationContext(_client, null, null);
            var results = new List<ValidationResult>();

            // Act
            var isModelStateValid = Validator.TryValidateObject(_client, context, results, true);

            // Assert
            isModelStateValid.Should().BeFalse();
        }

        [TestMethod]
        public void Client_ShouldBeInvalid_WhenAddressIsLongerThanSpecified()
        {
            // Arrange
            _client.Address = OneHundredAndOneChar;
            var context = new ValidationContext(_client, null, null);
            var results = new List<ValidationResult>();

            // Act
            var isModelStateValid = Validator.TryValidateObject(_client, context, results, true);

            // Assert
            isModelStateValid.Should().BeFalse();
        }

        [TestMethod]
        public void Client_ShouldBeInvalid_WhenPhoneIsLongerThanSpecified()
        {
            // Arrange
            _client.Phone = OneHundredAndOneChar;
            var context = new ValidationContext(_client, null, null);
            var results = new List<ValidationResult>();

            // Act
            var isModelStateValid = Validator.TryValidateObject(_client, context, results, true);

            // Assert
            isModelStateValid.Should().BeFalse();
        }

        [TestMethod]
        public void Client_ShouldBeInvalid_WhenFaxIsLongerThanSpecified()
        {
            // Arrange
            _client.Phone = OneHundredAndOneChar;
            var context = new ValidationContext(_client, null, null);
            var results = new List<ValidationResult>();

            // Act
            var isModelStateValid = Validator.TryValidateObject(_client, context, results, true);

            // Assert
            isModelStateValid.Should().BeFalse();
        }

        [TestMethod]
        public void Client_ShouldBeValid_WhenModelIsValid()
        {
            // Arrange
            var context = new ValidationContext(_client, null, null);
            var results = new List<ValidationResult>();

            // Act
            var isModelStateValid = Validator.TryValidateObject(_client, context, results, true);

            // Assert
            isModelStateValid.Should().BeTrue();
        }
    }
}
