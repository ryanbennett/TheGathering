using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using TheGathering.Web.Models;
using TheGathering.Web.Service;

namespace TheGathering.Web.Controllers
{
    public class BaseController : Controller
    {
        ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ApplicationUser GetCurrentUser()
        {
            var VolunteerService = new VolunteerService();
            var user1 = UserManager.FindById(User.Identity.GetUserId());
            return user1;
        }
        public Volunteer GetCurrentVolunteer()
        {
            var VolunteerService = new VolunteerService();
            var volunteer1 = VolunteerService.GetByApplicationUserId(User.Identity.GetUserId());
            return volunteer1;
        }


        public async Task Email() //pass in email, subject, text
        {
            var apiKey = WebConfigurationManager.AppSettings["SendGridEnvironmentalKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("21ahmeda@elmbrookstudents.org", "The Gathering");
            var subject = "The Gathering Email Confirmation";
            var to = new EmailAddress("thomrya22@outlook.com", "Ryan");
            var plainTextContent = "You have registered with The Gathering!";
            var htmlContent = "<strong>You have registered with The Gathering!</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}