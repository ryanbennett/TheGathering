using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheGathering.Web.Models;

namespace TheGathering.Web.ViewModels
{
    public class VolunteerCalendarViewModel
    {
        public List<Models.VolunteerEvent> VolunteerEvents { get; set; }
        public Volunteer Volunteer { get; set; }
    }
}