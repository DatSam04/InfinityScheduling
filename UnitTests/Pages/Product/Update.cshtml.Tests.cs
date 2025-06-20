using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using NUnit.Framework;

using ContosoCrafts.WebSite.Pages.Product;
using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UnitTests.Pages.Product
{
    /// <summary>
    /// Provides unit testing for the Update page
    /// </summary>
    public class ProductUpdateTests
    {
        #region TestSetup
        // Declare the model of the Update page to be used in unit tests
        public static UpdateModel pageModel;

        
        /// <summary>
        /// Initializes mock Update page model for testing.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            pageModel = new UpdateModel(TestHelper.ProductService);
        }

        #endregion TestSetup

        #region OnGet
        /// <summary>
        /// Test if ReturnUrl has value
        /// </summary>
        [Test]
        public void ReturnUrl_Valid_Get_Set()
        {
            //Arrange
            //Act
            pageModel.ReturnUrl = "/Product/Index";
            
            //Assert
            Assert.AreEqual("/Product/Index", pageModel.ReturnUrl);
        }
        
        /// <summary>
        /// Test OnGet with a valid Id 
        /// </summary>
        [Test]
        public void OnGet_Valid_Id_Should_Return_Valid_Product()
        {
            // Arrange
            // Act
            pageModel.OnGet("jenlooper-cactus");
            
            // Assert
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
            Assert.AreEqual("jenlooper-cactus", pageModel.Product.Id);
            // Reset
            // This should remove the error we added
            pageModel.ModelState.Clear();

        }


        
        /// <summary>
        /// Test that's loading the update page returns an invalid state
        /// </summary>
        [Test]
        public void OnGet_Invalid_Id_Should_Return_Null_Product()
        {
            // Arrange

            // Act
            pageModel.OnGet("mike-clown1");  // Should not find
            // Assert
            Assert.AreEqual(null, pageModel.Product);
            Assert.AreEqual(true, pageModel.ModelState.IsValid);

            // Reset
            pageModel.ModelState.Clear();

        }
        #endregion OnGet

        #region OnPost
        /// <summary>
        /// Test that checks update functionality
        /// </summary>
        [Test]
        public async Task OnPost_Valid_Should_Return_Products()
        {
            // Arrange

            // Act
            pageModel.OnGet("jenlooper-cactus");
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
            var originalDescription = pageModel.Product.Description;

            // change Value of Product Description to Microsoft and update
            pageModel.Product.Description = "Microsoft";
            var result = await pageModel.OnPost();

            // Assert to see that post succeeeded
            Assert.AreEqual(true, pageModel.ModelState.IsValid);

            // Read it to see if it changed
            pageModel.OnGet("jenlooper-cactus");

            // Assertions to verify
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
            Assert.AreEqual("Microsoft", pageModel.Product.Description);

            // Reset it back
            pageModel.Product.Description = originalDescription;
            result = await pageModel.OnPost();
            // Assert to see that post succeeded
            Assert.AreEqual(true, pageModel.ModelState.IsValid);

            // Reset 
            pageModel.ModelState.Clear();

        }
        
        /// <summary>
        /// Test that checks update functionality's error state
        /// </summary>
        [Test]
        public void OnPost_InValid_Model_NotValid_Return_Page()
        {
            // Arrange

            // Force an invalid error state
            pageModel.ModelState.AddModelError("bogus", "bogus error");

            // Act
            // Store the ActionResult of the post? TODO: better understand this line of code or ask professor
            var result = pageModel.OnPost();
            // Store whether the ModelState is valid for later assert
            var stateIsValid = pageModel.ModelState.IsValid;

            // Assert
            Assert.AreEqual(false, stateIsValid);

            // Reset
            // This should remove the error we added
            pageModel.ModelState.Clear();
        }


        
        /// <summary>
        /// Test that's posting an invalid Image Url returns Invalid ModelState
        /// </summary>
        [Test]
        public void OnPost_Invalid_ImageUrl_Should_Return_Invalid_ModelState()
        {
            // Arrange

            // Act
            pageModel.OnGet("jenlooper-cactus");
            Assert.AreEqual(true, pageModel.ModelState.IsValid);

            pageModel.Product.Image = "not an image url"; // post with an invalid image url
            var result = pageModel.OnPost();

            // Store whether the ModelState is valid for later assert
            var stateIsValid = pageModel.ModelState.IsValid;

            // Assert
            Assert.AreEqual(false, stateIsValid);

            // Reset
            // This should remove the error we added
            pageModel.ModelState.Clear();

        }
        
        /// <summary>
        /// Test that posts a valid Image Url and updates a product
        /// </summary>
        [Test]
        public async Task OnPost_Valid_ImageUrl_Should_Return_Valid_ModelState()
        {
            // Arrange
            // Act
            pageModel.OnGet("jenlooper-cactus");
            pageModel.Product.Image =
                "https://images.unsplash.com/photo-1502175353174-a7a70e73b362?q=80&w=3226&auto=format&fit=crop&ixlib=" +
                "rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"; // change to a valid Image url
            var result = await pageModel.OnPost();
            
            var stateIsValid = pageModel.ModelState.IsValid;
            // Assert
            Assert.AreEqual(true, stateIsValid);
        }
        #endregion OnPost
        
        #region IsImageUrlAsync
        /// <summary>
        /// Test that the https request work with both valid and non-valid URL
        /// </summary>
        [Test]
        public async Task IsImageUrlAsync_InValid_Should_Return_False()
        {
            // Arrange
            // Act
            var url = "https://httpstat.us/404";
            var result = await pageModel.IsImageUrlAsync(url); //return non-success status
            
            var url2 = "https://images.unsplash.com/photo-1502175353174-a7a70e73b362?q=80&w=3226&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D";
            var result2 = await pageModel.IsImageUrlAsync(url2); //return success status
            
            //Assert
            Assert.AreEqual(false, result);
            Assert.AreEqual(true, result2);
            
            //Reset
            pageModel.ModelState.Clear();
        }
        
        #endregion IsImageUrlAsync
    }
}