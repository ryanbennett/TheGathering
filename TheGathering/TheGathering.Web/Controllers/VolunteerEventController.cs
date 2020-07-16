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

        // GET: VolunteerEvent
        public ActionResult Index()
        {
            List<VolunteerEvent> events = service.GetAllEvents();

            foreach (VolunteerEvent e in events)
            {
                e.Location = mealSiteService.GetMealSiteById(e.LocationId);
                
                if (e.Location == null)
                {
                    System.Diagnostics.Debug.WriteLine($"No location found for ID: {e.LocationId}");
                    e.Location = new MealSite();
                }
            }

            return View(events);
        }
        public ActionResult Create()
        {
            VolunteerEvent model = new VolunteerEvent();
            model.AllLocations = AllLocations();
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
            return View(toBeDeleted);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VolunteerEvent toBeDeleted = service.GetEventById(id);
            service.DeleteEvent(toBeDeleted);
            mealSiteService.DeleteVolunteerEvent(toBeDeleted.Id, toBeDeleted.LocationId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VolunteerEvent volunteerevent)
        {
            if (ModelState.IsValid)
            {
                service.AddEvent(volunteerevent);
                mealSiteService.AddVolunteerEvent(volunteerevent.Id, volunteerevent.LocationId);
                return RedirectToAction("Index");
            }

            return View(volunteerevent);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolunteerEvent volunteerevent = service.GetEventById((int)id);
            if (volunteerevent == null)
            {
                return HttpNotFound();
            }
            volunteerevent.Location = mealSiteService.GetMealSiteById(volunteerevent.LocationId);
            return View(volunteerevent);
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
            volunteerEvent.AllLocations = AllLocations();
            return View(volunteerEvent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StartingShiftTime, EndingShiftTime, OpenSlots, Location, Description")] VolunteerEvent VolunteerEvent)
        {
            if (ModelState.IsValid)
            {
                VolunteerEvent oldEvent = service.GetEventById(VolunteerEvent.Id);
                mealSiteService.DeleteVolunteerEvent(oldEvent.Id, oldEvent.LocationId);
                service.SaveEdits(VolunteerEvent);
                mealSiteService.AddVolunteerEvent(VolunteerEvent.Id, VolunteerEvent.LocationId);

                return RedirectToAction("Index");
            }
            return View(VolunteerEvent);
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
                SelectListItem item = new SelectListItem();
                item.Text = mealSite.AddressLine1;
                item.Value = mealSite.Id.ToString();
                Locations.Add(item);
            }
            return Locations;
        }
    }
}