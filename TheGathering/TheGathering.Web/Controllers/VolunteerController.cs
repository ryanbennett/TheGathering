using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TheGathering.Web.Models;
using TheGathering.Web.Services;
using TheGathering.Web.ViewModels.VolunteerModels;

namespace TheGathering.Web.Controllers
{
    public class VolunteerController : Controller
    {
        // GET: Volunteer
        VolunteerService _service = new VolunteerService();
        CalendarService _eventService = new CalendarService();
        public ActionResult Index()
        {
            var model = _service.GetAllVolunteers();

            if (model == null)
            {
                model = new List<Volunteer>();
            }

            return View(model);
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

       
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Volunteer volunteer = _service.GetById((int)id);
            if (volunteer == null)
            {
                return HttpNotFound();
            }
            return View(volunteer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                _service.Edit(volunteer);
                return RedirectToAction("Index");
            }
            return View(volunteer);
        }

        public ActionResult Details(int id)
        {
            return View(_service.GetById(id));
        }

        public ActionResult SignUpEvent(int eventId, string userId)
        {
            SignUpEventViewModel model = new SignUpEventViewModel();
            model.Volunteer = _service.GetByApplicationUserId(userId);
            //TODO: change Volunteer get
            model.VolunteerEvent = _eventService.GetEventById(eventId);
            var volunteerEventIds = _service.GetVolunteerEventIdsByVolunteerId(4);
            var events = _eventService.GetEventsByIds(volunteerEventIds);
            foreach(int id in volunteerEventIds)
            {
                if(id == eventId)
                {
                    return RedirectToAction("EventAlreadyRegistered");
                }
            }
            return View(model);
        }
        
        public ActionResult EventAlreadyRegistered()
        {
            return View();
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Volunteer volunteer = _service.GetById((int)id);
            if (volunteer == null)
            {
                return HttpNotFound();
            }
            return View(volunteer);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Volunteer volunteer = _service.GetById((int)id);
            _service.DeleteVolunteer(volunteer);
            return RedirectToAction("Index");
        }

        public ActionResult GetEventsByIds(List<int> eventId)
        {
            List<VolunteerEvent> volunteerEvents = _eventService.GetEventsByIds(eventId);
            return View(volunteerEvents);
        }

        public ActionResult UserEventsList(int volunteerId)
        {
           var volunteerEvents = _service.GetVolunteerEventIdsByVolunteerId(volunteerId);
           var events = _eventService.GetEventsByIds(volunteerEvents);
           
           UserEventsListViewModel viewModel = new UserEventsListViewModel();
           viewModel.VolunteerEvents = events;
           return View(viewModel);
        }

        public ActionResult EventRegistered(int volunteerId, int eventId)
        {
            _service.AddVolunteerVolunteerEvent(volunteerId, eventId);
            return View();
        }
    }

}