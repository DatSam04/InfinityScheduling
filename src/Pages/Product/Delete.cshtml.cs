using System.Linq;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// Delete Page
    /// </summary>
    public class DeleteModel : PageModel
    {
        // data middle tier
        public JsonFileProductService ProductService { get; }
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="productService"></param>
        public DeleteModel(JsonFileProductService productService)
        {
            ProductService = productService;
        }
        
        // Current product data
        [BindProperty]
        public ProductModel Product { get; set; }
        // Url of page to return to
        [BindProperty(SupportsGet = true)]
        // Url of page to return to
        public string ReturnUrl {  get; set; }
        /// <summary>
        /// REST Get request
        /// </summary>
        /// <param name="id"></param>
        public void OnGet(string id)
        {
            Product = ProductService.GetProducts().FirstOrDefault(x => x.Id.Equals(id));
        }
        /// <summary>
        /// REST Post request, deletes Product with Product.Id
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(Product?.Id))
            {
                return Page();
            }

            ProductService.DeleteData(Product.Id);

            return RedirectToPage("./Index");
        }
    }
}
