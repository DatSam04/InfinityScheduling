using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// Update Page
    /// </summary>
    public class UpdateModel : PageModel
    {
        // Data middletier
        public JsonFileProductService ProductService { get; }
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="productService"></param>
        public UpdateModel(JsonFileProductService productService)
        {
            ProductService = productService;
        }
        
        // Product data to show
        [BindProperty]
        public ProductModel Product { get; set; }

        // Allow Razor to bind ?returnUrl=/Product/Read
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }
        /// <summary>
        /// REST Get request, sets Product with given id
        /// </summary>
        /// <param name="id"></param>
        public void OnGet(string id)
        {
            Product = ProductService.GetProducts().FirstOrDefault(x => x.Id.Equals(id));
        }

        /// <summary>
        /// OnPost method used to call the Post API method to change data in products.json
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            if(!await IsImageUrlAsync(Product.Image))
            {
                ModelState.AddModelError("Product.Image", "The URL must point to a valid image.");
                return Page();
            }
            ProductService.UpdateData(Product);
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

                if (response.IsSuccessStatusCode == false)
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
