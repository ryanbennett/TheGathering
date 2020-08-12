using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheGathering.Web.Models;

namespace TheGathering.Web.ViewModels.VolunteerGroup
{
    public class GroupNumberEventsListViewModel
    {
        public List<VolunteerGroupVolunteerEvent> VolunteerGroupEvents { get; set; }
        public List<Models.VolunteerEvent> VolunteerEvents { get; set; }
        //public List<Models.VolunteerEvent> CancelledEvents { get; set; }
        public List<Models.VolunteerGroupVolunteerEvent> CurrentEvents { get; set; }
        public List<Models.VolunteerGroupVolunteerEvent> PastEvents { get; set; }
        public VolunteerGroupLeader volunteerGroupLeader { get; set; }
    }
}