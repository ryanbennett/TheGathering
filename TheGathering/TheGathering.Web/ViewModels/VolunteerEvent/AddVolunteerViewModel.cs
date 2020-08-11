using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheGathering.Web.Models;

namespace TheGathering.Web.ViewModels
{
    public class AddVolunteerViewModel
    {
        public Models.VolunteerEvent Event { get; set; }

        public List<Volunteer> AvailableVolunteers { get; set; }

        public List<Volunteer> VolunteersToAdd { get; set; }
    }
}