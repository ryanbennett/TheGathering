using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheGathering.Web.Models;

namespace TheGathering.Web.ViewModels.VolunteerModels
{
    public class VolunteerViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool InterestInLeadership { get; set; }
        public bool SignUpForNewsLetter { get; set; }
        public List<VolunteerEvent> VolunteerEvents { get; set; }
        public string ApplicationUserId { get; set; }

        public VolunteerViewModel(Volunteer volunteer)
        {
            Id = volunteer.Id;
            FirstName = volunteer.FirstName;
            LastName = volunteer.LastName;
            Birthday = volunteer.Birthday;
            Email = volunteer.Email;
            PhoneNumber = volunteer.PhoneNumber;
            InterestInLeadership = volunteer.InterestInLeadership;
            SignUpForNewsLetter = volunteer.SignUpForNewsLetter;
            ApplicationUserId = volunteer.ApplicationUserId;
        }
    }
}