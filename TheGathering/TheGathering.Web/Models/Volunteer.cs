using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheGathering.Web.Models
{
    public class Volunteer
    {
        public int Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateTime Birthday { get; set; }
        public String Email { get; set; }
        public String PhoneNumber { get; set; }
        public Boolean InterestInLeadership { get; set; }
        public Boolean SignUpForNewsLetter { get; set; }
        public String ApplicationUserId { get; set; }
    }
}