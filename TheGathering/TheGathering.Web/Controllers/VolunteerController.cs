using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TheGathering.Web.Models;
using TheGathering.Web.Service;

namespace TheGathering.Web.Controllers
{
    public class VolunteerController : Controller
    {
        // GET: Volunteer
        VolunteerService _service = new VolunteerService();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Create(Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                _service.Create(volunteer);
                return RedirectToAction("Index");
            }
            return View();
            
        }
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Volunteer volunteer = _service.GetVolunteerById((int)id);
            if (volunteer == null)
            {
                return HttpNotFound();
            }
            return View(volunteer);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Volunteer volunteer = _service.GetVolunteerById((int)id);
            _service.DeleteVolunteer(volunteer);
            return RedirectToAction("Index");
        }
    }
}