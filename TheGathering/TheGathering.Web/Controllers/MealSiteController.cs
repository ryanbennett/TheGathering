﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            return View(mealSiteService.GetAllMealSites());
        }

        //These need to updated, these are placeholders
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            MealSite MealSite = mealSiteService.GetMealSiteById((int)id);
            if (MealSite == null)
            {
                return HttpNotFound();
            }
            return View(MealSite);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include=(MealSite.IncludeBind))] MealSite mealSite)
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
        public ActionResult Create([Bind(Include = (MealSite.IncludeBind))] MealSite mealSite)
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
            var MealSite = mealSiteService.GetMealSiteById((int)id);

            if (MealSite == null)
            {
                return HttpNotFound();
            }

            return View(MealSite);
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
            MealSite MealSite = mealSiteService.GetMealSiteById((int)id);
            if (MealSite == null)
            {
                return HttpNotFound();
            }
            return View(MealSite);
        }
    }
}