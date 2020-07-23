using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheGathering.Web.Models;

namespace TheGathering.Web.ViewModels.VolunteerGroup
{
    public class SignUpGroupViewModel
    {
        public VolunteerGroupLeader VolunteerGroupLeader { get; set; }
        public VolunteerEvent VolunteerEvent { get; set; }

        public int VolunteerSlots { get; set; }
    }
}