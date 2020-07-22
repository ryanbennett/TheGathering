using Microsoft.Owin.Security.Provider;
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

        public const string INVALID_CALENDER_DATES_ERROR = "The given calender dates are incorrect, make sure the start date is earlier than the end date.";

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
            return View(new MealSiteViewModel(MealSite));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include=MealSite.IncludeBind)] MealSiteViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.ValidateAllData())
                {
                    MealSite mealSite = new MealSite(viewModel);
                    mealSiteService.UpdateMealSite(mealSite);
                    return RedirectToAction("Index");
                }

                viewModel.Error = INVALID_CALENDER_DATES_ERROR;
                return View(viewModel);
            }

            return View(viewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = (MealSite.IncludeBind))] MealSiteViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.ValidateAllData())
                {
                    MealSite mealSite = new MealSite(viewModel);
                    mealSiteService.AddMealSite(mealSite);
                    return RedirectToAction("Index");
                }

                viewModel.Error = INVALID_CALENDER_DATES_ERROR;
                return View(viewModel);
            }

            return View(viewModel);
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
            MealSite mealSite = mealSiteService.GetMealSiteById((int)id);

            if (mealSite == null)
            {
                return HttpNotFound();
            }

            MealSiteViewModel mealSiteViewModel = new MealSiteViewModel(mealSite)
            {
                VolunteerEvents = mealSite.VolunteerEvents
            };

            return View(mealSiteViewModel);
        }
    }
}