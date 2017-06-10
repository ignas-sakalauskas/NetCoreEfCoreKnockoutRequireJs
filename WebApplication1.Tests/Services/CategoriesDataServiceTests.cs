using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Web.DatabaseContext;
using WebApplication1.Web.Models;
using WebApplication1.Web.Services;

namespace WebApplication1.Tests.Services
{
    [TestClass]
    public sealed class CategoriesDataServiceTests
    {
        private DbContextOptions _dbContextOptions;

        [TestInitialize]
        public void Init()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            // Make sure each test gets a clean database
            using (var context = new AppDbContext(_dbContextOptions))
            {
                context.Database.EnsureDeleted();
            }
        }

        [TestCleanup]
        public void Finish()
        {
            // Delete database after each test
            using (var context = new AppDbContext(_dbContextOptions))
            {
                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public void Constructor_ShouldThrowArgumentNullException_WhenAppDbContextIsNull()
        {
            // Arrange
            // Act
            // ReSharper disable once ObjectCreationAsStatement
            Action action = () => new CategoriesDataService(null);

            // Assert
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public async Task GetCategories_ShouldReturnTwoCategories_WhenTwoCategoriesInDatabase()
        {
            // Arrange
            using (var context = new AppDbContext(_dbContextOptions))
            {
                context.Categories.AddRange(new Category(), new Category());
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var service = new CategoriesDataService(context);
                var result = await service.GetCategories();

                // Assert
                result.Should().HaveCount(2);
            }
        }

        [TestMethod]
        public void AddCategory_ShouldThrowArgumentNullException_WhenCategoryIsNull()
        {
            // Arrange
            // Act
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var service = new CategoriesDataService(context);
                Func<Task> action = async () => await service.AddCategory(null);

                // Assert
                action.ShouldThrow<ArgumentNullException>();
            }
        }

        [TestMethod]
        public async Task AddCategory_ShouldReturnAddedCategory_WhenCategorySaved()
        {
            // Arrange
            const string expectedCategoryName = "name";

            // Act
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var service = new CategoriesDataService(context);
                var result = await service.AddCategory(new Category { Name = expectedCategoryName });

                // Assert
                result.Name.Should().Be(expectedCategoryName);
            }
        }

        [TestMethod]
        public async Task AddCategory_ShouldBlankCategoryId_WhenCategoryIdProvided()
        {
            // Arrange
            const int categoryId = 999;

            // Act
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var service = new CategoriesDataService(context);
                var result = await service.AddCategory(new Category { CategoryId = categoryId });

                // Assert
                result.CategoryId.Should().NotBe(categoryId);
            }
        }
    }
}
