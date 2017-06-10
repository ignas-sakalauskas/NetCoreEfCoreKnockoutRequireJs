using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Web.DatabaseContext;
using WebApplication1.Web.Exceptions;
using WebApplication1.Web.Models;

namespace WebApplication1.Web.Services
{
    /// <summary>
    /// Data Service for working with Client data
    /// </summary>
    public class ClientsDataService : IClientsDataService
    {
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientsDataService"/> class.
        /// </summary>
        /// <param name="dbContext">Application Database context</param>
        public ClientsDataService(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Gets all client records
        /// </summary>
        /// <returns>List of Client objects</returns>
        public async Task<IList<Client>> GetClients()
        {
            return await _dbContext.Clients
                .Include(c => c.Category)
                .ToListAsync();
        }

        /// <summary>
        /// Gets client record by ID
        /// </summary>
        /// <param name="id">Client ID</param>
        /// <returns>Client object</returns>
        public async Task<Client> GetClient(int id)
        {
            var client = await _dbContext.Clients
                .Where(c => c.ClientId == id)
                .Include(c => c.Category)
                .SingleOrDefaultAsync();

            if (client == null)
            {
                throw new ClientNotFoundException($"Client with ID='{id}' not found.");
            }

            return client;
        }

        /// <summary>
        /// Adds client record to database
        /// </summary>
        /// <param name="client">Client object</param>
        /// <returns>Added client object</returns>
        public async Task<Client> AddClient(Client client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            // Blank the ID to make sure a new record is created
            client.ClientId = 0;

            // Set createdOn date
            client.CreatedOn = DateTime.Now;

            var result = await _dbContext.Clients.AddAsync(client);
            await _dbContext.SaveChangesAsync();

            return result.Entity;
        }

        /// <summary>
        /// Updates client with the Client object provided
        /// </summary>
        /// <param name="id">Client ID</param>
        /// <param name="client">Client object to updat with</param>
        /// <returns>Updated client object</returns>
        public async Task<Client> UpdateClient(int id, Client client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            var existingClient = await _dbContext.Clients.SingleOrDefaultAsync(q => q.ClientId == id);
            if (existingClient == null)
            {
                throw new ClientNotFoundException($"Client with ID='{id}' not found.");
            }

            // Update properties
            existingClient.Name = client.Name;
            existingClient.Address = client.Address;
            existingClient.Email = client.Email;
            existingClient.Fax = client.Fax;
            existingClient.Phone = client.Phone;
            existingClient.Status = client.Status;
            existingClient.CategoryId = client.CategoryId;

            var result = _dbContext.Clients.Update(existingClient);
            await _dbContext.SaveChangesAsync();

            return result.Entity;
        }

        /// <summary>
        /// Deletes client record
        /// </summary>
        /// <param name="id">Client ID</param>
        /// <returns>Void</returns>
        public async Task DeleteClient(int id)
        {
            var client = await _dbContext.Clients.SingleOrDefaultAsync(q => q.ClientId == id);
            if (client == null)
            {
                throw new ClientNotFoundException($"Client with ID='{id}' not found.");
            }

            _dbContext.Clients.Remove(client);
            await _dbContext.SaveChangesAsync();
        }
    }
}
