using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication1.Web.Exceptions;
using WebApplication1.Web.Helpers;
using WebApplication1.Web.Models;
using WebApplication1.Web.Services;

namespace WebApplication1.Web.Controllers.Api
{
    /// <summary>
    /// Main API controller to manage client records
    /// </summary>
    [Route("api/[controller]")]
    public class ClientsController : Controller
    {
        private readonly IClientsDataService _clientsDataService;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientsController"/> class.
        /// </summary>
        /// <param name="clientsDataService">Clients data service</param>
        /// <param name="loggerFactory">Logger factory</param>
        public ClientsController(IClientsDataService clientsDataService, ILoggerFactory loggerFactory)
        {
            _clientsDataService = clientsDataService ?? throw new ArgumentNullException(nameof(clientsDataService));
            _logger = loggerFactory?.CreateLogger(nameof(ClientsController)) ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        /// <summary>
        /// Gets all clients
        /// </summary>
        /// <returns>HTTP response. List of clients if successful.</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var clients = await _clientsDataService.GetClients();

                return Ok(clients);
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Gets one client by ID
        /// </summary>
        /// <param name="id">Client ID</param>
        /// <returns>HTTP response. Client if successful.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest($"Invalid ID: {id}");
                }

                var client = await _clientsDataService.GetClient(id);

                return Ok(client);
            }
            catch (ClientNotFoundException e)
            {
                _logger.LogError(0, e, e.Message);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Creates a new client
        /// </summary>
        /// <param name="client">Client object</param>
        /// <returns>HTTP response. Created client if successful.</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Client client)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(FormatHelpers.FormatValidationErrorMessage(ModelState.Values));
                }

                var addedClient = await _clientsDataService.AddClient(client);

                return Created("/", addedClient);
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, e.Message);
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Updates existing client by ID
        /// </summary>
        /// <param name="id">Client ID</param>
        /// <param name="client">Client object</param>
        /// <returns>HTTP response. Updated client if successful.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Client client)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid ID.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(FormatHelpers.FormatValidationErrorMessage(ModelState.Values));
                }

                var updatedClient = await _clientsDataService.UpdateClient(id, client);

                return Ok(updatedClient);
            }
            catch (ClientNotFoundException e)
            {
                _logger.LogError(0, e, e.Message);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, e.Message);
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Deletes client by ID
        /// </summary>
        /// <param name="id">Client ID</param>
        /// <returns>HTTP response. No Content response if successful.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid ID.");
                }

                await _clientsDataService.DeleteClient(id);

                return NoContent();
            }
            catch (ClientNotFoundException e)
            {
                _logger.LogError(0, e, e.Message);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}
