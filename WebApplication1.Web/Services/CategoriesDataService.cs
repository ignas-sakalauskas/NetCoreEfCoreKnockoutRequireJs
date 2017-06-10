using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Web.DatabaseContext;
using WebApplication1.Web.Models;

namespace WebApplication1.Web.Services
{
    /// <summary>
    /// Data Service for working with Category data
    /// </summary>
    public class CategoriesDataService : ICategoriesDataService
    {
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesDataService"/> class.
        /// </summary>
        /// <param name="dbContext">Application Database context</param>
        public CategoriesDataService(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Get all category records
        /// </summary>
        /// <returns>List of Category objects</returns>
        public async Task<IList<Category>> GetCategories()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        /// <summary>
        /// Adds category record to database
        /// </summary>
        /// <param name="category">Category object</param>
        /// <returns>Added category object</returns>
        public async Task<Category> AddCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            // Blank the ID to make sure a new record is created
            category.CategoryId = 0;

            var result = await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            return result.Entity;
        }
    }
}
