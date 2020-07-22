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
        public VolunteerEvent VolunteerEvent { get; set; }
    }
}