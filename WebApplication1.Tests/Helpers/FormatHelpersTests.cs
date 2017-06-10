using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Web.Helpers;

namespace WebApplication1.Tests.Helpers
{
    [TestClass]
    public sealed class FormatHelpersTests
    {
        [TestMethod]
        public void FormatValidationErrorMessage_ShouldThrowArgumentNullException_WhenParamIsNull()
        {
            // Arrange
            // Act
            Action action = () => FormatHelpers.FormatValidationErrorMessage(null);

            // Assert
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void FormatValidationErrorMessage_ShouldReturnInitialMessage_WhenListIsEmpty()
        {
            // Arrange
            IEnumerable<ModelStateEntry> list = new List<ModelStateEntry>();

            // Act
            var result = FormatHelpers.FormatValidationErrorMessage(list);

            // Assert
            result.Should().Be($"Invalid model. Errors:{Environment.NewLine}");
        }
    }
}
