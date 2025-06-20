using System;
using System.Linq;
using System.Threading.Tasks;
using ContosoCrafts.WebSite.Pages.Schedule;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NUnit.Framework;

namespace UnitTests.Pages.Schedule
{
    /// <summary>
    /// Unit Testing for UpdateSchedule
    /// </summary>
    public class UpdateSchedule_cshtml_Tests
    {
        #region TestSetup
        
        // Data service
        public static UpdateScheduleModel ScheduleModel;
        
        /// <summary>
        /// Test initialization
        /// </summary>
        [SetUp]
        public static void TestInitialize()
        {
            ScheduleModel = new UpdateScheduleModel(TestHelper.ScheduleService);
        }
        
        #endregion TestSetup
        
        #region OnGet
        /// <summary>
        /// Unit test for OnGet with valid input returns Page Result
        /// </summary>
        [Test]
        public void OnGet_Valid_Id_Should_Return_Page()
        {
            // Arrange
            ScheduleModel.Id = "sydney-2025";
            // Act
            var result = ScheduleModel.OnGet();
            // Assert
            Assert.AreEqual("Microsoft.AspNetCore.Mvc.RazorPages.PageResult", result.ToString());
        }
        /// <summary>
        /// Unit test for fail to find on OnGet, redirects to Index page result
        /// </summary>
        [Test]
        public void OnGet_Not_Found_Returns_RedirectToPage()
        {
            // Arrange
            ScheduleModel.Id = "IdThatDoesNotExist";
            // Act
            var result = ScheduleModel.OnGet();
            // Assert
            Assert.AreEqual("Microsoft.AspNetCore.Mvc.RedirectToPageResult", result.ToString());
        }
        #endregion OnGet
        
        #region OnPost
        /// <summary>
        /// Unit test for OnPost with invalid model state returns Page
        /// </summary>
        [Test]
        public void OnPost_Invalid_ModelState_Should_Return_Invalid_ModelState()
        {
            // Arrange
            ScheduleModel.ModelState.AddModelError("Test", "Error");
            // Act
            var result = ScheduleModel.OnPost();
            // Assert
            Assert.AreEqual(false, ScheduleModel.ModelState.IsValid);
            // Reset
            ScheduleModel.ModelState.Clear();
        }
        
        /// <summary>
        /// Unit test for OnPost with valid model state and valid update returns redirect to page
        /// </summary>
        [Test]
        public void OnPost_Valid_ModelState_Successful_Update_Returns_Valid_ModelState()
        {
            // Arrange
            ScheduleModel.Id = "sydney-2025";
            ScheduleModel.OnGet();
            var description = ScheduleModel.Schedule.Description;
            // Act
            ScheduleModel.Schedule.Description = "Updated Description Test";
            var result = ScheduleModel.OnPost();
            // Assert
            Assert.AreEqual(true, ScheduleModel.ModelState.IsValid);
            // Reset
            ScheduleModel.Schedule.Description = description;
            ScheduleModel.OnPost();
        }
        
        /// <summary>
        /// Unit test for OnPost with valid model state and valid update returns redirect to page
        /// </summary>
        [Test]
        public void OnPost_Valid_ModelState_Unsuccessful_Update_Returns_Valid_ModelState()
        {
            // Arrange
            ScheduleModel.Id = "sydney-2025";
            ScheduleModel.OnGet();
            ScheduleModel.Schedule.Id = "InvalidTestId";
            // Act
            var result = ScheduleModel.OnPost();
            // Assert
            Assert.AreEqual(true, ScheduleModel.ModelState.IsValid);
        }

        /// <summary>
        /// Test that empty image url will return invalid and direct back to page
        /// </summary>
        [Test]
        public async Task OnPost_Empty_Image_Should_return_Invalid()
        {
            //Arrange
            //Act
            ScheduleModel.Id = "sydney-2025";
            ScheduleModel.OnGet();
            ScheduleModel.Schedule.Image = null;
            var result = await ScheduleModel.OnPost();
            var error = ScheduleModel.ModelState.Keys
                .Where(x => x.StartsWith("Schedule.Image"))
                .ToList();
            
            //Assert
            Assert.AreEqual(true, error.Count > 0);
            Assert.IsInstanceOf<PageResult>(result);
            
            ScheduleModel.ModelState.Clear();
        }

        /// <summary>
        /// Test that invalid image should return invalid and direct back to page
        /// </summary>
        [Test]
        public async Task OnPost_Invalid_Image_Should_Return_Invalid()
        {
            //Arrange
            //Act
            ScheduleModel.Id = "sydney-2025";
            ScheduleModel.OnGet();
            ScheduleModel.Schedule.Image = ["https://google.com"];
            var result = await ScheduleModel.OnPost();
            var error = ScheduleModel.ModelState.Keys
                .Where(x => x.StartsWith("Schedule.Image"))
                .ToList();
            
            //Assert
            Assert.AreEqual(true, error.Count > 0);
            Assert.IsInstanceOf<PageResult>(result);
            
            ScheduleModel.ModelState.Clear();
        }
        
        /// <summary>
        /// Test valid DateRangeInput should return valid model state
        /// </summary>
        [Test]
        public async Task OnPost_Valid_DateRangeInput_Return_ModelState_IsValid()
        {
            //Arrange
            //Act
            ScheduleModel.Id = "sydney-2025";
            ScheduleModel.OnGet();
            ScheduleModel.Schedule.Image = ["https://images.pexels.com/photos/208590/pexels-photo-208590.jpeg"];
            ScheduleModel.DateRangeInput = "05/25/2025 - 05/30/2025";
            var result = await ScheduleModel.OnPost();
            var DateRangeErrorKey = ScheduleModel.ModelState.Keys
                .Where(k => k.StartsWith("Schedule.DateRange"))
                .ToList();
            
            //Assert
            Assert.AreEqual(true, DateRangeErrorKey.Count == 0);
            Assert.AreEqual(new DateTime(2025, 05,25), ScheduleModel.Schedule.DateRange.Start);
            Assert.AreEqual(new DateTime(2025, 05,30), ScheduleModel.Schedule.DateRange.End);
        }

        /// <summary>
        /// Test if invalid date range return error in model state
        /// Test if it return page() when have invalid daterange
        /// </summary>
        [Test]
        public async Task OnPost_Invalid_DateRangeInput_Returns_Page()
        {
            //Act
            //Arrange
            ScheduleModel.Id = "sydney-2025";
            ScheduleModel.OnGet();
            ScheduleModel.Schedule.Image = ["https://images.pexels.com/photos/208590/pexels-photo-208590.jpeg"];
            ScheduleModel.DateRangeInput = "05/25/2025 - 01/01/0001";
            var result = await ScheduleModel.OnPost();
            var DateRangeErrorKey = ScheduleModel.ModelState
                .Where(k => k.Key.StartsWith("Schedule.DateRange"))
                .SelectMany(err => err.Value.Errors)
                .ToList();
            
            //Assert
            Assert.AreEqual(true, DateRangeErrorKey.Count > 0);
            Assert.IsInstanceOf<PageResult>(result);
        }

        /// <summary>
        /// Test that if ID is not found, it will return to page
        /// </summary>
        [Test]
        public async Task OnPost_Invalid_ID_Should_Return_Page()
        {
            //Arrange
            //Act
            ScheduleModel.Id = "sydney-2025";
            ScheduleModel.OnGet();
            ScheduleModel.Schedule.Id = "InvalidTestId";
            var result = await ScheduleModel.OnPost();
            
            //Assert
            Assert.IsInstanceOf<PageResult>(result);
        }
        
        #endregion OnPost
        
        #region IsImageUrlAsync
        /// <summary>
        /// Test that valid image return success status code
        /// </summary>
        [Test]
        public async Task IsImageUrlAsync_Valid_Should_Return_True()
        {
            //Arrange
            //Act
            var validImageUrl = "https://images.pexels.com/photos/208590/pexels-photo-208590.jpeg";
            var result = await ScheduleModel.IsImageUrlAsync(validImageUrl);
            //Assert
            Assert.AreEqual(true, result);
        }
        /// <summary>
        /// Test that invalid image url return unsuccess status code
        /// </summary>
        [Test]
        public async Task IsImageUrlAsync_InValid_Should_Return_False()
        {
            //Arrange
            //Act
            var invalidImageUrl = "https://httpstat.us/404";
            var result = await ScheduleModel.IsImageUrlAsync(invalidImageUrl);
            //Assert
            Assert.AreEqual(false, result);
        }
        #endregion IsImageUrlAsync
        
        #region OnGetGenerateWeatherUrl
        /// <summary>
        /// Test that not-null location return weather forecast url
        /// </summary>
        [Test]
        public void OnGetGenerateWeatherUrl_Valid_Should_Return_Url()
        {
            //Arrange
            //Act
            ScheduleModel.Id = "sydney-2025";
            ScheduleModel.OnGet();
            var location = "Seattle";
            var expectedURL = $"https://www.accuweather.com/en/search-locations?query={Uri.EscapeDataString(location)}";
            var generatedURL = ScheduleModel.OnGetGenerateWeatherUrl(location) as ContentResult;
            //Assert
            Assert.AreEqual(expectedURL, generatedURL.Content);
        }
        
        /// <summary>
        /// Test that null or empty location return empty string
        /// </summary>
        [Test]
        public void OnGetGenerateWeatherUrl_Invalid_Should_Return_Empty_String()
        {
            //Arrange
            //Act
            ScheduleModel.Id = "sydney-2025";
            ScheduleModel.OnGet();
            var location = "";
            var generatedURL = ScheduleModel.OnGetGenerateWeatherUrl(location) as ContentResult;
            //Assert
            Assert.AreEqual(string.Empty, generatedURL.Content);
        }
        #endregion OnGetGenerateWeatherUrl
    }
}

