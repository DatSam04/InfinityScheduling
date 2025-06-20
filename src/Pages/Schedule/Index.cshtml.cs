using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Schedule
{
    /// <summary>
    /// Index Page
    /// </summary>
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="scheduleService"></param>
        public IndexModel(JsonFileScheduleService scheduleService)
        {
            ScheduleService = scheduleService;
        }

        //Connect to service page for schedule
        public JsonFileScheduleService ScheduleService { get; }

        //Store the schedule list from json file
        public IEnumerable<ScheduleModel> Schedules { get; private set; }

        /// <summary>
        /// REST Get request
        /// </summary>
        public void OnGet()
        {
            Schedules = ScheduleService.GetSchedules().OrderBy(x => x.Name).ToList();
        }
    }
}
