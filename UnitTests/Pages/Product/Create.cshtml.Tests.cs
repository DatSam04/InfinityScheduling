using System.Linq;
using System.Threading.Tasks;
using ContosoCrafts.WebSite.Pages.Product;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NUnit.Framework;

namespace UnitTests.Pages.Product
{
    /// <summary>
    /// Provides unit testing for the Create page
    /// </summary>
    public class ProductCreateTests
    {
        #region TestSetup

        // Declare the model of the Create page to be used in unit tests
        public static CreateModel pageModel;

        [SetUp]
        /// <summary>
        /// Initializes mock Create page model for testing.
        /// </summary>
        public void TestInitialize()
        {
            pageModel = new CreateModel(TestHelper.ProductService);
        }

        #endregion TestSetup
        
        #region OnGet
        // OnGet on Create should test functionality of a Create of a specific product

        /// <summary>
        /// Return a Product with a given Id.
        /// We will create a Record, and then search for it
        /// 
        /// </summary>
        
        [Test]
        public void OnGet_Creates_Valid_Empty_Product_Returns_true()
        {
            // Arrange 

            // Act
            pageModel.OnGet();

            // Assert
            Assert.AreEqual(true, pageModel.ModelState.IsValid);

            return;
        }
        #endregion OnGet
        
        #region OnPost
        /// <summary>
        /// Test for OnPost with invalid model state 
        /// </summary>
        [Test]
        public void OnPost_Invalid_ModelState_Returns_Page_Completed_Successfully_True()
        {
            // Arrange
            // Act
            pageModel.ModelState.AddModelError("", "Error");
            var result = pageModel.OnPost();
            // Assert
            Assert.AreEqual(true, result.IsCompletedSuccessfully);
            // Reset
            pageModel.ModelState.Clear();
        }   
        /// <summary>
        /// Test for posting with an invalid image url
        /// </summary>
        [Test]
        public async Task OnPost_Invalid_Image_Url_Returns_Invalid_ModelState()
        {
            // Arrange
            pageModel.OnGet(); // create a new empty product
            // Act
            pageModel.Product.Image = "invalid dummy url"; // invalid image url
            var result = await pageModel.OnPost();
            // Assert
            Assert.AreEqual(false, pageModel.ModelState.IsValid);
            // Reset
            pageModel.ModelState.Clear();
        }
        
        /// <summary>
        /// Test that posts with a valid image url
        /// </summary>
        [Test]
        public async Task OnPost_Valid_Image_Url_Creates_Product_Valid_ModelState()
        {
            // Arrange
            pageModel.OnGet(); // create a new empty product
            var createdId = pageModel.Product.Id;
            // Act
            var validImageUrl = "https://images.unsplash.com/photo-1502175353174-a7a70e73b362?q=80&w=3226&auto=format&fit=crop&ixlib=" + 
                                "rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D";
            pageModel.Product.Image = validImageUrl; // change to a valid Image url;
            var result = await pageModel.OnPost();
            // Assert
            
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
            // Reset
        }
        
        #endregion OnPost
        
        #region IsImageUrlAsync
        /// <summary>
        /// Test that an invalid image url returning bad status code returns false
        /// </summary>
        [Test]
        public async Task IsImageUrlAsync_Invalid_Image_Url_Returns_False()
        {
            // Arrange
            // Act
            var url = "https://httpstat.us/404";
            var result = await pageModel.IsImageUrlAsync(url); //return non-success status
            // Assert
            Assert.AreEqual(false, result);
        }
        #endregion IsImageUrlAsync
    }
}