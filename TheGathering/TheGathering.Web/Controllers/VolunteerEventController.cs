using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TheGathering.Web.Controllers
{
    public class VolunteerEventController : Controller
    {
        // GET: VolunteerEvent
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
    }
}