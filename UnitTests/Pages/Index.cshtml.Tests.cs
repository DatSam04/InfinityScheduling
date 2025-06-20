using System;
using System.Linq;
using ContosoCrafts.WebSite.Pages;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace UnitTests.Pages;

/// <summary>
/// Provide unit test for Page/index page
/// </summary>
public class Index_cshtml_Tests
{
    #region TestSetup
    public static IndexModel pageModel;
    public readonly ILogger<IndexModel> _logger;

    /// <summary>
    /// Initialize pageModel and _logger for unit testing
    /// </summary>
    [SetUp]
    public void TestInitialize()
    {
        pageModel = new IndexModel(_logger, TestHelper.ProductService);
    }
    
    #endregion TestSetup
    
    #region OnGet

    /// <summary>
    /// Test OnGet method to ensure it retrieve product from json file
    /// </summary>
    [Test]
    public void OnGet_Valid_Should_Return_Products()
    {
        //Arrange
        
        //Act
        pageModel.OnGet();
        
        //Assert
        Assert.AreEqual(true, pageModel.ModelState.IsValid);
        Assert.AreEqual(true, pageModel.Products.ToList().Any());
        //Reset
    }
    
    #endregion OnGet
    
}