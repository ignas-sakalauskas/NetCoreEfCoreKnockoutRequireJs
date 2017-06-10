using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Web.Models;

namespace WebApplication1.Web.Services
{
    /// <summary>
    /// Data Service for working with Category data
    /// </summary>
    public interface ICategoriesDataService
    {
        /// <summary>
        /// Get all category records
        /// </summary>
        /// <returns>List of Category objects</returns>
        Task<IList<Category>> GetCategories();

        /// <summary>
        /// Adds category record to database
        /// </summary>
        /// <param name="category">Category object</param>
        /// <returns>Added category object</returns>
        Task<Category> AddCategory(Category category);
    }
}
