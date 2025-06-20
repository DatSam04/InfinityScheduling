using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages.Schedule;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace UnitTests.Services.TestJsonFileProductService
{
    /// <summary>
    /// Unit Tests for JsonFIleScheduleService
    /// </summary>
    public class JsonFileScheduleServiceTests
    {
        #region TestSetup
        /// <summary>
        /// Test Initialization
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            
        }
        #endregion TestSetup
        
        #region GetSchedule
        /// <summary>
        /// Unit Test for if GetSchedule passing in a valid id returns a non-null schedule
        /// </summary>
        [Test]
        public void GetSchedule_ValidId_Returns_NonNullSchedule()
        {
            // Arrange
            var schedule = TestHelper.ScheduleService.GetSchedule("sydney-2025");
            // Act
            var isNull = schedule == null;

            // Assert
            Assert.AreEqual(false, isNull);
        }
        #endregion GetSchedule
        
        #region CreateData
        /// <summary>
        /// Test that CreateData returns a valid Schedule Model
        /// </summary>
        [Test]
        public void CreateData_Returns_Valid_ScheduleModel()
        {
            // Arrange
            ScheduleModel newSchedule = new ScheduleModel();
            // Act
            TestHelper.ScheduleService.CreateData(newSchedule);
            // Assert
            bool valid = TestHelper.ModelState.IsValid;
            Assert.AreEqual(true, valid);
            
        }
        #endregion CreateData
        
        #region UpdateData
        /// <summary>
        /// Test that UpdateData correctly updates Schedule data
        /// </summary>
        [Test]
        public void UpdateData_Valid_Update_Returns_True()
        {
            // Arrange
            ScheduleModel existingSchedule = TestHelper.ScheduleService.GetSchedule("sydney-2025");
            var oldDescription = existingSchedule.Description;
            existingSchedule.Description = "UpdateTest";
            // Act
            var result = TestHelper.ScheduleService.UpdateData(existingSchedule);
            // Assert
            Assert.AreEqual(true, result);
            
            // Revert
            existingSchedule.Description = oldDescription;
            TestHelper.ScheduleService.UpdateData(existingSchedule);
        }
        #endregion UpdateData
    }
}