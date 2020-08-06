using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheGathering.Web.Models;

namespace TheGathering.Web.ViewModels.VolunteerGroup
{
    public class SignUpGroupViewModel
    {
        public int VolunteerGroupLeaderID { get; set; }
        public int VolunteerEventID { get; set; }
        public VolunteerGroupLeader VolunteerGroupLeader { get; set; }
        public Models.VolunteerEvent VolunteerEvent { get; set; }
        public int VolunteerSlots { get; set; }
    }
}