using System.Collections.Generic;
using System.Linq;
using ContosoCrafts.WebSite.Controllers;
using NUnit.Framework;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework.Internal;

namespace UnitTests.Pages.Controllers;
/// <summary>
/// Unit Testing for ProductsController
/// </summary>
public class ProductsController_Tests
{
    #region TestSetup

    public static ProductsController productModel;
    /// <summary>
    /// Initialize product Service for testing
    /// </summary>
    [SetUp]
    public void TestInitialize()
    {
        productModel = new ProductsController(TestHelper.ProductService);
    }

    #endregion TestSetup

    
    #region Get
    /// <summary>
    /// Call Get method and check if it return product list
    /// </summary>
    [Test]
    public void Get_Valid_Should_Return_Products()
    {
        //Act

        //Arrange
        productModel.Get();

        //Assert
        Assert.AreEqual(true, productModel.ModelState.IsValid);
        Assert.AreEqual(true, productModel.Get().ToList().Any());
    }

    #endregion Get

    #region Patch
    /// <summary>
    /// Checking if HttpPatch request successfully add rating to product
    /// </summary>
    [Test]
    public void Patch_Valid_Should_Return_Add_Rating()
    {
        //Action
        
        //Arrange
        var product = productModel.Get().FirstOrDefault();

        var request = new ProductsController.RatingRequest
        {
            ProductId = product.Id,
            Rating = 5,
        };

        var originalRatings = product.Ratings?.ToList() ?? new List<int>();
        
        var result = productModel.Patch(request);
        
        
        var updatedProduct = productModel.Get().First(x => x.Id == product.Id);
        
        //Assert
        Assert.IsInstanceOf<OkResult>(result);
        Assert.IsTrue(updatedProduct.Ratings.Length == originalRatings.Count + 1);
        Assert.Contains(5, updatedProduct.Ratings);
    }

    #endregion
}