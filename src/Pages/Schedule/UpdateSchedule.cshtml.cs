using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ContosoCrafts.WebSite.Pages.Schedule
{
    /// <summary>
    /// Class for Update Schedule page Model
    /// </summary>
    public class UpdateScheduleModel : PageModel
    {
        // data middletier
        private readonly JsonFileScheduleService _scheduleService;
        
        /// <summary>
        /// Update Schedule Model default constructor, sets schedule service
        /// </summary>
        /// <param name="scheduleService"></param>
        public UpdateScheduleModel(JsonFileScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }
        
        // property for a schedule's Id
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }
        
        // property for user-inputted range of dates
        [BindProperty]
        [Required(ErrorMessage = "Date range is required")]
        public string DateRangeInput { get; set; }
        
        // individual Schedule we are working on
        [BindProperty]
        public ScheduleModel Schedule { get; set; }
        
        /// <summary>
        /// OnGet method gets schedule corresponding to given ID
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGet()
        {
            // Fetch the schedule based on the given ID
            Schedule = _scheduleService.GetSchedules().FirstOrDefault(s => s.Id == Id);

            if (Schedule == null)
            {
                // TempData["Message"] = "Schedule not found.";
                return RedirectToPage("Index");
            }

            if (Schedule.DateRange != null)
            {
                DateRangeInput = $"{Schedule.DateRange.Start:yyyy-MM-dd} - {Schedule.DateRange.End:yyyy-MM-dd}";
            }

            return Page();
        }
        
        /// <summary>
        /// OnPost method posts updated Schedule to schedule database
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
                ModelState.AddModelError("Schedule.Image", $"Image #{i + 1} must be a valid image URL.");
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

            var resultUpdate = _scheduleService.UpdateData(Schedule);

            if (resultUpdate)
            {
                // TempData["Message"] = "Schedule updated successfully!";
                return RedirectToPage("Index");
            }

            // TempData["Message"] = "Update failed. Try again.";
            return Page();
        }
        
        /// <summary>
        /// Method that returns whether a given imageUrl's content type is an image or not
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public async Task<bool> IsImageUrlAsync(string imageUrl)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");
            //Pass the url to HttpMethod.Head request to check the content type of this metadata
            using var response = await httpClient.GetAsync(imageUrl, HttpCompletionOption.ResponseHeadersRead);


            if (!response.IsSuccessStatusCode)
                return false;

            var contentType = response.Content.Headers.ContentType?.MediaType;
            //Accept url image that have query string after image file extension
            return contentType != null && contentType.StartsWith("image/");
        }
        
        /// <summary>
        /// Method for generating an accuweather url for a given location name
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
