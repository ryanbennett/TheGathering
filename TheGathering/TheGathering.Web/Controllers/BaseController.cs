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
using System.Web.Security;
using TheGathering.Web.Models;
using TheGathering.Web.Services;

namespace TheGathering.Web.Controllers
{
    public class BaseController : Controller
    {
        ApplicationUserManager _userManager;

        /// <summary>
        /// This is the email that's used for official Gathering emails, used for confirmations and official email
        /// </summary>
        public const string GATHERING_EMAIL = "21ahmeda@elmbrookstudents.org";

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
            var user = UserManager.FindById(User.Identity.GetUserId());
            return user;
        }
        public Volunteer GetCurrentVolunteer()
        {
            var volunteerService = new VolunteerService();
            var volunteer = volunteerService.GetByApplicationUserId(User.Identity.GetUserId());
            return volunteer;
        }
        public VolunteerGroupLeader GetCurrentVolunteerGroupLeader()
        {
            var volunteerGroupService = new VolunteerGroupService();
            var volunteerGroup = volunteerGroupService.GetLeaderByApplicationUserId(User.Identity.GetUserId());
            return volunteerGroup;
        }

        public async Task SendGatheringEmail(string firstName, string email, string subject, string plainTextContent, string htmlContent)
        {
            await ConfirmationEmail(firstName, email, subject, plainTextContent, htmlContent);
        }

        public async Task SendGatheringEmail(List<string> emails, string subject, string plainTextContent, string htmlContent)
        {
            string apiKey = WebConfigurationManager.AppSettings["SendGridEnvironmentalKey"];
            SendGridClient client = new SendGridClient(apiKey);
            EmailAddress from = new EmailAddress(GATHERING_EMAIL, "The Gathering");

            List<EmailAddress> recipients = new List<EmailAddress>(emails.Count);

            foreach (string item in emails)
            {
                recipients.Add(new EmailAddress(item));
            }

            SendGridMessage msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, recipients, subject, plainTextContent, htmlContent);
            await client.SendEmailAsync(msg);
        }


        public async Task ConfirmationEmail(string firstName, string email, string subject, string plainTextContent, string htmlContent) //pass in email, subject, text
        {
            //subject is subject of email
            //PlainTextContent is non-html text of email
            //HtmlContent is a stylized version of plainTextContent
            var apiKey = WebConfigurationManager.AppSettings["SendGridEnvironmentalKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(GATHERING_EMAIL, "The Gathering");
            var to = new EmailAddress(email, firstName);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User.Identity.IsAuthenticated)
            {   //TODO Refactor for other users
                if (User.IsInRole("volunteer"))
                {
                ViewBag.UserFriendlyName = GetCurrentVolunteer().FirstName;
                }
                else if (User.IsInRole("groupleader"))
                {
                    ViewBag.UserFriendlyName = GetCurrentVolunteerGroupLeader().LeaderFirstName;
                }

            }
            base.OnActionExecuting(filterContext);
        }

    }
}