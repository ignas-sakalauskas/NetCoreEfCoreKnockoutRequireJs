using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Web.Models;

namespace WebApplication1.Web.Services
{
    /// <summary>
    /// Data Service for working with Client data
    /// </summary>
    public interface IClientsDataService
    {
        /// <summary>
        /// Get all client records
        /// </summary>
        /// <returns>List of Client objects</returns>
        Task<IList<Client>> GetClients();

        /// <summary>
        /// Get client record by ID
        /// </summary>
        /// <param name="id">Client ID</param>
        /// <returns>Client object</returns>
        Task<Client> GetClient(int id);

        /// <summary>
        /// Adds client record to database
        /// </summary>
        /// <param name="client">Client object</param>
        /// <returns>Added client object</returns>
        Task<Client> AddClient(Client client);

        /// <summary>
        /// Updates client with the Client object provided
        /// </summary>
        /// <param name="id">Client ID</param>
        /// <param name="client">Client object to updat with</param>
        /// <returns>Updated client object</returns>
        Task<Client> UpdateClient(int id, Client client);

        /// <summary>
        /// Deletes client record
        /// </summary>
        /// <param name="id">Client ID</param>
        /// <returns>Void</returns>
        Task DeleteClient(int id);
    }
}
