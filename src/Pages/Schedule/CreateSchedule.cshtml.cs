using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bunit.Extensions;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoCrafts.WebSite.Pages.Schedule
{
    /// <summary>
    /// Create Schedule Page
    /// </summary>
    public class CreateScheduleModel : PageModel
    {
        // Data middle tier
        public JsonFileScheduleService ScheduleService { get; }

        //Default Constructor
        public CreateScheduleModel(JsonFileScheduleService scheduleService)
        {
            ScheduleService = scheduleService;
        }

        //DateRangeInput data (Start-End)
        [BindProperty]
        [Required(ErrorMessage = "Date range is required")]
        public string DateRangeInput { get; set; }

        //Current schedule data
        [BindProperty]
        public ScheduleModel Schedule { get; set; }

        /// <summary>
        /// This method generate a new schedule model
        /// </summary>
        public void OnGet()
        {
            Schedule = new ScheduleModel()
            {
                Id = System.Guid.NewGuid().ToString(),
                Name = "",
                Image = Array.Empty<string>(),
                Location = "",
                WeatherUrl = "https://www.accuweather.com/",
                DateRange = new DateRangeModel
                {
                    Start = DateTime.Now,
                    End = DateTime.Now
                },
                Description = "",
                Triptype = Array.Empty<string>()
            };
        }

        /// <summary>
        /// Calling OnPost method to saved input information to json file
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPost()
        {
            // Trim and filter empty entries first
            Schedule.Image = Schedule.Image?
                .Select(url => url?.Trim())
                .Where(url => !string.IsNullOrWhiteSpace(url))
                .ToArray();

            // Validate at least one image
            if (Schedule.Image == null || Schedule.Image.Length == 0)
            {
                ModelState.AddModelError("Schedule.Image", "At least one image URL is required.");
                return Page();
            }
            //Filter empty input textbox to reduce amount of loop
            Schedule.Image = Schedule.Image?.Where(url => !string.IsNullOrWhiteSpace(url)).ToArray();
            var urlAttr = new UrlAttribute();
            for (int i = 0; i < Schedule.Image.Length; i++)
            {
                var url = Schedule.Image[i];
                if (await IsImageUrlAsync(url)) continue;
                ModelState.AddModelError("Schedule.Image",$"Image #{i + 1} must be a valid image URL.");
                return Page();
            }

            //Validate date-range
            var parts = DateRangeInput?.Split(" - ");
            if (parts?.Length == 2 &&
                DateTime.TryParse(parts[0].Trim(), out var start) &&
                DateTime.TryParse(parts[1].Trim(), out var end))
            {
                Schedule.DateRange = new DateRangeModel { Start = start, End = end };

                // Manually validate nested object
                var context = new ValidationContext(Schedule.DateRange);
                var results = new List<ValidationResult>();
                if (!Validator.TryValidateObject(Schedule.DateRange, context, results, true))
                {
                    foreach (var result in results)
                    {
                        foreach (var memberName in result.MemberNames)
                        {
                            ModelState.AddModelError($"Schedule.DateRange.{memberName}", result.ErrorMessage);
                        }
                    }
                    return Page();
                }
            }

            Schedule.Id = Guid.NewGuid().ToString();
            ScheduleService.CreateData(Schedule);

            return RedirectToPage("/Schedule/Index");
        }
        
        /// <summary>
        /// Method that checks the content type of the given imageUrl and returns
        /// whether it's an image or not
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public async Task<bool> IsImageUrlAsync(string imageUrl)
        {
            try
            {
                using var httpClient = new HttpClient();
                //Pass the url to HttpMethod.Head request to check the content type of this metadata
                using var request = new HttpRequestMessage(HttpMethod.Head, imageUrl);
                using var response = await httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                    return false;

                var contentType = response.Content.Headers.ContentType?.MediaType;
                //Accept url image that have query string after image file extension
                return contentType != null && contentType.StartsWith("image/");
            }
            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// Method for generating the accuweather Url for a given location name
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public IActionResult OnGetGenerateWeatherUrl(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                return Content(string.Empty);

            string formattedLocation = location.Trim().Replace(" ", "-").ToLowerInvariant();
            string url = $"https://www.accuweather.com/en/search-locations?query={Uri.EscapeDataString(location)}";

            return Content(url);
        }
    }
}
