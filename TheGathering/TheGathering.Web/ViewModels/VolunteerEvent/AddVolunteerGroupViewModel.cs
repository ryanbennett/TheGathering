using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheGathering.Web.Models;

namespace TheGathering.Web.ViewModels
{
    public class AddVolunteerGroupViewModel
    {
        public Models.VolunteerEvent Event { get; set; }

        public List<VolunteerGroupLeader> AvailableVolunteerGroups { get; set; }

        public List<VolunteerGroupLeader> VolunteerGroupsToAdd { get; set; }
    }
}