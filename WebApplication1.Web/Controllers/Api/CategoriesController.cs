using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication1.Web.Helpers;
using WebApplication1.Web.Models;
using WebApplication1.Web.Services;

namespace WebApplication1.Web.Controllers.Api
{
    /// <summary>
    /// API controller to manage category records
    /// </summary>
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly ICategoriesDataService _categoriesDataService;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesController"/> class.
        /// </summary>
        /// <param name="categoriesDataService">Categories data service</param>
        /// <param name="loggerFactory">Logger factory</param>
        public CategoriesController(ICategoriesDataService categoriesDataService, ILoggerFactory loggerFactory)
        {
            _categoriesDataService = categoriesDataService ?? throw new ArgumentNullException(nameof(categoriesDataService));
            _logger = loggerFactory?.CreateLogger(nameof(CategoriesController)) ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <returns>HTTP response. List of clients if successful.</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var clients = await _categoriesDataService.GetCategories();

                return Ok(clients);
            }
            catch (Exception e)
            {
                _logger.LogError(2, e, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Creates a new category
        /// </summary>
        /// <param name="category">Category object</param>
        /// <returns>HTTP response. Created category if successful.</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Category category)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(FormatHelpers.FormatValidationErrorMessage(ModelState.Values));
                }

                var addedClient = await _categoriesDataService.AddCategory(category);

                return Created("/", addedClient);
            }
            catch (Exception e)
            {
                _logger.LogError(2, e, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
