using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data;
using TheGathering.Web.Models;
using TheGathering.Web.Services;

namespace TheGathering.Web.Controllers
{
    public class VolunteerEventController : Controller
    {
        private CalendarService service = new CalendarService();
        private MealSiteService mealSiteService = new MealSiteService();

        public const string INVALID_CALENDER_DATES_ERROR = "The given calender dates are incorrect, make sure the start date is earlier than the end date.";

        // GET: VolunteerEvent
        public ActionResult Index()
        {
            List<VolunteerEvent> events = service.GetAllEvents();

            return View(events);
        }

        public ActionResult Create()
        {
            VolunteerEventViewModel model = new VolunteerEventViewModel();
            model.DropDownItems = AllLocations();
            return View(model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolunteerEvent toBeDeleted = service.GetEventById((int)id);
            if (toBeDeleted == null)
            {
                return HttpNotFound();
            }

            toBeDeleted.MealSite = mealSiteService.GetMealSiteById(toBeDeleted.MealSite_Id);

            return View(toBeDeleted);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VolunteerEvent toBeDeleted = service.GetEventById(id);
            service.DeleteEvent(toBeDeleted);
            //mealSiteService.DeleteVolunteerEvent(toBeDeleted.Id, toBeDeleted.MealSite.Id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VolunteerEventViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.StartingShiftTime.CompareTo(viewModel.EndingShiftTime) < 0)
                {
                    VolunteerEvent volunteerEvent = new VolunteerEvent(viewModel);
                    volunteerEvent.MealSite_Id = viewModel.MealSiteId;
                    service.AddEvent(volunteerEvent);

                    return RedirectToAction("Index");
                }

                viewModel.Error = INVALID_CALENDER_DATES_ERROR;
                viewModel.DropDownItems = AllLocations();
                return View(viewModel);
            }

            return View(viewModel);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolunteerEvent volunteerEvent = service.GetEventById((int)id);

            if (volunteerEvent == null)
            {
                return HttpNotFound();
            }

            volunteerEvent.MealSite = mealSiteService.GetMealSiteById(volunteerEvent.MealSite_Id);

            return View(volunteerEvent);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolunteerEvent volunteerEvent = service.GetEventById((int)id);

            if (volunteerEvent == null)
            {
                return HttpNotFound();
            }

            VolunteerEventViewModel viewModel = new VolunteerEventViewModel(volunteerEvent)
            {
                DropDownItems = AllLocations(),
                MealSite = mealSiteService.GetMealSiteById(volunteerEvent.MealSite_Id)
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StartingShiftTime, EndingShiftTime, OpenSlots, Location, Description")] VolunteerEventViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.StartingShiftTime.CompareTo(viewModel.EndingShiftTime) < 0)
                {
                    VolunteerEvent volunteerEvent = new VolunteerEvent(viewModel);
                    service.SaveEdits(volunteerEvent);

                    return RedirectToAction("Index");
                }

                viewModel.Error = INVALID_CALENDER_DATES_ERROR;
                viewModel.DropDownItems = AllLocations();
                return View(viewModel);
            }
            return View(viewModel);
        }

        public ActionResult Calendar()
        {
            return View();
        }

        public JsonResult GetEvents()
        {
            var events = service.GetAllEvents();
            return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public List<SelectListItem> AllLocations()
        {
            List<MealSite> AllLocations = mealSiteService.GetAllMealSites();
            List<SelectListItem> Locations = new List<SelectListItem>();

            foreach (MealSite mealSite in AllLocations)
            {
                SelectListItem item = new SelectListItem
                {
                    Text = mealSite.AddressLine1,
                    Value = mealSite.Id.ToString()
                };

                Locations.Add(item);
            }
            return Locations;
        }
    }
}