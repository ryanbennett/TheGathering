using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TheGathering.Web.Models;
using TheGathering.Web.Services;
using TheGathering.Web.ViewModels.VolunteerGroup;

namespace TheGathering.Web.Controllers
{
    public class VolunteerGroupController : BaseController
    {
        // GET: VolunteerGroup
        
        VolunteerGroupService _service = new VolunteerGroupService();
        CalendarService _eventService = new CalendarService();
        SignUpGroupViewModel model = new SignUpGroupViewModel();
        public ActionResult Index()
        {
            var model = _service.GetAllVolunteerGroups();

            if (model == null)
            {
                model = new List<VolunteerGroupLeader>();
            }

            return View(model);
        }
     
        public ActionResult SignUpGroupEvent(int volunteerId, int eventId)
        {
            model.VolunteerGroupLeader = _service.GetLeaderById(volunteerId);
            model.VolunteerEvent = _eventService.GetEventById((int)eventId);
            _service.AddVolunteerGroupVolunteerEvent(volunteerId, eventId);
            var volunteerEventIds = _service.GetVolunteerEventIdsByVolunteerGroupId(volunteerId);
            //TO DO: Change Hardcoding (model.VolunteerGroupLeader.Id)
            var events = _eventService.GetEventsByIds(volunteerEventIds);

            foreach(int id in volunteerEventIds)
            {
                if (id == eventId)
                {
                    return RedirectToAction("EventAlreadyRegistered");
                }
            }
            /*
            bool alreadyRegistered = volunteerEventIds.Any(id => id == eventId);
            if (alreadyRegistered)
            {
                return RedirectToAction("EventAlreadyRegistered");
            }
            */
            return View(model);
        }
        public ActionResult EventAlreadyRegistered()
        {
            return View();
        }
        public ActionResult SignUpGroupNumber()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUpGroupNumber(int volunteerslots)
        {
            model.VolunteerSlots = volunteerslots;
           

            return View(model);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Create(VolunteerGroupLeader volunteergroupleader)
        {
            if (ModelState.IsValid)
            {
                _service.CreateLeader(volunteergroupleader);
                return RedirectToAction("Index");
            }
            return View();

        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolunteerGroupLeader volunteergroupleader = _service.GetLeaderById((int)id);
            if (volunteergroupleader == null)
            {
                return HttpNotFound();
            }
            return View(volunteergroupleader);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VolunteerGroupLeader volunteergroupleader)
        {
            if (ModelState.IsValid)
            {
                _service.EditLeader(volunteergroupleader);
                return RedirectToAction("Index");
            }
            return View(volunteergroupleader);
        }

        public ActionResult Details(int id)
        {
            return View(_service.GetLeaderById(id));
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolunteerGroupLeader volunteergroupleader = _service.GetLeaderById((int)id);
            if (volunteergroupleader == null)
            {
                return HttpNotFound();
            }
            return View(volunteergroupleader);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            VolunteerGroupLeader volunteergroupleader = _service.GetLeaderById(id);
            _service.DeleteLeader(volunteergroupleader);
            return RedirectToAction("Index");
        }

        public ActionResult GetEventsByIds(List<int> eventId)
        {
            List<VolunteerEvent> volunteerEvents = _eventService.GetEventsByIds(eventId);
            return View(volunteerEvents);
        }
    }
}