using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ContosoCrafts.WebSite.Models;

using Microsoft.AspNetCore.Hosting;

namespace ContosoCrafts.WebSite.Services
{
    /// <summary>
    /// This class is used to manage the schedule data in the schedule.json file
    /// </summary>
    public class JsonFileScheduleService
    {
        /// <summary>
        /// This property is used to get the web host environment.
        /// </summary>
        public IWebHostEnvironment WebHostEnvironment { get; }

        /// <summary>
        /// This property is used to initialize the web host environment
        /// </summary>
        public JsonFileScheduleService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Return the path to schedule json file
        /// </summary>
        private string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "schedule.json"); }
        }

        /// <summary>
        /// This method return the list of schedule from the json file
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ScheduleModel> GetSchedules()
        {
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {
                return JsonSerializer.Deserialize<ScheduleModel[]>(jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });
            }
        }

        public ScheduleModel GetSchedule(string id)
        {
            ScheduleModel schedule = GetSchedules().FirstOrDefault(s => s.Id.Equals(id));

            return schedule;
        }

        /// <summary>
        /// this method create a new schedule and add to the schedule list
        /// </summary>
        public ScheduleModel CreateData(ScheduleModel newProduct)
        {
            var data = GetSchedules();
            data = data.Append(newProduct);

            SaveData(data);

            return newProduct;
        }

        /// <summary>
        /// Delete a schedule from json file
        /// </summary>
        public ScheduleModel DeleteData(string id)
        {
            var dataSet = GetSchedules();
            var data = dataSet.FirstOrDefault(s => s.Id.Equals(id));

            var newDataSet = GetSchedules().Where(s => s.Id.Equals(id) == false);

            SaveData(newDataSet);

            return data;
        }

        /// <summary>
        /// Save the new list to json file
        /// </summary>
        public void SaveData(IEnumerable<ScheduleModel> schedules)
        {
            using (var outputStream = File.Create(JsonFileName))
            {
                JsonSerializer.Serialize<IEnumerable<ScheduleModel>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {
                        SkipValidation = true,
                        Indented = true
                    }),
                    schedules
                );
            }
        }

        /// <summary>
        /// This method update the selected schedule and save to json file
        /// </summary>
        public bool UpdateData(ScheduleModel updatedSchedule)
        {
            var isValidUpdate = false;
            var schedules = GetSchedules().ToList();

            var index = schedules.FindIndex(s => s.Id == updatedSchedule.Id);
            if(index != -1)
            {
                //Update the field that display to user only
                schedules[index].Name = updatedSchedule.Name;
                schedules[index].Triptype = updatedSchedule.Triptype;
                schedules[index].Location = updatedSchedule.Location;
                schedules[index].WeatherUrl = updatedSchedule.WeatherUrl;
                schedules[index].Description = updatedSchedule.Description;
                schedules[index].Image = updatedSchedule.Image;
                schedules[index].DateRange = updatedSchedule.DateRange;

                //Write the updated schedule back to the json file
                using (var JsonFileUpdate = File.Create(JsonFileName))
                {
                    JsonSerializer.Serialize<IEnumerable<ScheduleModel>>(
                        new Utf8JsonWriter(JsonFileUpdate, new JsonWriterOptions
                        {
                            SkipValidation = true,
                            Indented = true
                        }),
                        schedules
                    );
                    isValidUpdate = true;
                }
            }

            return isValidUpdate;
        }
    }
}
