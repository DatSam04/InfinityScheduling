using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages;
using ContosoCrafts.WebSite.Pages.Schedule;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace UnitTests.Pages.Schedule
{
    /// <summary>
    /// Create Schedule Unit Tests
    /// </summary>
    public class CreateScheduleTests
    {
        #region TestSetup

        public static CreateScheduleModel ScheduleModel;
        public readonly ILogger<CreateScheduleModel> _logger;
        
        /// <summary>
        /// Initialize schedule service for unit testing
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            ScheduleModel = new CreateScheduleModel(TestHelper.ScheduleService);
        }
        
        #endregion TestSetup
        
        #region OnGet
        /// <summary>
        /// Test that OnGet returns a valid new empty schedule
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_Schedule()
        {
            // Arrange
            // Act
            ScheduleModel.OnGet();
            // Assert
            Assert.AreEqual(true, ScheduleModel.ModelState.IsValid);
        }
        #endregion OnGet
        
        #region OnPost
        /// <summary>
        /// Unit Test that OnPost returns valid model state
        /// </summary>
        [Test]
        public void OnPost_Valid_Should_Return_Valid_ModelState()
        {
            // Arrange
            // Act
            string newScheduleId = System.Guid.NewGuid().ToString();
            ScheduleModel.DateRangeInput = "2025-05-30 - 2025-05-31";
            ScheduleModel.Schedule = new ScheduleModel()
            {
                Id = newScheduleId,
                Name = "ValidTestName",
                Image = ["https://images-assets.nasa.gov/image/PIA00405/PIA00405~large.jpg",
                         "https://images.nationalgeographic.org/image/upload/v1638882786/EducationHub/photos/sun-blasts-a-m66-flare.jpg"],
                Location = "ValidTestLocation",
                WeatherUrl = "https://www.accuweather.com/",
                DateRange = new DateRangeModel
                {
                    Start = DateTime.Now,
                    End = DateTime.Now
                },
                Description = "ValidTestDescription",
                Triptype = Array.Empty<string>()
            };
            
            var result = ScheduleModel.OnPost();
            // Assert
            Assert.AreEqual(true, ScheduleModel.ModelState.IsValid);
            
            // Reset
            ScheduleModel.ScheduleService.DeleteData(newScheduleId);
        }
        /// <summary>
        /// Test that OnPost with a null image returns model state error
        /// </summary>
        [Test]
        public void OnPost_NullImage_Returns_Invalid_ModelState()
        {
            // Arrange
            ScheduleModel.OnGet();
            ScheduleModel.Schedule.Image = null;
            
            // Act
            var result = ScheduleModel.OnPost();
            // Assert
            Assert.AreEqual(false, ScheduleModel.ModelState.IsValid);
        }
        /// <summary>
        /// Test that an invalid image url returns invalid model state
        /// </summary>
        [Test]
        public void OnPost_InvalidImage_Returns_Invalid_ModelState()
        {
            // Arrange
            ScheduleModel.OnGet();
            ScheduleModel.Schedule.Image = ["invalidTestUrl"];
            // Act
            var result = ScheduleModel.OnPost();
            // Assert
            Assert.AreEqual(false, ScheduleModel.ModelState.IsValid);
        }

        /// <summary>
        /// Test valid DateRangeInput should return valid model state
        /// </summary>
        [Test]
        public async Task OnPost_Valid_DateRangeInput_Return_ModelState_IsValid()
        {
            //Arrange
            //Act
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
            ScheduleModel.OnGet();
            var location = "";
            var generatedURL = ScheduleModel.OnGetGenerateWeatherUrl(location) as ContentResult;
            //Assert
            Assert.AreEqual(string.Empty, generatedURL.Content);
        }
        #endregion OnGetGenerateWeatherUrl
    }
}

