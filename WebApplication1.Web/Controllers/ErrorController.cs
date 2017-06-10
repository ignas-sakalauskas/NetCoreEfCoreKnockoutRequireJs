using System;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Web.Controllers
{
    /// <summary>
    /// Error controller handles application errors
    /// </summary>
    [Route("[controller]")]
    public class ErrorController : BaseController
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorController"/> class.
        /// </summary>
        /// <param name="loggerFactory">Logger factory</param>
        public ErrorController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory?.CreateLogger(nameof(ErrorController)) ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        /// <summary>
        /// Default error handler
        /// </summary>
        /// <returns>View with generic error message.</returns>
        [Route("")]
        public IActionResult Index()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (exception != null)
            {
                _logger.LogError(1, exception.Error, "Unhandled exception occurred.");
            }

            return View("Index");
        }

        /// <summary>
        /// HTTP status code based error handler
        /// </summary>
        /// <param name="httpStatusCode">Error HTTP status code</param>
        /// <returns>View with more specific error message.</returns>
        [Route("{httpStatusCode}")]
        public IActionResult Index(int httpStatusCode)
        {
            _logger.LogError(1, $"An error with status code {httpStatusCode} occurred.");

            switch (httpStatusCode)
            {
                case (int)HttpStatusCode.NotFound:
                    return View("NotFound");
                default:
                    return View("Unknown");
            }
        }
    }
}
