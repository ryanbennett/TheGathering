using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    }
}