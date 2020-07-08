using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data;
using TheGathering.Web.Services;
using TheGathering.Web.Models;

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