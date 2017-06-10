using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Web.Models;

namespace WebApplication1.Tests.Models
{
    [TestClass]
    public sealed class CategoryTests
    {
        private const string OneHundredAndOneChar = "01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";
        private Category _category;

        [TestInitialize]
        public void Init()
        {
            // create default valid model
            _category = new Category
            {
                CategoryId = 1,
                Name = "123"
            };
        }

        [TestMethod]
        public void Category_ShouldBeInvalid_WhenRequiredFieldsAreNullOrEmpty()
        {
            // Arrange
            var model = new Category(); // default new empty object
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
        public void Category_ShouldBeInvalid_WhenNameIsShorterThanSpecified(string name)
        {
            // Arrange
            _category.Name = name;

            var context = new ValidationContext(_category, null, null);
            var results = new List<ValidationResult>();

            // Act
            var isModelStateValid = Validator.TryValidateObject(_category, context, results, true);

            // Assert
            isModelStateValid.Should().BeFalse();
        }

        [TestMethod]
        public void Category_ShouldBeInvalid_WhenNameIsLongerThanSpecified()
        {
            // Arrange
            _category.Name = OneHundredAndOneChar;
            var context = new ValidationContext(_category, null, null);
            var results = new List<ValidationResult>();

            // Act
            var isModelStateValid = Validator.TryValidateObject(_category, context, results, true);

            // Assert
            isModelStateValid.Should().BeFalse();
        }

        [TestMethod]
        public void Category_ShouldBeValid_WhenModelIsValid()
        {
            // Arrange
            var context = new ValidationContext(_category, null, null);
            var results = new List<ValidationResult>();

            // Act
            var isModelStateValid = Validator.TryValidateObject(_category, context, results, true);

            // Assert
            isModelStateValid.Should().BeTrue();
        }
    }
}
