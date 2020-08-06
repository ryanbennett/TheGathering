using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheGathering.Web.Models;

namespace TheGathering.Web.ViewModels.VolunteerGroup
{
    public class VolunteerGroupCalendarViewModel
    {
        public List<Models.VolunteerEvent> VolunteerEvents { get; set; }
        public VolunteerGroupLeader VolunteerGroupLeader { get; set; }
    }
}