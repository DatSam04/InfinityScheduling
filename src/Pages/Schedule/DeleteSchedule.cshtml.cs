using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Services;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Schedule
{
    /// <summary>
    /// Razor PageModel for handling the deletion of a schedule.
    /// </summary>
    public class DeleteScheduleModel : PageModel
    {
        // Service to manage schedule data in JSON
        private readonly JsonFileScheduleService _scheduleService;

        /// <summary>
        /// Constructor injecting the schedule service.
        /// </summary>
        /// <param name="scheduleService">The injected schedule service.</param>
        public DeleteScheduleModel(JsonFileScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        /// <summary>
        /// Name of the schedule to delete, retrieved from the query string (?name=...).
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string Name { get; set; }

        /// <summary>
        /// Name shown in the confirmation prompt on the page.
        /// </summary>
        [BindProperty]
        public string DestinationName { get; set; }

        /// <summary>
        /// User-typed value to confirm deletion.
        /// </summary>
        [BindProperty]
        public string ConfirmationText { get; set; }

        /// <summary>
        /// Handles GET requests and pre-fills the deletion confirmation with the schedule name.
        /// </summary>
        public IActionResult OnGet()
        {
            DestinationName = Name;
            return Page();
        }

        /// <summary>
        /// Handles POST requests when the user submits the confirmation to delete.
        /// </summary>
        public IActionResult OnPost()
        {
            // Check if the user's typed text matches the expected schedule name
            if (!string.Equals(ConfirmationText?.Trim(), Name?.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                TempData["Message"] = "The name you typed doesn't match. Please try again.";
                return Page();
            }

            // Get all schedules and find the one matching by name
            var allSchedules = _scheduleService.GetSchedules();
            var toDelete = allSchedules.FirstOrDefault(s => s.Name.Equals(Name, StringComparison.OrdinalIgnoreCase));

            if (toDelete == null)
            {
                TempData["Message"] = $"No schedule found with name: '{Name}'.";
                return Page();
            }

            // Delete the matching schedule using its ID
            _scheduleService.DeleteData(toDelete.Id);

            // Success message to show on the redirected page
            TempData["Message"] = $"Successfully deleted '{Name}'.";

            // Redirect to index after deletion
            return RedirectToPage("Index");
        }
    }
}
