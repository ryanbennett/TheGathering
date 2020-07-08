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
            VolunteerEvent toBeDeleted = service.GetEventbyID((int)id);
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
            VolunteerEvent toBeDeleted = service.GetEventbyID(id);
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
    }
}