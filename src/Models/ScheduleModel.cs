using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ContosoCrafts.WebSite.Models
{
    /// <summary>
    /// This class represents a schedule model
    /// </summary>
    public class ScheduleModel
    {
        //Schedule Id is a randomly generated string
        public string Id { get; set; }

        //This property is used to store name of the schedule
        [Required(ErrorMessage = "Schedule name is required")]
        public string Name { get; set; }

        //This property is used to store the image url list of the location
        [JsonPropertyName("img")]
        [Required(ErrorMessage = "Image URL is required")] // To validate the image URL
        public string[] Image { get; set; }

        //This property is used to store name of the location
        [Required(ErrorMessage = "Location name is required")]
        public string Location { get; set; }

        //This property is used to store the weather page url for this location
        public string WeatherUrl { get; set; }

        //This property is used to store date range user stay at a specific location
        [Required(ErrorMessage = "Date range is required")]
        public DateRangeModel DateRange { get; set; }

        //This property is used to describe about the location
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        //This property is used to store the list of string about the type of this schedule
        [Required(ErrorMessage = "Trip Type is required")]
        public string[] Triptype { get; set; }
    }
}
