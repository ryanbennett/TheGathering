using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheGathering.Web.Models
{
    public class Volunteer
    {
        public int Id { get; set; }
        [Display(Name = "First Name")]
        public String FirstName { get; set; }
        [Display(Name = "Last Name")]
        public String LastName { get; set; }
        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Birthday { get; set; }
        [Display(Name = "Email")]
        public String Email { get; set; }
        [Display(Name = "Phone Number")]
        public String PhoneNumber { get; set; }
        [Display(Name = "Interest in Leadership")]
        public Boolean InterestInLeadership { get; set; }
        [Display(Name = "Sign Up for Newsletter")]
        public Boolean SignUpForNewsLetter { get; set; }
        public List<VolunteerVolunteerEvent> VolunteerVolunteerEvents { get; set; }
        public string ApplicationUserId { get; set; }

        [Display(Name = "Account Active")]
        public bool IsAccountActive { get; set; }

        public Volunteer()
        {
            IsAccountActive = true;
        }
    }
}