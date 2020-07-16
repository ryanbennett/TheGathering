using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace TheGathering.Web.Models
{
    public class VolunteerEvent
    {
        public int Id { get; set; }
        [Display(Name = "Starting Shift Time")]
        public DateTime StartingShiftTime { get; set; }

        [Display(Name = "Ending Shift Time")]
        public DateTime EndingShiftTime { get; set; }
        [Display(Name = "Open Slots")]
        public int OpenSlots { get; set; }
        public MealSite Location { get; set; }
       
        [NotMapped]
        public List<SelectListItem> AllLocations { get; set; }

        public string Description { get; set; }
        public List<VolunteerVolunteerEvent> VolunteerVolunteerEvents { get; set; }
    }
}