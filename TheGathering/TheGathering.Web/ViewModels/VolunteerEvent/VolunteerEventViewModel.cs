using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheGathering.Web.Models
{
    public class VolunteerEventViewModels
    {
        public int Id { get; set; }
        [Display(Name = "Starting Shift Time")]
        public DateTime StartingShiftTime { get; set; }

        [Display(Name = "Ending Shift Time")]
        public DateTime EndingShiftTime { get; set; }
        [Display(Name = "Open Slots")]
        public int OpenSlots { get; set; }

        public List<SelectListItem> DropDownItems { get; set; }
        public int MealSiteId { get; set; }

        public MealSite MealSite { get; set; }

        public string Description { get; set; }

        public List<VolunteerGroupVolunteerEvent> VolunteerGroupVolunteerEvents { get; set; }

        public List<VolunteerVolunteerEvent> VolunteerVolunteerEvents { get; set; } = new List<VolunteerVolunteerEvent>();

        public List<Volunteer> SignedUpVolunteers { get; set; } = new List<Volunteer>();

        /// <summary>
        /// This will be empty if there is no error, allows us to create an error for validating input information
        /// </summary>
        public string Error { get; set; }

        public VolunteerEventViewModel() { }

        public Volunteer volunteer { get; set; }

        public VolunteerEventViewModel(VolunteerEvent volunteerEvent)
        {
            Id = volunteerEvent.Id;
            StartingShiftTime = volunteerEvent.StartingShiftTime;
            EndingShiftTime = volunteerEvent.EndingShiftTime;
            OpenSlots = volunteerEvent.OpenSlots;
            MealSite = volunteerEvent.MealSite;
            MealSiteId = volunteerEvent.MealSite_Id;
            Description = volunteerEvent.Description;
            VolunteerVolunteerEvents = volunteerEvent.VolunteerVolunteerEvents;
        }
    }
}