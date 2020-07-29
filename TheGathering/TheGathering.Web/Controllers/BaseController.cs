﻿using Microsoft.AspNet.Identity;
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

        public async Task ConfirmationEmail(String firstName, String email, String Subject, String PlainTextContent, String HtmlContent) //pass in email, subject, text
        {
            //subject is subject of email
            //PlainTextContent is non-html text of email
            //HtmlContent is a stylized version of plainTextContent
            var apiKey = WebConfigurationManager.AppSettings["SendGridEnvironmentalKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("21ahmeda@elmbrookstudents.org", "The Gathering");
            var subject = Subject;
            var to = new EmailAddress(email, firstName);
            var plainTextContent = PlainTextContent;
            var htmlContent = HtmlContent;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.UserFriendlyName = GetCurrentVolunteer().FirstName;
            }
            base.OnActionExecuting(filterContext);
        }

    }
}