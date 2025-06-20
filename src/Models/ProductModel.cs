using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ContosoCrafts.WebSite.Models
{
    /// <summary>
    /// This class represents a product model.
    /// </summary>

    public class ProductModel
    {
        // Product Id that is randomly generated
        public string Id { get; set; }
        // Product maker
        public string Maker { get; set; }

        // This property is used to store the image URL of the product.
        [JsonPropertyName("img")]
        [Required(ErrorMessage ="Image URL is required")] // To validate the image URL
        [Url(ErrorMessage = "Please enter a valid image URL")] // To validate the image URL
        // Product Image
        public string Image { get; set; } 

        [Required(ErrorMessage = "URL is required")] // To validate the URL
        [Url(ErrorMessage = "Please enter a valid URL")] 
        // This property is used to store the URL of the product.
        public string Url { get; set; } 

        [Required(ErrorMessage = "Title is required")]  // To validate the title
        // Product Title
        public string Title { get; set; } 
        
        [Required(ErrorMessage = "Description is required")] // To validate the description
        // This property is used to store the description of the product.
        public string Description { get; set; } 
        // This property is used to store the ratings of the product.
        public int[] Ratings { get; set; } 

        // This property is used to store the average rating of the product.
        public override string ToString() => JsonSerializer.Serialize<ProductModel>(this);

 
    }
}