using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ContosoCrafts.WebSite.Pages
{
    /// <summary>
    /// My Product Page
    /// </summary>
    public class MyProductModel : PageModel
    {
        // logger to help monitor application behavior
        private readonly ILogger<MyProductModel> _logger;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="logger"></param>
        public MyProductModel(ILogger<MyProductModel> logger)
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
