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

        public const string INVALID_NUMBER_OF_GUESTS_ERROR = "The given number of guests are incorrect, make sure the maximum number of guests are greater than the minimum number of guests ";
        public const string INVALID_MEALSITE_TIME_ERROR = "The selected mealsite time is incorrect, make sure the start time is before the end time ";

        public const string BREAKFAST_ADDON = "in breakfast seection";
        public const string LUNCH_ADDON = "in lunch seection";
        public const string DINNER_ADDON = "in dinner seection";


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
                    return RedirectToAction("MealSites", "AdminPortal", null);
                }
                else if (viewModel.ValidateBreakfastData() == false)
                {
                    if (viewModel.Breakfast_MaximumGuestsServed < viewModel.Breakfast_MinimumGuestsServed | viewModel.Breakfast_MinimumGuestsServed < 0 | viewModel.Breakfast_MaximumGuestsServed < 0)
                    {
                        viewModel.Error = INVALID_NUMBER_OF_GUESTS_ERROR + BREAKFAST_ADDON;
                        return View(viewModel);
                    }

                    viewModel.Error = INVALID_MEALSITE_TIME_ERROR + BREAKFAST_ADDON;
                    return View(viewModel);
                }
                else if (viewModel.ValidateLunchData() == false)
                {
                    if (viewModel.Lunch_MaximumGuestsServed < viewModel.Lunch_MinimumGuestsServed | viewModel.Lunch_MinimumGuestsServed < 0 | viewModel.Lunch_MaximumGuestsServed < 0)
                    {
                        viewModel.Error = INVALID_NUMBER_OF_GUESTS_ERROR + LUNCH_ADDON;
                        return View(viewModel);
                    }

                    viewModel.Error = INVALID_MEALSITE_TIME_ERROR + LUNCH_ADDON;
                    return View(viewModel);

                }
                else if (viewModel.ValidateDinnerData() == false)
                {
                    if (viewModel.Dinner_MaximumGuestsServed < viewModel.Dinner_MinimumGuestsServed | viewModel.Dinner_MinimumGuestsServed < 0 | viewModel.Dinner_MaximumGuestsServed < 0)
                    {
                        viewModel.Error = INVALID_NUMBER_OF_GUESTS_ERROR + DINNER_ADDON;
                        return View(viewModel);
                    }

                    viewModel.Error = INVALID_MEALSITE_TIME_ERROR + DINNER_ADDON;
                    return View(viewModel);
                }
                //viewModel.Error = INVALID_NUMBER_OF_GUESTS_ERROR;
                //return View(viewModel);
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
                    return RedirectToAction("MealSites", "AdminPortal", null);
                }

                viewModel.Error = INVALID_NUMBER_OF_GUESTS_ERROR;
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
            MealSiteViewModel viewModel = new MealSiteViewModel(MealSite);
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            mealSiteService.DeleteMealSite(id);
            return RedirectToAction("MealSites", "AdminPortal", null);
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