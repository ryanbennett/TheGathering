using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheGathering.Web.Models;
namespace TheGathering.Web.ViewModels.VolunteerModels
{
    public class UserEventsListViewModel
    {
        public List<VolunteerEvent> VolunteerEvents { get; set; }
        public List<VolunteerEvent> CancelledEvents { get; set; }
        public List<VolunteerEvent> CurrentEvents { get; set; }
        public List<VolunteerEvent> PastEvents { get; set; }

    }
}