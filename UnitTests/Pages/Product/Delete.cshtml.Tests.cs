using ContosoCrafts.WebSite.Pages.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NUnit.Framework;

namespace UnitTests.Pages.Product
{
    /// <summary>
    /// Provides unit testing for the Delete page
    /// </summary>
    public class ProductDeleteTests
    {
        #region TestSetup

        // Declare the model of the Delete page to be used in unit tests
        public static DeleteModel pageModel;

        
        /// <summary>
        /// Initializes mock Delete page model for testing.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            pageModel = new DeleteModel(TestHelper.ProductService);
        }

        #endregion TestSetup
        
        #region OnGet
        /// <summary>
        /// Test returns true if get and set for the page model's ReturnUrl are working correctly
        /// </summary>
        [Test]
        public void ReturnUrl_Valid_Get_Set()
        {
            // Arrange
            // Act
            pageModel.ReturnUrl = "./Index";
            // Assert
            Assert.AreEqual("./Index", pageModel.ReturnUrl);
        }
        /// <summary>
        /// Test returns true if the page model's OnGet method returns a valid model state
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_Products()
        {
            // Arrange
            
            // Act
            pageModel.OnGet("jenlooper-cactus");
            // Assert
            // check valid Model State
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
        }
        #endregion OnGet
        
        #region OnPost
        /// <summary>
        /// Test returns true if posting with a null or empty string returns an ActionResult
        /// </summary>
        [Test]
        public void OnPost_NullOrEmpty_String_Returns_ActionResult_True()
        {
            // Arrange
            
            // Act

            var result = pageModel.OnPost();
            
            // Assert
            var answer = result is ActionResult; 
            Assert.AreEqual(true, answer);
        }
        /// <summary>
        /// Test returns true if OnPost with a valid ProductId redirects to the index page
        /// </summary>
        [Test]
        public void OnPost_Valid_ProductID_Redirects_To_Index_Page()
        {
            // Arrange
            pageModel.OnGet("sailorhg-corsage");
            // Act
            var result = pageModel.OnPost();
            var redirect = result as RedirectToPageResult;
            // Assert
            Assert.AreEqual(redirect.PageName, "./Index");
        }
        #endregion OnPost

    }
}