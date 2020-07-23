using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheGathering.Web.Models
{
    public class VolunteerGroupVolunteerEvent
    {
        public int Id { get; set; }
        public int VolunteerGroupId { get; set; }
        public int VolunteerEventId { get; set; }
        public int NumberOfGroupMembersSignedUp { get; set; }
        public Boolean Confirmed { get; set; }
    }
}