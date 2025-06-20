using System.Linq;
using ContosoCrafts.WebSite.Models;
using NUnit.Framework;


namespace UnitTests.Services.TestJsonFileProductService
{
    /// <summary>
    /// Unit Tests for JsonFileProductService
    /// </summary>
    public class JsonFileProductServiceTests
    {
        #region TestSetup
        /// <summary>
        /// Test initialization
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
        }
        
        #endregion TestSetup
        
        #region AddRating
        /// <summary>
        /// REST Get Products data
        /// POST a valid rating
        /// Test that the last data that was added was added correctly
        /// </summary>
        [Test]
        public void AddRating_Valid_ProductId_Return_true()
        {
            // Arrange
            
            // Create Dummy Record with no prior Ratings
            // Get the Last data item
            var data = TestHelper.ProductService.GetProducts().Last();
            
            // Act
            // Store the result of the AddRating method (which is being tested)
            bool validAdd = TestHelper.ProductService.AddRating(data.Id, 0);
            
            // Assert
            Assert.AreEqual(true, validAdd);
            
            // Reset
            // Delete Dummy Record
        }
        
        /// <summary>
        /// REST POST data that doesn't fit the constraints defined in function
        /// Test if it Adds
        /// Returns False because it wont add
        /// </summary>
        [Test]
        public void AddRating_Invalid_Product_ID_Not_Present_Should_Return_False()
        {
            // Arrange
            
            // Act
            // Store the result of the AddRating method (which is being tested)
            var result = TestHelper.ProductService.AddRating("1000", 5);
            
            // Assert
            Assert.AreEqual(false, result);
        }
        /// <summary>
        /// REST Get Products data
        /// POST a valid rating
        /// Return true if the rating was added successfully
        /// </summary>
        [Test]
        public void AddRating_To_NonEmpty_Product_Ratings_Return_True()
        {
            // Arrange
            
            // Creates a record form the first Product in the list, that has ratings already
            var data = TestHelper.ProductService.GetProducts().First();
            
            // Act
            // Store the result of the AddRating method (which is being tested)
            var validAdd = TestHelper.ProductService.AddRating(data.Id, 5);
            
            // Assert
            Assert.AreEqual(validAdd, true);
        }
        
        #endregion AddRating
        
        #region UpdateData
        /// <summary>
        /// Test that updates a valid product's Title and returns true if successfully updated
        /// </summary>
        [Test]
        public void UpdateData_Valid_ProductId_Return_True()
        {
            // Arrange
            ProductModel updatedInformationProduct = TestHelper.ProductService.GetProducts().First();
            updatedInformationProduct.Title = "Updated Title";
            // Act
            var isValid = TestHelper.ProductService.UpdateData(updatedInformationProduct);
            // Assert
            Assert.AreEqual(true, isValid);
        }
        #endregion UpdateData
        
        #region CreateData
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CreateData_Creates_Valid_Product()
        {
            // Arrange
            var productId = System.Guid.NewGuid().ToString(); // generate new Id for new Product
            
            // create new Product
            var newProduct = new ProductModel() {
                Id = productId,
                Title = "Test, Washington",
                Description = "Test Description",
                Maker = "TestMaker",
                Url = "",
                Image = "",
                Ratings = null
            };
            // Act
            TestHelper.ProductService.CreateData(newProduct); // Call CreateData on mock ProductService
            var addedProduct = TestHelper.ProductService.GetProducts().First(x => x.Id == productId);
            // Assert
            Assert.AreEqual("TestMaker", addedProduct.Maker);
        }
        #endregion CreateData
    
    }
}