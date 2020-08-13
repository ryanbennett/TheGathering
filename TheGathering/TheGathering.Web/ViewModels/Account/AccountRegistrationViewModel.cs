using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TheGathering.Web.Models;

namespace TheGathering.Web.ViewModels.Account
{

    public class AccountRegistrationViewModel : RegisterViewModel
    { //For more data validation look at AccountViewModels-Register
        [Required] //This makes the FirstName field required
        [Display(Name = "First Name")]
        public String FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public String LastName { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Birthday { get; set; }
        [Display(Name = "Phone Number")]
        public String PhoneNumber { get; set; }
        public bool InterestInLeadership { get; set; }
        public bool SignUpForNewsLetter { get; set; }
        public string ApplicationUserId { get; set; }
    }
}