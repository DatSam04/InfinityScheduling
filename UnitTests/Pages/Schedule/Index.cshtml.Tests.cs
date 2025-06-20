using System.Linq;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using ContosoCrafts.WebSite.Pages.Schedule;
using NUnit.Framework;

namespace UnitTests.Pages.Schedule;
/// <summary>
/// Implement Unit Testing for Schedule Index Page
/// </summary>
public class ScheduleIndexTests
{
    #region TestSetup

    public static IndexModel  ScheduleModel;
    /// <summary>
    /// Initialize ScheduleModel for later UT
    /// </summary>
    [SetUp]
    public void TestInitialize()
    {
        ScheduleModel = new IndexModel(TestHelper.ScheduleService);
    }
    #endregion TestSetup
    
    #region OnGet
    /// <summary>
    /// Check OnGet method for return list of schedules
    /// </summary>
    [Test]
    public void OnGet_Valid_Should_Return_Valid_Model()
    {
        //Action
        
        //Arrange
        ScheduleModel.OnGet();
        
        //Assert
        Assert.AreEqual(true, ScheduleModel.ModelState.IsValid);
        Assert.AreEqual(ScheduleModel.Schedules, ScheduleModel.Schedules.OrderBy(x => x.Name).ToList());
    }
    
    #endregion OnGet
    
    
}