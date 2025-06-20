using System.Net.Http;
using System.Threading.Tasks;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// Create Page
    /// </summary>
    public class CreateModel : PageModel
    {
        // Data middle tier
        public JsonFileProductService ProductService { get; }
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="productService"></param>
        public CreateModel(JsonFileProductService productService)
        {
            ProductService = productService;
        }
        // Current product data
        [BindProperty]
        public ProductModel Product { get; set; }
        /// <summary>
        /// REST Get request
        /// </summary>
        public void OnGet()
        {
            Product = new ProductModel()
            {
                Id = System.Guid.NewGuid().ToString(),
                Title = "",
                Description = "",
                Maker = "",
                Url = "",
                Image = "",
                Ratings = null
            };
        }
        /// <summary>
        /// REST Post request, returns Controller Action
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if(!await IsImageUrlAsync(Product.Image))
            {
                ModelState.AddModelError("Product.Image", "The URL must point to a valid image.");
                return Page();
            }
            Product.Id = System.Guid.NewGuid().ToString();
            Product.Maker = "Infinity";
            Product.Ratings = null;
            ProductService.CreateData(Product);
            return RedirectToPage("./Index");
        }

        /// <summary>
        /// IsImageUrlAsync method is used to verify if the input valid url is an image
        /// </summary>
        public async Task<bool> IsImageUrlAsync(string imageUrl)
        {
            try
            {
                using var httpClient = new HttpClient();
                //Pass the url to HttpMethod.Head request to check the content type of this metadata
                using var request = new HttpRequestMessage(HttpMethod.Head, imageUrl);
                using var response = await httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                    return false;
                
                var contentType = response.Content.Headers.ContentType?.MediaType;
                //Accept url image that have query string after image file extension
                return contentType != null && contentType.StartsWith("image/");
            }
            catch
            {
                return false;
            }
        }
    }
}
