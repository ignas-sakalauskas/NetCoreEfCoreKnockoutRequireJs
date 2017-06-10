using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Web.Helpers;

namespace WebApplication1.Tests.Helpers
{
    [TestClass]
    public sealed class EnumExtensionsTests
    {
        private enum TestEnum
        {
            Item1 = 1,
            Item2 = 2
        }

        [TestMethod]
        public void ConvertToJsonDictionary_ShouldThrowArgumentException_WhenTypeIsClass()
        {
            // Arrange
            // Act
            Action action = () => EnumHelpers.ConvertToJsonDictionary<TestClass>();

            // Assert
            action.ShouldThrow<ArgumentException>();
        }

        [TestMethod]
        public void ConvertToJsonDictionary_ShouldThrowArgumentException_WhenTypeIsInt()
        {
            // Arrange
            // Act
            Action action = () => EnumHelpers.ConvertToJsonDictionary<int>();

            // Assert
            action.ShouldThrow<ArgumentException>();
        }

        [TestMethod]
        public void ConvertToJsonDictionary_ShouldReturnEnumSerializedIntoJson_WhenTypeIsEnum()
        {
            // Arrange
            var expectedResult = $"{{\"{TestEnum.Item1}\":{(int)TestEnum.Item1},\"{TestEnum.Item2}\":{(int)TestEnum.Item2}}}";

            // Act
            var result = EnumHelpers.ConvertToJsonDictionary<TestEnum>();

            // Assert
            result.Should().Be(expectedResult);
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class TestClass
        {
        }
    }
}