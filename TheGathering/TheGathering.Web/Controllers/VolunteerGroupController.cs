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
        MealSiteService _mealSiteService = new MealSiteService();

        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            var model = _service.GetAllVolunteerGroups();

            if (model == null)
            {
                model = new List<VolunteerGroupLeader>();
            }

            return View(model);
        }
        [Authorize(Roles = "groupleader")]
        public ActionResult SignUpGroupEvent(int eventId)
        {
            SignUpGroupViewModel model = new SignUpGroupViewModel();
            var VolunteerGroupLeader = GetCurrentVolunteerGroupLeader();
            int volunteerId = VolunteerGroupLeader.Id;
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
        [Authorize(Roles = "groupleader")]
        public ActionResult SignUpGroupEvent(SignUpGroupViewModel signUpGroupViewModel)
        {
            int numVolunteers = signUpGroupViewModel.VolunteerSlots;
            int eventId = signUpGroupViewModel.VolunteerEventID;
            signUpGroupViewModel.VolunteerEvent = _eventService.GetEventById(eventId);
            signUpGroupViewModel.VolunteerGroupLeader = GetCurrentVolunteerGroupLeader();
            int volunteerId = signUpGroupViewModel.VolunteerGroupLeader.Id;
            var origOpenSlots = _eventService.GetEventById(eventId).OpenSlots;
            var volunteerEvent = _eventService.GetEventById(eventId);
            if (signUpGroupViewModel.VolunteerSlots<1)
            {
                ModelState.AddModelError("VolunteerSlots", "The number of volunteers cannot be less than 1.");
            }
            if (origOpenSlots < numVolunteers)
            {
                ModelState.AddModelError("VolunteerSlots", "There are only "+origOpenSlots+" volunteer slots available.");
            }

            if (ModelState.IsValid)
            {
                _service.ReduceOpenSlots(volunteerEvent, origOpenSlots, numVolunteers);
                _service.AddVolunteerGroupVolunteerEvent(volunteerId, eventId, numVolunteers);
                return RedirectToAction("GroupEventsList");
            }
            GetCurrentVolunteerGroupLeader().TotalGroupMembers = numVolunteers;
            return View(signUpGroupViewModel);
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

        [Authorize(Roles = "groupleader")]
        public ActionResult Edit(int? id)
        {
            VolunteerGroupLeader vgl ;
            if (id != null && User.IsInRole("admin"))
            {
                vgl = _service.GetLeaderById((int)id);
            }
            else
            {
                vgl = GetCurrentVolunteerGroupLeader();
            }
            if (vgl == null)
            {
                return HttpNotFound();
            }

            return View(vgl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "groupleader")]
        public ActionResult Edit(VolunteerGroupLeader volunteergroupleader)
        {
            if (volunteergroupleader.LeaderEmail.Contains('.') == false)
            {
                ModelState.AddModelError("Email", "Email must contain a period");
            }
            if (volunteergroupleader.LeaderEmail.Contains('@') == false)
            {
                ModelState.AddModelError("Email", "Email must contain an @");
            }
            if (volunteergroupleader.TotalGroupMembers<0)
            {
                ModelState.AddModelError("TotalGroupMembers", "Total Group Members must be greater than 0");
            }
            /***
            if (volunteergroupleader.LeaderPhoneNumber.Length > 11)
            {
                ModelState.AddModelError("LeaderPhoneNumber", "Phone number must be shorter than 11 numbers");
            }
            if (volunteergroupleader.LeaderPhoneNumber.Any(char.IsDigit) == false)
            {
                ModelState.AddModelError("LeaderPhoneNumber", "Phone number must not have non-numeric characters in it.");
            }***/
            if (ModelState.IsValid)
            {
                _service.EditLeader(volunteergroupleader);
                return RedirectToAction("Index");
            }
            return View(volunteergroupleader);
        }
        [Authorize(Roles = "groupleader")]
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

        [Authorize(Roles = "groupleader")]
        public ActionResult GetEventsByIds(List<int> eventId)
        {
            List<VolunteerEvent> volunteerEvents = _eventService.GetEventsByIds(eventId);
            return View(volunteerEvents);
        }
        [Authorize(Roles = "groupleader")]
        public ActionResult GroupEventsList()
        {
            //var volunteergroup = GetCurrentVolunteerGroupLeader();

            //GroupNumberEventsListViewModel viewModel = new GroupNumberEventsListViewModel();
            //viewModel.VolunteerGroupEvents = volunteergroup.VolunteerGroupVolunteerEvents;

            
            //return View(viewModel);

            var volunteerGroupLeader = GetCurrentVolunteerGroupLeader();
            var events = volunteerGroupLeader.VolunteerGroupVolunteerEvents.Select(vgve => vgve.VolunteerEvent).ToList();
           
            //var cancelledEventIds = _service.GetCancelledVolunteerEventIdsByVolunteerId(volunteer.Id);
            //var cancelledEvents = _eventService.GetEventsByIds(cancelledEventIds);

            events.Sort(new SortByDate());
            //cancelledEvents.Sort(new SortByDate());
            List<VolunteerGroupVolunteerEvent> CurrentEvents = new List<VolunteerGroupVolunteerEvent>();
            List<VolunteerGroupVolunteerEvent> PastEvents = new List<VolunteerGroupVolunteerEvent>();
            foreach (VolunteerGroupVolunteerEvent item in volunteerGroupLeader.VolunteerGroupVolunteerEvents)
            {
                if (!item.IsItCanceled)
                {
                    var mealsite = _mealSiteService.GetMealSiteById(item.VolunteerEvent.MealSite_Id);
                    item.VolunteerEvent.MealSite = mealsite;
                    if (item.VolunteerEvent.StartingShiftTime > DateTime.Now)
                        CurrentEvents.Add(item);
                    else
                        PastEvents.Add(item);
                }
            }
            CurrentEvents.Sort(new SortByDateForVolunteerGroup());
            PastEvents.Sort(new SortByDateForVolunteerGroup());
            GroupNumberEventsListViewModel viewModel = new GroupNumberEventsListViewModel();
            viewModel.VolunteerEvents = events;
            //viewModel.CancelledEvents = cancelledEvents;
            viewModel.CurrentEvents = CurrentEvents;
            viewModel.PastEvents = PastEvents;
            viewModel.volunteerGroupLeader = volunteerGroupLeader;

            return View(viewModel);

        }
    }
    public class SortByDateForVolunteerGroup : IComparer<VolunteerGroupVolunteerEvent>
    {
        public int Compare(VolunteerGroupVolunteerEvent x, VolunteerGroupVolunteerEvent y)
        {
            return y.VolunteerEvent.StartingShiftTime.CompareTo(x.VolunteerEvent.StartingShiftTime);
        }
    }
}