using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ContosoCrafts.WebSite.Pages
{
    /// <summary>
    /// My Schedule Page
    /// </summary>
    public class MyScheduleModel : PageModel
    {
        // logger to help monitor application behavior
        private readonly ILogger<MyScheduleModel> _logger;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="logger"></param>
        public MyScheduleModel(ILogger<MyScheduleModel> logger)
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
