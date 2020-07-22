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
        public int? Breakfast_StartTime { get; set; }

        [Display(Name = "End Time")]
        public int? Breakfast_EndTime { get; set; }

        // Lunch
        public bool Lunch_Used { get; set; }

        [Display(Name = "Days Served")]
        public string Lunch_DaysServed { get; set; }

        [Display(Name = "Maximum Guests Served")]
        public int? Lunch_MaximumGuestsServed { get; set; }

        [Display(Name = "Minimum Guests Served")]
        public int? Lunch_MinimumGuestsServed { get; set; }

        [Display(Name = "Start Time")]
        public int? Lunch_StartTime { get; set; }

        [Display(Name = "End Time")]
        public int? Lunch_EndTime { get; set; }

        // Dinner
        public bool Dinner_Used { get; set; }

        [Display(Name = "Days Served")]
        public string Dinner_DaysServed { get; set; }

        [Display(Name = "Maximum Guests Served")]
        public int? Dinner_MaximumGuestsServed { get; set; }

        [Display(Name = "Minimum Guests Served")]
        public int? Dinner_MinimumGuestsServed { get; set; }

        [Display(Name = "Start Time")]
        public int? Dinner_StartTime { get; set; }

        [Display(Name = "End Time")]
        public int? Dinner_EndTime { get; set; }

        // String of Includes
        public const string IncludeBind = "Id, Name, AddressLine1, City, State, ZipCode, Latitude, Longitude, CrossStreet1, CrossStreet2, IsTheGatheringSite, "
                                        + "Breakfast_Used, Breakfast_DaysServed, Breakfast_MaximumGuestsServed, Breakfast_MinimumGuestsServed, Breakfast_StartTime, Breakfast_EndTime, "
                                        + "Lunch_Used, Lunch_DaysServed, Lunch_MaximumGuestsServed, Lunch_MinimumGuestsServed, Lunch_StartTime, Lunch_EndTime, "
                                        + "Dinner_Used, Dinner_DaysServed, Dinner_MaximumGuestsServed, Dinner_MinimumGuestsServed, Dinner_StartTime, Dinner_EndTime";
        public MealSite() { }
        public  MealSite(MealSiteViewModel model)
        {
            Id = model.Id;
            AddressLine1 = model.AddressLine1;
            AddressLine2 = model.AddressLine2;
            City = model.City;
            State = model.State;
            Zipcode = model.Zipcode;
            CrossStreet1 = model.CrossStreet1;
            CrossStreet2 = model.CrossStreet2;
            MealServed = model.MealServed;
            DaysServed = model.DaysServed;
            MaximumGuestsServed = model.MaximumGuestsServed;
            MinimumGuestsServed = model.MinimumGuestsServed;
            StartTime = model.StartTime;
            EndTime = model.EndTime;
            VolunteerEvents = model.VolunteerEvents;
        }
    }
}