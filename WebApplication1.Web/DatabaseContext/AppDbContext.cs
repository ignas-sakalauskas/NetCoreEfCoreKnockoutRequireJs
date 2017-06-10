using Microsoft.EntityFrameworkCore;
using WebApplication1.Web.Models;

namespace WebApplication1.Web.DatabaseContext
{
    /// <summary>
    /// Application Database context
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options">Database context options</param>
        public AppDbContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets clients entity
        /// </summary>
        public virtual DbSet<Client> Clients { get; set; }

        /// <summary>
        /// Gets or sets categories entity
        /// </summary>
        public virtual DbSet<Category> Categories { get; set; }
    }
}
