using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheGathering.Web.Models;

namespace TheGathering.Web.ViewModels.VolunteerGroup
{
    public class GroupEventsListViewModel
    {
        public List<VolunteerGroupVolunteerEvent> VolunteerGroupEvents { get; set; }

        public List<VolunteerEvent> VolunteerEvents { get; set; }


    }
}