using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security;
using System.Web;
using TheGathering.Web.Models;
using TheGathering.Web.Services;

namespace TheGathering.Web.ViewModels.MealSite
{
    public class MealSiteViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Display(Name = "Street")]
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsTheGatheringSite { get; set; }
        [Display(Name = "Is Meal Site Active?" )]
        public bool IsMealSiteActive { get; set; }

        /*
         *      Breakfast, Lunch, and Dinner Meals
         */

        // Breakfast
        public bool Breakfast_Used { get; set; }

        [Display(Name = "Days Served")]
        public List<bool> Breakfast_DaysServed { get; set; }

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
        public List<bool> Lunch_DaysServed { get; set; }

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
        public List<bool> Dinner_DaysServed { get; set; }

        [Display(Name = "Maximum Guests Served")]
        public int? Dinner_MaximumGuestsServed { get; set; }

        [Display(Name = "Minimum Guests Served")]
        public int? Dinner_MinimumGuestsServed { get; set; }

        [Display(Name = "Start Time")]
        public string Dinner_StartTime { get; set; }

        [Display(Name = "End Time")]
        public string Dinner_EndTime { get; set; }

        public List<VolunteerEvent> VolunteerEvents { get; set; }


        public const string INVALID_NUMBER_OF_GUESTS_ERROR = "Minimum guests must be smaller than or equal to maximum guests";
        public const string GUESTS_NEGATIVE_ERROR = "Number of guests cannot be negative";
        public const string INVALID_MEALSITE_TIME_ERROR = "The selected mealsite time is incorrect, make sure the start time is before the end time";
        public const string NO_MEALSITE_TIME_ERROR = "Selected meals must have a given time";
        public const string NO_DAYS_SELECTED_ERROR = "No days have been selected";
        public const string UNKNOWN_ERROR = "Unknown error";

        public const string BREAKFAST_ADDON = " in breakfast section.";
        public const string LUNCH_ADDON = " in lunch section.";
        public const string DINNER_ADDON = " in dinner section.";

        /// <summary>
        /// This will be empty if there is no error, allows us to create an error for validating input information
        /// </summary>
        public string Error { get; set; }

        public MealSiteViewModel()
        {
            Breakfast_DaysServed = new List<bool>();
            Lunch_DaysServed = new List<bool>();
            Dinner_DaysServed = new List<bool>();
            IsMealSiteActive = true;
        }

        public MealSiteViewModel(Models.MealSite mealSite)
        {
            // Transfer Variables
            Id = mealSite.Id;
            Name = mealSite.Name;
            AddressLine1 = mealSite.AddressLine1;
            City = mealSite.City;
            Zipcode = mealSite.Zipcode;
            State = mealSite.State;
            Latitude = mealSite.Latitude;
            Longitude = mealSite.Longitude;
            IsTheGatheringSite = mealSite.IsTheGatheringSite;
            IsMealSiteActive = mealSite.IsMealSiteActive;

            Breakfast_Used = mealSite.Breakfast_Used;
            Breakfast_DaysServed = JsonConvert.DeserializeObject<List<bool>>(mealSite.Breakfast_DaysServed);
            Breakfast_MaximumGuestsServed = mealSite.Breakfast_MaximumGuestsServed;
            Breakfast_MinimumGuestsServed = mealSite.Breakfast_MinimumGuestsServed;
            Breakfast_StartTime = mealSite.Breakfast_StartTime;
            Breakfast_EndTime = mealSite.Breakfast_EndTime;

            Lunch_Used = mealSite.Lunch_Used;
            Lunch_DaysServed = JsonConvert.DeserializeObject<List<bool>>(mealSite.Lunch_DaysServed);
            Lunch_MaximumGuestsServed = mealSite.Lunch_MaximumGuestsServed;
            Lunch_MinimumGuestsServed = mealSite.Lunch_MinimumGuestsServed;
            Lunch_StartTime = mealSite.Lunch_StartTime;
            Lunch_EndTime = mealSite.Lunch_EndTime;

            Dinner_Used = mealSite.Dinner_Used;
            Dinner_DaysServed = JsonConvert.DeserializeObject<List<bool>>(mealSite.Dinner_DaysServed);
            Dinner_MaximumGuestsServed = mealSite.Dinner_MaximumGuestsServed;
            Dinner_MinimumGuestsServed = mealSite.Dinner_MinimumGuestsServed;
            Dinner_StartTime = mealSite.Dinner_StartTime;
            Dinner_EndTime = mealSite.Dinner_EndTime;

            if (VolunteerEvents != null)
            {
                VolunteerEvents = mealSite.VolunteerEvents.ToList();
            }
        }

        #region Validation Functions
        public List<string> GetBreakfastValidationErrors()
        {
            List<ValidationError> errors = ValidateBreakfastData();
            List<string> stringErrors = new List<string>();

            foreach (var item in errors)
            {
                switch (item)
                {
                    case ValidationError.NoDaysSelected:
                        stringErrors.Add(NO_DAYS_SELECTED_ERROR + BREAKFAST_ADDON);
                        break;

                    case ValidationError.GuestsServedMinGreaterThanMax:
                        stringErrors.Add(INVALID_NUMBER_OF_GUESTS_ERROR + BREAKFAST_ADDON);
                        break;

                    case ValidationError.GuestsServedIsNegative:
                        stringErrors.Add(GUESTS_NEGATIVE_ERROR + BREAKFAST_ADDON);
                        break;

                    case ValidationError.NullTimes:
                        stringErrors.Add(NO_MEALSITE_TIME_ERROR + BREAKFAST_ADDON);
                        break;

                    case ValidationError.StartLaterThanEnd:
                        stringErrors.Add(INVALID_MEALSITE_TIME_ERROR + BREAKFAST_ADDON);
                        break;

                    default:
                        stringErrors.Add(UNKNOWN_ERROR + BREAKFAST_ADDON);
                        break;
                }
            }

            return stringErrors;
        }

        /// <summary>
        /// Checks the breakfast data to see if it's invalid. If breakfast is not used then any data related to breakfast is dropped.
        /// If the data is used and valid it returns true, if it's invalid it returns false
        /// </summary>
        /// <returns>All errors from validation</returns>
        public List<ValidationError> ValidateBreakfastData()
        {
            if (!Breakfast_Used)
            {
                Breakfast_StartTime = null;
                Breakfast_EndTime = null;
                Breakfast_DaysServed = null;
                Breakfast_MaximumGuestsServed = null;
                Breakfast_MinimumGuestsServed = null;
                return new List<ValidationError>();
            }

            List<ValidationError> errors = new List<ValidationError>();

            if (Breakfast_StartTime == null || Breakfast_EndTime == null) { errors.Add(ValidationError.NullTimes); }

            else
            {
                int startInt = int.Parse(Breakfast_StartTime.Replace(":", ""));
                int endInt = int.Parse(Breakfast_EndTime.Replace(":", ""));

                if (startInt >= endInt)
                {
                    errors.Add(ValidationError.StartLaterThanEnd);
                }
            }

            bool daysValid = false;

            foreach (bool item in Breakfast_DaysServed)
            {
                if (item)
                {
                    daysValid = true;
                    break;
                }
            }

            if (!daysValid)
            {
                errors.Add(ValidationError.NoDaysSelected);
            }

            if (Breakfast_MinimumGuestsServed > Breakfast_MaximumGuestsServed)
            {
                errors.Add(ValidationError.GuestsServedMinGreaterThanMax);
            }

            if (Breakfast_MinimumGuestsServed < 0 || Breakfast_MaximumGuestsServed < 0)
            {
                errors.Add(ValidationError.GuestsServedIsNegative);
            }

            return errors;
        }

        public List<string> GetLunchValidationErrors()
        {
            List<ValidationError> errors = ValidateLunchData();
            List<string> stringErrors = new List<string>();

            foreach (var item in errors)
            {
                switch (item)
                {
                    case ValidationError.NoDaysSelected:
                        stringErrors.Add(NO_DAYS_SELECTED_ERROR + LUNCH_ADDON);
                        break;

                    case ValidationError.GuestsServedMinGreaterThanMax:
                        stringErrors.Add(INVALID_NUMBER_OF_GUESTS_ERROR + LUNCH_ADDON);
                        break;

                    case ValidationError.GuestsServedIsNegative:
                        stringErrors.Add(GUESTS_NEGATIVE_ERROR + LUNCH_ADDON);
                        break;

                    case ValidationError.NullTimes:
                        stringErrors.Add(NO_MEALSITE_TIME_ERROR + LUNCH_ADDON);
                        break;

                    case ValidationError.StartLaterThanEnd:
                        stringErrors.Add(INVALID_MEALSITE_TIME_ERROR + LUNCH_ADDON);
                        break;

                    default:
                        stringErrors.Add(UNKNOWN_ERROR + LUNCH_ADDON);
                        break;
                }
            }

            return stringErrors;
        }

        /// <summary>
        /// Checks the lunch data to see if it's invalid. If lunch is not used then any data related to lunch is dropped.
        /// If the data is used and valid it returns true, if it's invalid it returns false
        /// </summary>
        /// <returns>All errors from validation</returns>
        public List<ValidationError> ValidateLunchData()
        {
            if (!Lunch_Used)
            {
                Lunch_StartTime = null;
                Lunch_EndTime = null;
                Lunch_DaysServed = null;
                Lunch_MaximumGuestsServed = null;
                Lunch_MinimumGuestsServed = null;
                return new List<ValidationError>();
            }

            List<ValidationError> errors = new List<ValidationError>();

            if (Lunch_StartTime == null || Lunch_EndTime == null) { errors.Add(ValidationError.NullTimes); }

            else
            {
                int startInt = int.Parse(Lunch_StartTime.Replace(":", ""));
                int endInt = int.Parse(Lunch_EndTime.Replace(":", ""));

                if (startInt >= endInt)
                {
                    errors.Add(ValidationError.StartLaterThanEnd);
                }
            }

            bool daysValid = false;

            foreach (bool item in Lunch_DaysServed)
            {
                if (item)
                {
                    daysValid = true;
                    break;
                }
            }

            if (!daysValid)
            {
                errors.Add(ValidationError.NoDaysSelected);
            }

            if (Lunch_MinimumGuestsServed > Lunch_MaximumGuestsServed)
            {
                errors.Add(ValidationError.GuestsServedMinGreaterThanMax);
            }

            if (Lunch_MinimumGuestsServed < 0 || Lunch_MaximumGuestsServed < 0)
            {
                errors.Add(ValidationError.GuestsServedIsNegative);
            }

            return errors;
        }

        public List<string> GetDinnerValidationErrors()
        {
            List<ValidationError> errors = ValidateDinnerData();
            List<string> stringErrors = new List<string>();

            foreach (var item in errors)
            {
                switch (item)
                {
                    case ValidationError.NoDaysSelected:
                        stringErrors.Add(NO_DAYS_SELECTED_ERROR + DINNER_ADDON);
                        break;

                    case ValidationError.GuestsServedMinGreaterThanMax:
                        stringErrors.Add(INVALID_NUMBER_OF_GUESTS_ERROR + DINNER_ADDON);
                        break;

                    case ValidationError.GuestsServedIsNegative:
                        stringErrors.Add(GUESTS_NEGATIVE_ERROR + DINNER_ADDON);
                        break;

                    case ValidationError.NullTimes:
                        stringErrors.Add(NO_MEALSITE_TIME_ERROR + DINNER_ADDON);
                        break;

                    case ValidationError.StartLaterThanEnd:
                        stringErrors.Add(INVALID_MEALSITE_TIME_ERROR + DINNER_ADDON);
                        break;

                    default:
                        stringErrors.Add(UNKNOWN_ERROR + DINNER_ADDON);
                        break;
                }
            }

            return stringErrors;
        }

        /// <summary>
        /// Checks the dinner data to see if it's invalid. If dinner is not used then any data related to dinner is dropped.
        /// If the data is used and valid it returns true, if it's invalid it returns false
        /// </summary>
        /// <returns>All errors from validation</returns>
        public List<ValidationError> ValidateDinnerData()
        {
            if (!Dinner_Used)
            {
                Dinner_StartTime = null;
                Dinner_EndTime = null;
                Dinner_DaysServed = null;
                Dinner_MaximumGuestsServed = null;
                Dinner_MinimumGuestsServed = null;
                return new List<ValidationError>();
            }

            List<ValidationError> errors = new List<ValidationError>();

            if (Dinner_StartTime == null || Dinner_EndTime == null) { errors.Add(ValidationError.NullTimes); }

            else
            {
                int startInt = int.Parse(Dinner_StartTime.Replace(":", ""));
                int endInt = int.Parse(Dinner_EndTime.Replace(":", ""));

                if (startInt >= endInt)
                {
                    errors.Add(ValidationError.StartLaterThanEnd);
                }
            }

            bool daysValid = false;

            foreach (bool item in Dinner_DaysServed)
            {
                if (item)
                {
                    daysValid = true;
                    break;
                }
            }

            if (!daysValid)
            {
                errors.Add(ValidationError.NoDaysSelected);
            }

            if (Dinner_MinimumGuestsServed > Dinner_MaximumGuestsServed)
            {
                errors.Add(ValidationError.GuestsServedMinGreaterThanMax);
            }

            if (Dinner_MinimumGuestsServed < 0 || Dinner_MaximumGuestsServed < 0)
            {
                errors.Add(ValidationError.GuestsServedIsNegative);
            }

            return errors;
        }

        #endregion
    }

    public enum ValidationError
    {
        /// <summary>
        /// No days have been selected
        /// </summary>
        NoDaysSelected,
        /// <summary>
        /// Minimum guests served is larger than the maximum guests served
        /// </summary>
        GuestsServedMinGreaterThanMax,
        /// <summary>
        /// Input guests served are negative
        /// </summary>
        GuestsServedIsNegative,
        /// <summary>
        /// When the times are null
        /// </summary>
        NullTimes,
        /// <summary>
        /// When the start time is later than the end time
        /// </summary>
        StartLaterThanEnd
    }
}