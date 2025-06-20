using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContosoCrafts.WebSite.Models
{
    /// <summary>
    /// This class represent the date range model of a schedule
    /// </summary>
    public class DateRangeModel
    {
        //This property is used to store start time of date range
        [Required(ErrorMessage = "Start date is required")]
        [Range(typeof(DateTime), "1/1/2000", "12/31/2100", ErrorMessage = "Start date is out of range")]
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime Start { get; set; }

        //This property is used to store end time of date range
        [Required(ErrorMessage = "End date is required")]
        [Range(typeof(DateTime), "1/1/2000", "12/31/2100", ErrorMessage = "End date is out of range")]
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime End { get; set; }
    }
}
