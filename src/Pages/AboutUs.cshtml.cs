using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ContosoCrafts.WebSite.Pages
{
    /// <summary>
    /// About Us Page
    /// </summary>
    public class AboutUsModel : PageModel
    {
        // logger for monitoring application behavior
        private readonly ILogger<AboutUsModel> _logger;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="logger"></param>
        public AboutUsModel(ILogger<AboutUsModel> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// REST Get request
        /// </summary>
        public void OnGet()
        {
        }
    }
}
