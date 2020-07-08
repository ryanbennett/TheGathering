using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
    }
}