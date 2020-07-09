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
        // GET: VolunteerEvent
        public ActionResult Index()
        {
            return View(service.GetAllEvents());
        }
        public ActionResult Create()
        {
            return View();
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
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VolunteerEvent volunteerevent)
        {
            if (ModelState.IsValid)
            {
                service.AddEvent(volunteerevent);
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
            return View(volunteerevent);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolunteerEvent VolunteerEvent = service.GetEventById((int)id);
            if (VolunteerEvent == null)
            {
                return HttpNotFound();
            }
            var viewModel = new VolunteerEventViewModel();
            viewModel.Id = VolunteerEvent.Id;
            viewModel.StartingShiftTime = VolunteerEvent.StartingShiftTime.ToString("yyyy-MM-ddThh:mm");
            viewModel.EndingShiftTime = VolunteerEvent.EndingShiftTime.ToString("yyyy-MM-ddThh:mm");
            viewModel.OpenSlots = VolunteerEvent.OpenSlots;
            viewModel.Location = VolunteerEvent.Location;
            viewModel.Description = VolunteerEvent.Description;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StartingShiftTime, EndingShiftTime, OpenSlots, Location, Description")] VolunteerEvent VolunteerEvent)
        {
            if (ModelState.IsValid)
            {
                service.SaveEdits(VolunteerEvent);
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
    }
}