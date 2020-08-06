using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheGathering.Web.Models;

namespace TheGathering.Web.ViewModels.VolunteerModels
{
    public class SignUpEventViewModel
    {
        public Volunteer Volunteer { get; set; }
        public Models.VolunteerEvent VolunteerEvent { get; set; }
        public List<Volunteer> Volunteers { get; set; }
    }
}