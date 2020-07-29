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
using TheGathering.Web.Services;

namespace TheGathering.Web.Controllers
{
    public class BaseController : Controller
    {
        ApplicationUserManager _userManager;

        /// <summary>
        /// This is the email that's used for official gathering emails, used for confirmations and official email
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

        public async Task SendGatheringEmail(string firstName, string email, string subject, string plainTextContent, string htmlContent) //pass in email, subject, text
        {
            //subject is subject of email
            //PlainTextContent is non-html text of email
            //HtmlContent is a stylized version of plainTextContent
            string apiKey = WebConfigurationManager.AppSettings["SendGridEnvironmentalKey"];
            SendGridClient client = new SendGridClient(apiKey);
            EmailAddress from = new EmailAddress(GATHERING_EMAIL, "The Gathering");
            EmailAddress to = new EmailAddress(email, firstName);

            SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            Response response = await client.SendEmailAsync(msg);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User.Identity.IsAuthenticated)
            {   //TODO Refactor for other users
                //ViewBag.UserFriendlyName = GetCurrentVolunteerGroupLeader().LeaderFirstName;
            }
            base.OnActionExecuting(filterContext);
        }

    }
}