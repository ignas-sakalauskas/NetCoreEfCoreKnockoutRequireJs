using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Web.Constants;
using WebApplication1.Web.Enums;

namespace WebApplication1.Web.Models
{
    /// <summary>
    /// Client model
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Gets or sets client ID
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ClientId { get; set; }

        /// <summary>
        /// Gets or sets Name
        /// </summary>
        [MaxLength(Validation.MaxLength)]
        [MinLength(Validation.MinLength)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Address
        /// </summary>
        [MaxLength(Validation.MaxLength)]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets Phone
        /// </summary>
        [MaxLength(Validation.MaxLength)]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets Fax
        /// </summary>
        [MaxLength(Validation.MaxLength)]
        public string Fax { get; set; }

        /// <summary>
        /// Gets or sets Email
        /// Regex looksfor @ char only
        /// </summary>
        [RegularExpression(@"^\S+@\S+$")]
        [MaxLength(Validation.MaxLength)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets Status
        /// </summary>
        [Required]
        public ClientStatus Status { get; set; }

        /// <summary>
        /// Gets or sets CreatedOn
        /// </summary>
        [Required]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets Category
        /// </summary>
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets category navigation property
        /// </summary>
        public virtual Category Category { get; set; }
    }
}
