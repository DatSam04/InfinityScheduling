using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ContosoCrafts.WebSite.Pages
{
    /// <summary>
    /// Privacy Page
    /// </summary>
    public class PrivacyModel : PageModel
    {
        // logger to help monitor application behavior
        private readonly ILogger<PrivacyModel> _logger;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="logger"></param>
        public PrivacyModel(ILogger<PrivacyModel> logger)
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
