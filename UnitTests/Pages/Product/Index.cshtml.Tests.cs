using System.Linq;
using ContosoCrafts.WebSite.Pages.Product;
using NUnit.Framework;

namespace UnitTests.Pages.Product
{
    /// <summary>
    /// Unit Testing for Pages/Product/Index Tests
    /// </summary>
    public class ProductIndexTests
    {
        #region TestSetup
        public static IndexModel pageModel;
        
        /// <summary>
        /// Test Initialization
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            pageModel = new IndexModel(TestHelper.ProductService);
        }
        
        #endregion TestSetup



        #region OnGet
        /// <summary>
        /// Calls IndexModel's OnGet and returns true if the the model state is valid
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_Products()
        {
            // Arrange
            
            // Act
            pageModel.OnGet();
            // Assert
            // check valid Model State
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
            //
            Assert.AreEqual(pageModel.Products, pageModel.Products.OrderBy(x => x.Title).ToList());
        }
        #endregion OnGet
    }
}