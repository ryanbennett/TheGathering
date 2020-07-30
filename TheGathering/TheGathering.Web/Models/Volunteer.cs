using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheGathering.Web.Models
{
    public class Volunteer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool InterestInLeadership { get; set; }
        public bool SignUpForNewsLetter { get; set; }
        public List<VolunteerVolunteerEvent> VolunteerVolunteerEvents { get; set; }
        public string ApplicationUserId { get; set; }
        public bool IsAccountActive { get; set; }

        public Volunteer()
        {
            IsAccountActive = true;
        }
    }
}