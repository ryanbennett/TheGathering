using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheGathering.Web.Models;
using TheGathering.Web.Services;

namespace TheGathering.Web.ViewModels.MealSite
{
    public class MealSiteViewModel
    {
        public int Id { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string CrossStreet1 { get; set; }
        public string CrossStreet2 { get; set; }
        public string MealServed { get; set; }
        public string DaysServed { get; set; }
        public int MaximumGuestsServed { get; set; }
        public int MinimumGuestsServed { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<VolunteerEvent> VolunteerEvents { get; set; }

        public MealSiteViewModel(TheGathering.Web.Models.MealSite mealSite)
        {
            // Transfer Variables
            Id = mealSite.Id;
            AddressLine1 = mealSite.AddressLine1;
            AddressLine2 = mealSite.AddressLine2;
            City = mealSite.City;
            Zipcode = mealSite.Zipcode;
            State = mealSite.State;
            CrossStreet1 = mealSite.CrossStreet1;
            CrossStreet2 = mealSite.CrossStreet2;
            MealServed = mealSite.MealServed;
            DaysServed = mealSite.DaysServed;
            MaximumGuestsServed = mealSite.MaximumGuestsServed;
            MinimumGuestsServed = mealSite.MinimumGuestsServed;
            StartTime = mealSite.StartTime;
            EndTime = mealSite.EndTime;
            VolunteerEvents = mealSite.VolunteerEvents;
        }
    }
}