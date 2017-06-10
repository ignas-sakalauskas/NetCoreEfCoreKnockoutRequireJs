using Microsoft.AspNetCore.Mvc;
using WebApplication1.Web.Enums;
using WebApplication1.Web.Helpers;
using WebApplication1.Web.ViewModels;

namespace WebApplication1.Web.Controllers
{
    /// <summary>
    /// Main application controller, mostly used for routing
    /// </summary>
    public class HomeController : BaseController
    {
        /// <summary>
        /// Home page
        /// </summary>
        /// <returns>View only</returns>
        public IActionResult Index()
        {
            return View("Index");
        }

        /// <summary>
        /// Client add page
        /// </summary>
        /// <returns>View with model</returns>
        public IActionResult Add()
        {
            var model = PopulateViewModelWithEnumValues();

            return View("Add", model);
        }

        /// <summary>
        /// Client edit page
        /// </summary>
        /// <param name="id">Client ID</param>
        /// <returns>View with model</returns>
        public IActionResult Edit(int id)
        {
            var model = PopulateViewModelWithEnumValues();
            model.ClientId = id;

            return View("Edit", model);
        }

        /// <summary>
        /// Populates model with JSON string of serialized ClientStatus enum values
        /// </summary>
        /// <returns>Home View Model</returns>
        private static HomeViewModel PopulateViewModelWithEnumValues()
        {
            return new HomeViewModel
            {
                ClientStatusJson = EnumHelpers.ConvertToJsonDictionary<ClientStatus>()
            };
        }
    }
}
