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

        [Display(Name="Address")]
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [Display(Name = "Cross Street 1")]
        public string CrossStreet1 { get; set; }

        [Display(Name = "Cross Street 2")]
        public string CrossStreet2 { get; set; }

        public bool IsTheGatheringSite { get; set; }

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
        public DateTime? Breakfast_StartTime { get; set; }

        [Display(Name = "End Time")]
        public DateTime? Breakfast_EndTime { get; set; }

        // Lunch
        public bool Lunch_Used { get; set; }

        [Display(Name = "Days Served")]
        public string Lunch_DaysServed { get; set; }

        [Display(Name = "Maximum Guests Served")]
        public int? Lunch_MaximumGuestsServed { get; set; }

        [Display(Name = "Minimum Guests Served")]
        public int? Lunch_MinimumGuestsServed { get; set; }

        [Display(Name = "Start Time")]
        public DateTime? Lunch_StartTime { get; set; }

        [Display(Name = "End Time")]
        public DateTime? Lunch_EndTime { get; set; }

        // Dinner
        public bool Dinner_Used { get; set; }

        [Display(Name = "Days Served")]
        public string Dinner_DaysServed { get; set; }

        [Display(Name = "Maximum Guests Served")]
        public int? Dinner_MaximumGuestsServed { get; set; }

        [Display(Name = "Minimum Guests Served")]
        public int? Dinner_MinimumGuestsServed { get; set; }

        [Display(Name = "Start Time")]
        public DateTime? Dinner_StartTime { get; set; }

        [Display(Name = "End Time")]
        public DateTime? Dinner_EndTime { get; set; }

        public List<VolunteerEvent> VolunteerEvents { get; set; }

        // String of Includes
        public const string IncludeBind = "Id, Name, AddressLine1, City, State, ZipCode, Latitude, Longitude, CrossStreet1, CrossStreet2, IsTheGatheringSite, "
                                        + "Breakfast_Used, Breakfast_DaysServed, Breakfast_MaximumGuestsServed, Breakfast_MinimumGuestsServed, Breakfast_StartTime, Breakfast_EndTime, "
                                        + "Lunch_Used, Lunch_DaysServed, Lunch_MaximumGuestsServed, Lunch_MinimumGuestsServed, Lunch_StartTime, Lunch_EndTime, "
                                        + "Dinner_Used, Dinner_DaysServed, Dinner_MaximumGuestsServed, Dinner_MinimumGuestsServed, Dinner_StartTime, Dinner_EndTime";
        public MealSite() { }

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
            CrossStreet1 = model.CrossStreet1;
            CrossStreet2 = model.CrossStreet2;
            IsTheGatheringSite = model.IsTheGatheringSite;

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
    }
}