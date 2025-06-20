using ContosoCrafts.WebSite.Pages;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace UnitTests.Pages;

/// <summary>
/// Provides unit testing for AboutUs Page
/// </summary>
public class AboutUs_cshtml_Tests
{
    #region TestSetup

    //declare the page model for about us page to be used in Unit Testing
    public static AboutUsModel pageModel;
    public readonly ILogger<AboutUsModel> _logger;
    
    /// <summary>
    /// Initialize mock about us page model for unit testing
    /// </summary>
    [SetUp]
    public void TestInitialize()
    {
        pageModel = new AboutUsModel(_logger);
    }
    
    #endregion TestSetup
    
    #region OnGet

    /// <summary>
    /// Test the OnGet method
    /// </summary>
    [Test]
    public void OnGet_Valid_Should_Return_Valid()
    {
        //Act
        pageModel.OnGet();
    }
    
    #endregion OnGet
}