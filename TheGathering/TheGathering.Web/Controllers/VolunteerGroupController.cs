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
            SignUpGroupViewModel model = new SignUpGroupViewModel();
            var volunteerEventIds = _service.GetVolunteerGroupEvents(volunteerId);
            bool alreadyRegistered = volunteerEventIds.Any(id => id == eventId);
            if (alreadyRegistered)
            {
                return RedirectToAction("EventAlreadyRegistered");
            }
            model.VolunteerGroupLeader = _service.GetLeaderById(volunteerId);
            model.VolunteerEvent = _eventService.GetEventById((int)eventId);
            model.VolunteerGroupLeaderID = model.VolunteerGroupLeader.Id;
            model.VolunteerEventID = model.VolunteerEvent.Id;
            //TO DO: Change Hardcoding (model.VolunteerGroupLeader.Id)
            var events = _eventService.GetEventsByIds(volunteerEventIds);
            return View(model);
        }
        [HttpPost]
        public ActionResult SignUpGroupEvent(SignUpGroupViewModel signUpGroupViewModel)
        {
            int numVolunteers = signUpGroupViewModel.VolunteerSlots;
            int eventId = signUpGroupViewModel.VolunteerEventID;
            int volunteerId = signUpGroupViewModel.VolunteerGroupLeaderID;
            var origOpenSlots = _eventService.GetEventById(eventId).OpenSlots;
            _eventService.GetEventById(eventId).OpenSlots = origOpenSlots - numVolunteers;
            _service.AddVolunteerGroupVolunteerEvent(volunteerId, eventId, numVolunteers);
            return RedirectToAction("GroupEventsList");
        }
        public ActionResult EventAlreadyRegistered()
        {
            return View();
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

        public ActionResult GroupEventsList()
        {
            var volunteergroup = GetCurrentVolunteerGroupLeader();
           
            GroupNumberEventsListViewModel viewModel = new GroupNumberEventsListViewModel();
            viewModel.VolunteerGroupEvents = volunteergroup.VolunteerGroupVolunteerEvents;
           
            viewModel.VolunteerEvents = volunteergroup.VolunteerGroupVolunteerEvents.Select(vgve=>vgve.VolunteerEvent).ToList();
            return View(viewModel);
        }
    }
}