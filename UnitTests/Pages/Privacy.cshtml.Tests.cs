using ContosoCrafts.WebSite.Pages;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace UnitTests.Pages;

/// <summary>
/// Provides unit testing for Privacy Page
/// </summary>
public class Privacy_cshtml_Tests
{
    #region TestSetup
    //declare the page model for privacy page to be used in Unit Testing
    public static PrivacyModel pageModel;
    public readonly ILogger<PrivacyModel> _logger;
    
    /// <summary>
    /// Initialize mock privacy page model for unit testing
    /// </summary>
    [SetUp]
    public void TestInitialize()
    {
        pageModel = new PrivacyModel(_logger);
    }
    
    #endregion TestSetup
    
    #region OnGet

    /// <summary>
    /// Test the OnGet method
    /// </summary>
    [Test]
    public void OnGet_Valid_Should_Return_Valid()
    {
        // Arrange
        // Act
        pageModel.OnGet();
        // Assert
        Assert.AreEqual(true, pageModel.ModelState.IsValid);
    }
    
    #endregion OnGet
}