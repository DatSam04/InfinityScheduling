using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;


namespace ContosoCrafts.WebSite.Controllers
{
    /// <summary>
    /// This controller is used to manage products
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="productService"></param>
        public ProductsController(JsonFileProductService productService)
        {
            ProductService = productService;
        }

        // This is the service that provides access to the product data
        public JsonFileProductService ProductService { get; }

        /// <summary>
        /// This method returns a list of products.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<ProductModel> Get()
        {
            return ProductService.GetProducts();
        }
        
        /// <summary>
        /// This method updates the rating of a product.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPatch]
        public ActionResult Patch([FromBody] RatingRequest request)
        {
            ProductService.AddRating(request.ProductId, request.Rating);
            
            return Ok();
        }

        /// <summary>
        /// This class is used to represent a rating request.
        /// </summary>
        public class RatingRequest
        {
            // Property for getting and setting product's id
            public string ProductId { get; set; }

            // The product's rating
            public int Rating { get; set; }
        }
    }
}