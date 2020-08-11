using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TheGathering.Web.ViewModels.MealSite;

namespace TheGathering.Web.Models
{
    public class MealSite
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Display(Name="Street")]
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsTheGatheringSite { get; set; }
        public bool IsMealSiteActive { get; set; }

        /*
         *      Breakfast, Lunch, and Dinner Meals
         */

        // Breakfast
        public bool Breakfast_Used { get; set; }

        [Display(Name = "Days Served")]
        public string Breakfast_DaysServed { get; set; }

        [Display(Name = "Maximum Guests Served")]
        public int? Breakfast_MaximumGuestsServed { get; set; }

        [Display(Name = "Minimum Guests Served")]
        public int? Breakfast_MinimumGuestsServed { get; set; }

        [Display(Name = "Start Time")]
        public string Breakfast_StartTime { get; set; }

        [Display(Name = "End Time")]
        public string Breakfast_EndTime { get; set; }

        // Lunch
        public bool Lunch_Used { get; set; }

        [Display(Name = "Days Served")]
        public string Lunch_DaysServed { get; set; }

        [Display(Name = "Maximum Guests Served")]
        public int? Lunch_MaximumGuestsServed { get; set; }

        [Display(Name = "Minimum Guests Served")]
        public int? Lunch_MinimumGuestsServed { get; set; }

        [Display(Name = "Start Time")]
        public string Lunch_StartTime { get; set; }

        [Display(Name = "End Time")]
        public string Lunch_EndTime { get; set; }

        // Dinner
        public bool Dinner_Used { get; set; }

        [Display(Name = "Days Served")]
        public string Dinner_DaysServed { get; set; }

        [Display(Name = "Maximum Guests Served")]
        public int? Dinner_MaximumGuestsServed { get; set; }

        [Display(Name = "Minimum Guests Served")]
        public int? Dinner_MinimumGuestsServed { get; set; }

        [Display(Name = "Start Time")]
        public string Dinner_StartTime { get; set; }

        [Display(Name = "End Time")]
        public string Dinner_EndTime { get; set; }

        public List<VolunteerEvent> VolunteerEvents { get; set; }

        // String of Includes
        public const string IncludeBind = "Id, Name, AddressLine1, City, State, ZipCode, Latitude, Longitude, CrossStreet1, CrossStreet2, IsTheGatheringSite, "
                                        + "Breakfast_Used, Breakfast_DaysServed, Breakfast_MaximumGuestsServed, Breakfast_MinimumGuestsServed, Breakfast_StartTime, Breakfast_EndTime, "
                                        + "Lunch_Used, Lunch_DaysServed, Lunch_MaximumGuestsServed, Lunch_MinimumGuestsServed, Lunch_StartTime, Lunch_EndTime, "
                                        + "Dinner_Used, Dinner_DaysServed, Dinner_MaximumGuestsServed, Dinner_MinimumGuestsServed, Dinner_StartTime, Dinner_EndTime";
        public MealSite() 
        {
            IsMealSiteActive = true;        
        }

        public MealSite(MealSiteViewModel model)
        {
            // Transfer Variables
            Id = model.Id;
            Name = model.Name;
            AddressLine1 = model.AddressLine1;
            City = model.City;
            Zipcode = model.Zipcode;
            State = model.State;
            Latitude = model.Latitude;
            Longitude = model.Longitude;
            IsTheGatheringSite = model.IsTheGatheringSite;
            IsMealSiteActive = model.IsMealSiteActive;

            Breakfast_Used = model.Breakfast_Used;
            Breakfast_DaysServed = JsonConvert.SerializeObject(model.Breakfast_DaysServed);
            Breakfast_MaximumGuestsServed = model.Breakfast_MaximumGuestsServed;
            Breakfast_MinimumGuestsServed = model.Breakfast_MinimumGuestsServed;
            Breakfast_StartTime = model.Breakfast_StartTime;
            Breakfast_EndTime = model.Breakfast_EndTime;

            Lunch_Used = model.Lunch_Used;
            Lunch_DaysServed = JsonConvert.SerializeObject(model.Lunch_DaysServed);
            Lunch_MaximumGuestsServed = model.Lunch_MaximumGuestsServed;
            Lunch_MinimumGuestsServed = model.Lunch_MinimumGuestsServed;
            Lunch_StartTime = model.Lunch_StartTime;
            Lunch_EndTime = model.Lunch_EndTime;

            Dinner_Used = model.Dinner_Used;
            Dinner_DaysServed = JsonConvert.SerializeObject(model.Dinner_DaysServed);
            Dinner_MaximumGuestsServed = model.Dinner_MaximumGuestsServed;
            Dinner_MinimumGuestsServed = model.Dinner_MinimumGuestsServed;
            Dinner_StartTime = model.Dinner_StartTime;
            Dinner_EndTime = model.Dinner_EndTime;

            if (VolunteerEvents != null)
            {
                VolunteerEvents = model.VolunteerEvents.ToList();
            }

        }

        public static string twelveTo24HourTime(string twelveHourTime)
        {
            try
            {
                string[] split = twelveHourTime.Split(':');
                int hr = int.Parse(split[0]);
                int min = int.Parse(split[1]);
                int newHr = hr % 12;
                if (newHr == 0)
                    newHr = 12;
                if (hr < 12)
                    return newHr + ":" + min + " AM";
                else
                    return newHr + ":" + min + " PM";
            }
            catch
            {
                throw new Exception("Invalid Time: " + twelveHourTime);
            }
        }
    }
}