using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Web.Constants;

namespace WebApplication1.Web.Models
{
    /// <summary>
    /// Category model
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Gets or sets category ID
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets Name
        /// </summary>
        [MaxLength(Validation.MaxLength)]
        [MinLength(Validation.MinLength)]
        [Required]
        public string Name { get; set; }
    }
}
