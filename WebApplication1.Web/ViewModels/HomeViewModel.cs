namespace WebApplication1.Web.ViewModels
{
    /// <summary>
    /// Home page view model
    /// </summary>
    public class HomeViewModel
    {
        /// <summary>
        /// Gets or sets JSON string for enum
        /// </summary>
        public string ClientStatusJson { get; set; }

        /// <summary>
        /// Gets or sets client ID from routing params
        /// </summary>
        public int ClientId { get; set; }
    }
}
