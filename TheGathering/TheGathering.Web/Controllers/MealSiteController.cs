using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using TheGathering.Web.Models;
using TheGathering.Web.Services;
using TheGathering.Web.ViewModels.MealSite;

namespace TheGathering.Web.Controllers
{
    public class MealSiteController : Controller
    {
        private MealSiteService mealSiteService = new MealSiteService();
        private CalendarService volunteerEventService = new CalendarService();

        // GET: MealSite
        public ActionResult Index()
        {
            return View(mealSiteService.GetAllMealSites());
        }

        public ActionResult MultiMapTest()
        {
            return View();
        }

        //These need to updated, these are placeholders
        public ActionResult Edit(int id)
        {
            return View(mealSiteService.GetMealSiteById(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include =
            ("Id, AddressLine1, AddressLine2, City, State, Zipcode, CrossStreet1, " +
            "CrossStreet2, MealServed, DaysServed, MaximumGuestsServed, MinimumGuestsServed, StartTime, EndTime"))] MealSite mealSite)
        {
            if (ModelState.IsValid)
            {
                mealSiteService.UpdateMealSite(mealSite);
                return RedirectToAction("Index");
            }
            return View(mealSite);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include =
            ("Id, AddressLine1, AddressLine2, City, State, Zipcode, CrossStreet1, " +
            "CrossStreet2, MealServed, DaysServed, MaximumGuestsServed, MinimumGuestsServed, StartTime, EndTime"))] MealSite mealSite)
        {
            if (ModelState.IsValid)
            {
                mealSiteService.AddMealSite(mealSite);
                return RedirectToAction("Index");
            }

            return View(mealSite);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var mealSite = mealSiteService.GetMealSiteById((int)id);

            if (mealSite == null)
            {
                return HttpNotFound();
            }

            return View(mealSite);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            mealSiteService.DeleteMealSite(id);
            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MealSite mealSite = mealSiteService.GetMealSiteById((int)id);
            if (mealSite == null)
            {
                return HttpNotFound();
            }
            MealSiteViewModel mealSiteViewModel = new MealSiteViewModel(mealSite);
            mealSiteViewModel.VolunteerEventsAtMealSite = volunteerEventService.GetEventsById(mealSiteViewModel.VolunteerEventIdsAtMealSite);

            return View(mealSiteViewModel);
        }
    }
}