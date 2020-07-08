using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using TheGathering.Web.Models;
using TheGathering.Web.Services;

namespace TheGathering.Web.Controllers
{
    public class MealSiteController : Controller
    {
        private MealSiteService mealSiteService = new MealSiteService();

        // GET: MealSite
        public ActionResult Index()
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

        public ActionResult Delete()
        {
            return View();
        }

        public ActionResult Details()
        {
            return View();
        }
    }
}