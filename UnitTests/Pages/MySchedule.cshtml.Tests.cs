using ContosoCrafts.WebSite.Pages;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace UnitTests.Pages;

/// <summary>
/// Unit Testing for MyProduct page
/// </summary>
public class MySchedule_cshtml_Tests
{
    #region TestSetup
    //declare the page model for MyProduct page to be used in Unit Testing
    public static MyScheduleModel pageModel;
    public readonly ILogger<MyScheduleModel> _logger;
    
    /// <summary>
    /// Initialize mock MyProduct page model for unit testing
    /// </summary>
    [SetUp]
    public void TestInitialize()
    {
        pageModel = new MyScheduleModel(_logger);
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