using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheGathering.Web.Models
{
    public class VolunteerGroupLeader
    {
        public int Id { get; set; }
        [Display(Name = "Leader First Name")]
        public string LeaderFirstName { get; set; }
        [Display(Name = "Leader Last Name")]
        public string LeaderLastName { get; set; }
        [Display(Name = "Leader D.O.B.")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LeaderBirthday { get; set; }
        [Display(Name = "Leader Email")]
        public string LeaderEmail { get; set; }
        [Display(Name = "Leader Phone Number")]
        public string LeaderPhoneNumber { get; set; }
        [Display(Name = "Group Name")]
        public string GroupName { get; set; }
        [Display(Name = "Newsletter")]
        public Boolean SignUpForNewsLetter { get; set; }
        public List<VolunteerGroupVolunteerEvent> VolunteerGroupVolunteerEvents { get; set; } = new List<VolunteerGroupVolunteerEvent>();
        public String ApplicationUserId { get; set; }
        [Display(Name = "Total Group Members")]
        public int TotalGroupMembers { get; set; }
        
        
    }
}