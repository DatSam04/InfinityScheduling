using System;
using System.Linq;
using ContosoCrafts.WebSite.Pages.Schedule;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;

namespace UnitTests.Pages.Schedule;
/// <summary>
/// Implement Unit Testing for Delete Schedule Page
/// </summary>
public class ScheduleDeleteTest
{
    #region TestSetup

    public static DeleteScheduleModel ScheduleModel;
    /// <summary>
    /// Initialize tmpData and Schedule Service for UT
    /// </summary>
    [SetUp]
    public void TestInitialize()
    {
        ScheduleModel = new DeleteScheduleModel(TestHelper.ScheduleService);
        
        //TempData is null by default so initialize a value for it
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        ScheduleModel.TempData = tempData;
    }

    #endregion TestSetup
    
    #region OnGet
    /// <summary>
    /// Check if OnGet assigned name to destinationName and if it return page()
    /// </summary>
    [Test]
    public void OnGet_Valid_Should_Return_ConfirmationPage()
    {
        //Act
        
        //Arrange
        ScheduleModel.Name = "Maldives";
        var result = ScheduleModel.OnGet();
        
        //Assert
        Assert.AreEqual("Maldives", ScheduleModel.DestinationName);
        Assert.IsInstanceOf<PageResult>(result);
    }
    #endregion OnGet

    
    #region OnPost
    /// <summary>
    /// Check if OnPost return Page when Name and Confirmation Text is not the same
    /// </summary>
    [Test]
    public void OnPost_Invalid_Should_Return_Page()
    {
        //Act
        
        //Arrange
        ScheduleModel.ConfirmationText = "Testing";
        ScheduleModel.Name = "Maldives";
        
        var result = ScheduleModel.OnPost();
        
        //Assert
        Assert.AreEqual("The name you typed doesn't match. Please try again.", ScheduleModel.TempData["Message"]);
        Assert.IsInstanceOf<PageResult>(result);
    }
    
    /// <summary>
    /// Check if OnPost return page when the schedule is not found
    /// </summary>
    [Test]
    public void OnPost_Schedule_NotFound_Should_Return_Page()
    {
        //Act
        
        //Arrange
        ScheduleModel.Name = "Testing";
        ScheduleModel.ConfirmationText = "Testing";
        
        var service = TestHelper.ScheduleService;
        var schedule = service.GetSchedules().FirstOrDefault(x => x.Name == "Maldives");
        var result = ScheduleModel.OnPost();
        //Assert
        Assert.IsNotNull(schedule, $"No schedule found with name: '{ScheduleModel.Name}'.");
        Assert.IsInstanceOf<PageResult>(result);

    }
    
    /// <summary>
    /// Check if the OnPost return to Index when confirm the schedule exist and delete it
    /// </summary>
    [Test]
    public void OnPost_ValidConfirmation_Should_Delete_Schedule()
    {
        //Act
        
        //Arrange
        ScheduleModel.Name = "Maldives";
        ScheduleModel.ConfirmationText = "Maldives";
        
        var result = ScheduleModel.OnPost();
        var redirect = result as RedirectToPageResult;
        
        //Assert
        Assert.AreEqual($"Successfully deleted '{ScheduleModel.Name}'.", ScheduleModel.TempData["Message"]);
        Assert.AreEqual(redirect.PageName, "Index");
    }
    #endregion OnPost
}