using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TheGathering.Web.Models;
using TheGathering.Web.Service;
using TheGathering.Web.Services;

namespace TheGathering.Web.Controllers
{
    public class AdminPortalController : Controller
    {
        private VolunteerService volunteerService = new VolunteerService();
        private MealSiteService mealService = new MealSiteService();

        // GET: AdminPortal
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Users()
        {
            return View();
        }

        public ActionResult MealSites()
        {
            return View(mealService.GetAllMealSites());
        }

        public ActionResult MealSiteDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MealSite mealSite = mealService.GetMealSiteById((int)id);
            if (mealSite == null)
            {
                return HttpNotFound();
            }
            return View(mealSite);
        }
    }
}