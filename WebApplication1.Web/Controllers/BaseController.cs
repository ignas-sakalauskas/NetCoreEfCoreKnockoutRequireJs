using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Web.Controllers
{
    /// <summary>
    /// Base controller for all non-api controllers
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BaseController : Controller
    {
    }
}
