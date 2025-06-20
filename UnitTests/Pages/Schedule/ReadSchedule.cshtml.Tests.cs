using ContosoCrafts.WebSite.Pages.Schedule;
using NUnit.Framework;

namespace UnitTests.Pages.Schedule
{
    /// <summary>
    /// Unit Tests for Read Schedule
    /// </summary>
    public class ReadSchedule_cshtml_Tests
    {
        #region TestSetup
        
        public static ReadScheduleModel ScheduleModel;
        
        [SetUp]
        public void TestInitialize()
        {
            ScheduleModel = new ReadScheduleModel(TestHelper.ScheduleService);
        }
        #endregion TestSetup
        
        #region OnGet
        /// <summary>
        /// Unit test that OnGet with a valid id returns the Page
        /// </summary>
        [Test]
        public void OnGet_Valid_Returns_Valid_ModelState()
        {
            // Arrange
            // Act
            ScheduleModel.OnGet("sydney-2025");
            // Assert
            Assert.AreEqual(true, ScheduleModel.ModelState.IsValid);
        }
        /// <summary>
        /// Unit test that OnGet with an invalid id returns an invalid model state
        /// </summary>
        [Test]
        public void OnGet_Invalid_Returns_Invalid_ModelState()
        {
            // Arrange
            // Act
            ScheduleModel.OnGet("invalidTest");
            // Assert
            Assert.AreEqual(false, ScheduleModel.ModelState.IsValid);
        }
        #endregion OnGet
    }
}

