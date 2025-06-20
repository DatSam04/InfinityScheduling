using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite.Pages
{
    /// <summary>
    /// Jeffrey Li
    /// Gayathri Poluri
    /// Dat Boi Sam
    /// </summary>
    
    /// <summary>
    /// Index Page
    /// </summary>
    public class IndexModel : PageModel
    {
        // logger to monitor application behavior
        private readonly ILogger<IndexModel> _logger;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="productService"></param>
        public IndexModel(ILogger<IndexModel> logger,
            JsonFileProductService productService)
        {
            _logger = logger;
            ProductService = productService;
        }

        // data middle tier
        public JsonFileProductService ProductService { get; }
        // list of all products
        public IEnumerable<ProductModel> Products { get; private set; }

        // REST Get request
        public void OnGet()
        {
            Products = ProductService.GetProducts();
        }
    }
}