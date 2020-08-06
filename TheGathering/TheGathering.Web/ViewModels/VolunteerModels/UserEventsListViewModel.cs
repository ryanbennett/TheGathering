using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheGathering.Web.Models;
namespace TheGathering.Web.ViewModels.VolunteerModels
{
    public class UserEventsListViewModel
    {
        public List<Models.VolunteerEvent> VolunteerEvents { get; set; }
        public List<Models.VolunteerEvent> CancelledEvents { get; set; }
        public List<Models.VolunteerEvent> CurrentEvents { get; set; }
        public List<Models.VolunteerEvent> PastEvents { get; set; }
        public Volunteer Volunteer { get; set; }

    }
}