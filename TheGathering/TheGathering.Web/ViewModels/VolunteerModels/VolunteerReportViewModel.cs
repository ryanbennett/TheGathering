using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheGathering.Web.Models;


namespace TheGathering.Web.ViewModels.VolunteerModels
{
    public class VolunteerReportViewModel
    {
        public Volunteer Volunteer { get; set; }
        public List<VolunteerEvent> VolunteerEvents { get; set; }
        public List<VolunteerEvent> CancelledEvents { get; set; }

    }
}