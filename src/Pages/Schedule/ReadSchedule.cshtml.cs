using ContosoCrafts.WebSite.Services;
using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Collections;
using System;

namespace ContosoCrafts.WebSite.Pages.Schedule
{
    /// <summary>
    /// Model for Read Schedule
    /// </summary>
    public class ReadScheduleModel : PageModel
    {
        /// <summary>
        /// Constructor method for Read Schedule Model, taking in a scheduleService
        /// </summary>
        /// <param name="scheduleService"></param>
        public ReadScheduleModel(JsonFileScheduleService scheduleService)
        {
            ScheduleService = scheduleService;
        }
        // Data middletier
        public JsonFileScheduleService ScheduleService { get; set; } 
        // The individual Schedule being worked on
        public ScheduleModel Schedule { get; private set; }
        
        /// <summary>
        /// OnGet method for reading a given schedule's id from the list of schedules
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult OnGet(string id)
        {
            Console.Write(id);
            Schedule = ScheduleService.GetSchedules().FirstOrDefault(x => x.Id.Equals(id));
            if(Schedule == null)
            {
                this.ModelState.AddModelError(string.Empty, "Schedule not found");
            }
            return Page();
        }
    }
}
