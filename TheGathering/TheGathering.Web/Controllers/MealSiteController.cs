using Microsoft.Owin.Security.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using TheGathering.Web.Models;
using TheGathering.Web.Services;
using TheGathering.Web.ViewModels.MealSite;

namespace TheGathering.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class MealSiteController : Controller
    {
        private MealSiteService mealSiteService = new MealSiteService();
        private CalendarService volunteerEventService = new CalendarService();

        public const string ERROR_SEPARATOR = " <br>";

        // GET: MealSite
        [AllowAnonymous]
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
                List<string> allErrors = viewModel.GetBreakfastValidationErrors();
                allErrors.AddRange(viewModel.GetDinnerValidationErrors());
                allErrors.AddRange(viewModel.GetLunchValidationErrors());

                if (allErrors.Count > 0)
                {
                    StringBuilder str = new StringBuilder();

                    foreach (string error in allErrors)
                    {
                        str.Append(error);
                        str.Append(ERROR_SEPARATOR);
                    }

                    viewModel.Error = str.ToString();

                    return View(viewModel);
                }

                MealSite mealSite = new MealSite(viewModel);
                mealSiteService.UpdateMealSite(mealSite);
                return RedirectToAction("MealSites", "AdminPortal", null);
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
                List<string> allErrors = viewModel.GetBreakfastValidationErrors();
                allErrors.AddRange(viewModel.GetDinnerValidationErrors());
                allErrors.AddRange(viewModel.GetLunchValidationErrors());

                if (allErrors.Count > 0)
                {
                    StringBuilder str = new StringBuilder();

                    foreach (string error in allErrors)
                    {
                        str.Append(error);
                        str.Append(ERROR_SEPARATOR);
                    }

                    viewModel.Error = str.ToString();

                    return View(viewModel);
                }

                MealSite mealSite = new MealSite(viewModel);
                mealSiteService.AddMealSite(mealSite);
                return RedirectToAction("MealSites", "AdminPortal", null);

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

        [AllowAnonymous]
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