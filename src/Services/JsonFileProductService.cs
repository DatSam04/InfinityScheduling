using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

using ContosoCrafts.WebSite.Models;

using Microsoft.AspNetCore.Hosting;

namespace ContosoCrafts.WebSite.Services
{
    /// <summary>
    /// This class is used to manage the product data in a JSON file.
    /// </summary>
    public class JsonFileProductService
    {
        /// <summary>
        /// Constructor, sets initial WebHostEnvironment
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        public JsonFileProductService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        // This property is used to get the web host environment.
        public IWebHostEnvironment WebHostEnvironment { get; }

        /// <summary>
        /// This method is used to get the path of the JSON file.
        /// </summary>
        private string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "products.json"); }
        }

        /// <summary>
        /// This method is used to get the list of products from the JSON file.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductModel> GetProducts()
        {
            // Check if the file exists, if not create it
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {
                var products = JsonSerializer.Deserialize<ProductModel[]>(jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                // Sort Product List by Title
                return products;
            }
        }

        /// <summary>
        /// This method is used to add a rating to a product.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="rating"></param>
        /// <returns></returns>
        public bool AddRating(string productId, int rating)
        {
            var products = GetProducts();

            var selectedProduct = products.FirstOrDefault(x => x.Id == productId);
            
            // Check if product with given productId exists
            if (selectedProduct == null)
            {
                return false;
            }
            // Check if the product's ratings exists
            if (selectedProduct.Ratings == null)
            {
                selectedProduct.Ratings = new int[] { rating };
            }

            else //If the product exists, add the rating to the existing ratings
            {
                var ratings = selectedProduct.Ratings.ToList();
                ratings.Add(rating);
                selectedProduct.Ratings = ratings.ToArray();
            }

            // Write the updated list back to the JSON file
            using (var outputStream = File.Create(JsonFileName)) //Create method truncate file
            {
                JsonSerializer.Serialize<IEnumerable<ProductModel>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {
                        SkipValidation = true,
                        Indented = true
                    }), 
                    products
                );
            }

            return true;
        }

        /// <summary>
        /// This method is for updating an updated product in the database
        /// </summary>
        /// <param name="updatedProduct"></param>
        /// <returns></returns>
        public bool UpdateData(ProductModel updatedProduct)
        {
            var isValidUpdate = false;
            var products = GetProducts().ToList();

            var index = products.FindIndex(p => p.Id == updatedProduct.Id);
            if(index != -1)
            {
                //Update the field that display to user only
                products[index].Title = updatedProduct.Title;
                products[index].Description = updatedProduct.Description;
                products[index].Url = updatedProduct.Url;
                products[index].Image = updatedProduct.Image;

                //Write the updated list back to the JSON file
                using (var outputStream = File.Create(JsonFileName))
                {
                    JsonSerializer.Serialize<IEnumerable<ProductModel>>(
                        new Utf8JsonWriter(outputStream, new JsonWriterOptions
                        {
                            SkipValidation = true,
                            Indented = true
                        }),
                        products
                    );
                }
                isValidUpdate = true;
            }
            
            return isValidUpdate;
        }

        /// <summary>
        /// This method is used to create a new product.
        /// </summary>
        /// <param name="newProduct"></param>
        /// <returns></returns>
        public ProductModel CreateData(ProductModel newProduct)
        {
            Console.WriteLine(newProduct);
            var dataSet = GetProducts();
            dataSet = dataSet.Append(newProduct);

            SaveData(dataSet);

            return newProduct;
        }

        /// <summary>
        /// This method is used to delete a product.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProductModel DeleteData(string id)
        {
            //Get the current list of subject, and append the new record after delete the selected subject
            var dataSet = GetProducts();
            var data = dataSet.FirstOrDefault(m => m.Id.Equals(id));

            var newDataSet = GetProducts().Where(m => m.Id.Equals(id) == false);

            SaveData(newDataSet);

            return data;
        }

        /// <summary>
        /// This method is used to save the product data to the JSON file.
        /// </summary>
        /// <param name="products"></param>
        public void SaveData(IEnumerable<ProductModel> products)
        {
            using (var outputStream = File.Create(JsonFileName))
            {
                JsonSerializer.Serialize<IEnumerable<ProductModel>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {
                        SkipValidation = true,
                        Indented = true
                    }),
                    products
                );
            }
        }
    }
}