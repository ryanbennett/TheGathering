using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TheGathering.Web.Models;

namespace TheGathering.Web.ViewModels.Account
{
    public class GroupRegistrationViewModel : RegisterViewModel
    {
        [Required] //This makes the FirstName field required
        [Display(Name = "Leader First Name")]
        public string LeaderFirstName { get; set; }
        [Required]
        [Display(Name = "Leader Last Name")]
        public string LeaderLastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Display(Name = "Leader D.O.B.")]
        public DateTime LeaderBirthday { get; set; }
        [Display(Name = "Group Name")]
        public string GroupName { get; set; }
        [Display(Name = "Leader Phone Number")]
        public string LeaderPhoneNumber { get; set; }
        public bool SignUpForNewsLetter { get; set; }
        public string ApplicationUserId { get; set; }
        [Display(Name = "Total Group Members")]
        public int TotalGroupMembers { get; set; }
    }
}