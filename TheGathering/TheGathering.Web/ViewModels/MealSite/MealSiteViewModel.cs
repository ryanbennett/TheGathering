using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TheGathering.Web.Models;
using TheGathering.Web.Services;

namespace TheGathering.Web.ViewModels.MealSite
{
    public class MealSiteViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Display(Name = "Address")]
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

        /// <summary>
        /// This will be empty if there is no error, allows us to create an error for validating input information
        /// </summary>
        public string Error { get; set; }

        public MealSiteViewModel() { }

        public MealSiteViewModel(Models.MealSite mealSite)
        {
            // Transfer Variables
            Id = mealSite.Id;
            AddressLine1 = mealSite.AddressLine1;
            City = mealSite.City;
            Zipcode = mealSite.Zipcode;
            State = mealSite.State;
            Latitude = mealSite.Latitude;
            Longitude = mealSite.Longitude;
            CrossStreet1 = mealSite.CrossStreet1;
            CrossStreet2 = mealSite.CrossStreet2;
            IsTheGatheringSite = mealSite.IsTheGatheringSite;

            Breakfast_Used = mealSite.Breakfast_Used;
            Breakfast_DaysServed = mealSite.Breakfast_DaysServed;
            Breakfast_MaximumGuestsServed = mealSite.Breakfast_MaximumGuestsServed;
            Breakfast_MinimumGuestsServed = mealSite.Breakfast_MinimumGuestsServed;
            Breakfast_StartTime = mealSite.Breakfast_StartTime;
            Breakfast_EndTime = mealSite.Breakfast_EndTime;

            Lunch_Used = mealSite.Lunch_Used;
            Lunch_DaysServed = mealSite.Lunch_DaysServed;
            Lunch_MaximumGuestsServed = mealSite.Lunch_MaximumGuestsServed;
            Lunch_MinimumGuestsServed = mealSite.Lunch_MinimumGuestsServed;
            Lunch_StartTime = mealSite.Lunch_StartTime;
            Lunch_EndTime = mealSite.Lunch_EndTime;

            Dinner_Used = mealSite.Dinner_Used;
            Dinner_DaysServed = mealSite.Dinner_DaysServed;
            Dinner_MaximumGuestsServed = mealSite.Dinner_MaximumGuestsServed;
            Dinner_MinimumGuestsServed = mealSite.Dinner_MinimumGuestsServed;
            Dinner_StartTime = mealSite.Dinner_StartTime;
            Dinner_EndTime = mealSite.Dinner_EndTime;

            if (VolunteerEvents != null)
            {
                VolunteerEvents = mealSite.VolunteerEvents.ToList();
            }
        }

        #region Time Validation Functions
        /// <summary>
        /// Calls all the validate data functions, returns true if all of them are valid returns false if one of them is invalid.
        /// </summary>
        /// <returns>Whether or not all the data is valid</returns>
        public bool ValidateAllData()
        {
            return ValidateBreakfastData() && ValidateLunchData() && ValidateDinnerData();
        }

        /// <summary>
        /// Checks the breakfast data to see if it's invalid. If breakfast is not used then any data related to breakfast is dropped.
        /// If the data is used and valid it returns true, if it's invalid it returns false
        /// </summary>
        /// <returns>Whether or not the data is valid</returns>
        public bool ValidateBreakfastData()
        {
            if (!Breakfast_Used)
            {
                Breakfast_StartTime = null;
                Breakfast_EndTime = null;
                Breakfast_DaysServed = null;
                Breakfast_MaximumGuestsServed = null;
                Breakfast_MinimumGuestsServed = null;
                return true;
            }

            if (Breakfast_StartTime == null || Breakfast_EndTime == null) { return false; }

            DateTime start = (DateTime)Breakfast_StartTime;
            DateTime end = (DateTime)Breakfast_EndTime;

            if (start.CompareTo(end) < 0)
            {
                return false;
            }

            else if (Breakfast_MinimumGuestsServed < Breakfast_MaximumGuestsServed)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks the lunch data to see if it's invalid. If lunch is not used then any data related to lunch is dropped.
        /// If the data is used and valid it returns true, if it's invalid it returns false
        /// </summary>
        /// <returns>Whether or not the data is valid</returns>
        public bool ValidateLunchData()
        {
            if (!Lunch_Used)
            {
                Lunch_StartTime = null;
                Lunch_EndTime = null;
                Lunch_DaysServed = null;
                Lunch_MaximumGuestsServed = null;
                Lunch_MinimumGuestsServed = null;
                return true;
            }

            if (Lunch_StartTime == null && Lunch_EndTime == null) { return true; }

            DateTime start = (DateTime)Lunch_StartTime;
            DateTime end = (DateTime)Lunch_StartTime;

            if (start.CompareTo(end) < 0)
            {
                return false;
            }

            else if (Lunch_MinimumGuestsServed < Breakfast_MaximumGuestsServed)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks the dinner data to see if it's invalid. If dinner is not used then any data related to dinner is dropped.
        /// If the data is used and valid it returns true, if it's invalid it returns false
        /// </summary>
        /// <returns>Whether or not the data is valid</returns>
        public bool ValidateDinnerData()
        {
            if (!Dinner_Used)
            {
                Dinner_StartTime = null;
                Dinner_EndTime = null;
                Dinner_DaysServed = null;
                Dinner_MaximumGuestsServed = null;
                Dinner_MinimumGuestsServed = null;
                return true;
            }
            if (Dinner_StartTime == null && Dinner_EndTime == null) { return false; }

            DateTime start = (DateTime)Dinner_StartTime;
            DateTime end = (DateTime)Dinner_EndTime;

            if (start.CompareTo(end) < 0)
            {
                return false;
            }

            else if (Dinner_MinimumGuestsServed < Dinner_MaximumGuestsServed)
            {
                return true;
            }

            else
            {
                return false;
            }

        }

        #endregion
    }
}