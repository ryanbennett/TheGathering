using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheGathering.Web.Models
{
    public class VolunteerGroupVolunteerEvent
    {
        public int Id { get; set; }
        public int VolunteerGroupId { get; set; }
        public int VolunteerEventId { get; set; }
        public VolunteerEvent VolunteerEvent { get; set; }

        //[Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        [Display(Name = "Number of Group Members Signed Up")]
        public int NumberOfGroupMembersSignedUp { get; set; }
        public Boolean Confirmed { get; set; }
    }
}