using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Web.DatabaseContext;
using WebApplication1.Web.Enums;
using WebApplication1.Web.Exceptions;
using WebApplication1.Web.Models;
using WebApplication1.Web.Services;

namespace WebApplication1.Tests.Services
{
    [TestClass]
    public sealed class ClientsDataServiceTests
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
            Action action = () => new ClientsDataService(null);

            // Assert
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public async Task GetClients_ShouldReturnTwoClients_WhenTwoClientsFoundInDatabase()
        {
            // Arrange
            using (var context = new AppDbContext(_dbContextOptions))
            {
                context.Clients.AddRange(new Client(), new Client());
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var service = new ClientsDataService(context);
                var result = await service.GetClients();

                // Assert
                result.Should().HaveCount(2);
            }
        }

        [TestMethod]
        public async Task GetClients_ShouldIncludeCategoriesWithClients_WhenCategoriesAssigned()
        {
            // Arrange
            using (var context = new AppDbContext(_dbContextOptions))
            {
                context.Clients.Add(new Client { Category = new Category { Name = "a" } });
                context.Clients.Add(new Client { Category = new Category { Name = "b" } });
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var service = new ClientsDataService(context);
                var result = await service.GetClients();

                // Assert
                result.First().Category.Name.Should().Be("a");
                result.Last().Category.Name.Should().Be("b");
            }
        }

        [TestMethod]
        public void GetClient_ShouldThrowClientNotFoundException_WhenClientNotFound()
        {
            // Arrange
            // Act
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var service = new ClientsDataService(context);
                Func<Task> action = async () => await service.GetClient(0);

                // Assert
                action.ShouldThrow<ClientNotFoundException>();
            }
        }

        [TestMethod]
        public async Task GetClient_ShouldReturnClient_WhenClientFound()
        {
            // Arrange
            const int expectedClientId = 2;
            using (var context = new AppDbContext(_dbContextOptions))
            {
                context.Clients.Add(new Client { ClientId = 1 });
                context.Clients.Add(new Client { ClientId = expectedClientId });
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var service = new ClientsDataService(context);
                var result = await service.GetClient(expectedClientId);

                // Assert
                result.ClientId.Should().Be(expectedClientId);
            }
        }

        [TestMethod]
        public async Task GetClient_ShouldIncludeCategory_WhenClientFound()
        {
            // Arrange
            using (var context = new AppDbContext(_dbContextOptions))
            {
                context.Clients.Add(new Client { ClientId = 1, Category = new Category { Name = "a" } });
                context.Clients.Add(new Client { ClientId = 2, Category = new Category { Name = "b" } });
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var service = new ClientsDataService(context);
                var result = await service.GetClient(2);

                // Assert
                result.Category.Name.Should().Be("b");
            }
        }

        [TestMethod]
        public void AddClient_ShouldThrowArgumentNullException_WhenClientIsNull()
        {
            // Arrange
            // Act
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var service = new ClientsDataService(context);
                Func<Task> action = async () => await service.AddClient(null);

                // Assert
                action.ShouldThrow<ArgumentNullException>();
            }
        }

        [TestMethod]
        public async Task AddClient_ShouldReturnAddedClient_WhenClientSaved()
        {
            // Arrange
            const string expectedClientName = "name";

            // Act
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var service = new ClientsDataService(context);
                var result = await service.AddClient(new Client { Name = expectedClientName });

                // Assert
                result.Name.Should().Be(expectedClientName);
            }
        }

        [TestMethod]
        public async Task AddClient_ShouldAddCreatedOnDate_WhenClientSaved()
        {
            // Arrange
            // Act
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var service = new ClientsDataService(context);
                var result = await service.AddClient(new Client { CreatedOn = DateTime.MinValue });

                // Assert
                result.CreatedOn.Should().NotBe(DateTime.MinValue);
            }
        }

        [TestMethod]
        public async Task AddClient_ShouldBlankClientId_WhenClientIdProvided()
        {
            // Arrange
            const int clientId = 999;

            // Act
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var service = new ClientsDataService(context);
                var result = await service.AddClient(new Client { ClientId = clientId });

                // Assert
                result.ClientId.Should().NotBe(clientId);
            }
        }

        [TestMethod]
        public void UpdateClient_ShouldThrowArgumentNullException_WhenClientIsNull()
        {
            // Arrange
            // Act
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var service = new ClientsDataService(context);
                Func<Task> action = async () => await service.UpdateClient(0, null);

                // Assert
                action.ShouldThrow<ArgumentNullException>();
            }
        }

        [TestMethod]
        public void UpdateClient_ShouldThrowClientNotFoundException_WhenClientNotFound()
        {
            // Arrange
            using (var context = new AppDbContext(_dbContextOptions))
            {
                context.Clients.AddRange(new Client(), new Client());
                context.SaveChanges();
            }

            // Act
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var service = new ClientsDataService(context);
                Func<Task> action = async () => await service.UpdateClient(0, new Client());

                // Assert
                action.ShouldThrow<ClientNotFoundException>();
            }
        }

        [TestMethod]
        public async Task UpdateClient_ShouldUpdateAllClientProperties_WhenClientObjectProvided()
        {
            // Arrange
            const int clientId = 1;
            var expectedClient = new Client
            {
                ClientId = 1,
                Name = "1",
                Status = ClientStatus.Active,
                Address = "1",
                Email = "1",
                Fax = "1",
                Phone = "1"
            };

            using (var context = new AppDbContext(_dbContextOptions))
            {
                await context.Clients.AddAsync(new Client { ClientId = clientId });
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var service = new ClientsDataService(context);
                var result = await service.UpdateClient(clientId, expectedClient);

                // Assert
                result.ShouldBeEquivalentTo(expectedClient);
            }
        }

        [TestMethod]
        public async Task UpdateClient_ShouldIgnoreClientIdInObject_WhenClientObjectHasIdSet()
        {
            // Arrange
            const int clientId = 1;
            var inputClient = new Client
            {
                ClientId = 111
            };

            using (var context = new AppDbContext(_dbContextOptions))
            {
                await context.Clients.AddAsync(new Client { ClientId = clientId });
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var service = new ClientsDataService(context);
                var result = await service.UpdateClient(clientId, inputClient);

                // Assert
                result.ClientId.Should().Be(clientId);
            }
        }

        [TestMethod]
        public void DeleteClient_ShouldThrowClientNotFoundException_WhenClientNotFound()
        {
            // Arrange
            using (var context = new AppDbContext(_dbContextOptions))
            {
                context.Clients.AddRange(new Client(), new Client());
                context.SaveChanges();
            }

            // Act
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var service = new ClientsDataService(context);
                Func<Task> action = async () => await service.DeleteClient(0);

                // Assert
                action.ShouldThrow<ClientNotFoundException>();
            }
        }

        [TestMethod]
        public async Task DeleteClient_ShouldDeleteClient_WhenClientExists()
        {
            // Arrange
            const int clientId = 1;
            using (var context = new AppDbContext(_dbContextOptions))
            {
                await context.Clients.AddAsync(new Client { ClientId = clientId });
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var service = new ClientsDataService(context);
                await service.DeleteClient(clientId);
            }

            // Assert
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var result = await context.Clients.SingleOrDefaultAsync(o => o.ClientId == clientId);
                result.Should().BeNull();
            }
        }
    }
}
