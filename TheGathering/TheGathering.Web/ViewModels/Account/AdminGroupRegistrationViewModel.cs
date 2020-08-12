using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TheGathering.Web.Models;

namespace TheGathering.Web.ViewModels.Account
{
    public class AdminGroupRegistrationViewModel
    {
        [Required]
        public string LeaderFirstName { get; set; }

        [Required]
        public string LeaderLastName { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public DateTime LeaderBirthday { get; set; }
        public string GroupName { get; set; }
        public string LeaderPhoneNumber { get; set; }
        public bool SignUpForNewsLetter { get; set; }
        public string ApplicationUserId { get; set; }
        public int TotalGroupMembers { get; set; }
    }
}