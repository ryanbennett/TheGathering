using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TheGathering.Web.Models;
using TheGathering.Web.Services;
using TheGathering.Web.ViewModels;
using TheGathering.Web.ViewModels.MealSite;
using TheGathering.Web.ViewModels.VolunteerModels;

namespace TheGathering.Web.Controllers
{
    public class AdminPortalController : BaseController
    {
        private VolunteerService volunteerService = new VolunteerService();
        private MealSiteService mealService = new MealSiteService();
        private CalendarService calendarService = new CalendarService();

        // GET: AdminPortal
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Users()
        {
            var volunteers = volunteerService.GetAllVolunteers();

            if (volunteers == null)
            {
                volunteers = new List<Volunteer>();
            }

            return View(volunteers);
        }

        public ActionResult MealSites()
        {

            return View(mealService.GetAllMealSites());
        }

        public ActionResult ManageCalendar()
        {
            return View(calendarService.GetAllEvents());
        }

        public Volunteer GetVolunteerById(int id)
        {
            return volunteerService.GetById(id);
        }

        public ActionResult ViewVolunteers(int eventID)
        {
            var evt = calendarService.GetEventById(eventID);
            var volunteerEventViewModel = new VolunteerEventViewModel(evt);
            List<int> IdList = evt.VolunteerVolunteerEvents.Where(vve => !vve.IsItCanceled).Select(vve => vve.VolunteerId).ToList();
            volunteerEventViewModel.SignedUpVolunteers = volunteerService.GetVolunteersById(IdList);
            return View(volunteerEventViewModel);
        }

        public ActionResult AddVolunteers(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            VolunteerEvent volunteerEvent = calendarService.GetEventById((int)id);

            if (volunteerEvent == null)
            {
                return HttpNotFound();
            }

            List<int> idList = volunteerEvent.VolunteerVolunteerEvents.Select(vve => vve.VolunteerId).ToList();
            List<Volunteer> signedUpVolunteers = volunteerService.GetVolunteersById(idList);
            List<Volunteer> addableVolunteers = volunteerService.GetAllVolunteers();

            foreach (Volunteer v in signedUpVolunteers)
            {
                var volVolEvent = volunteerEvent.VolunteerVolunteerEvents.Where(ctx => ctx.VolunteerId == v.Id).ToList();

                //If there's more than one item then they're most likely duplicates, there should only be one in this list
                if (volVolEvent.Count > 0 && volVolEvent[0].IsItCanceled) { continue; }

                addableVolunteers.Remove(v);
            }

            volunteerEvent.MealSite = mealService.GetMealSiteById(volunteerEvent.MealSite_Id);
            AddVolunteerViewModel viewModel = new AddVolunteerViewModel
            {
                Event = volunteerEvent,
                AvailableVolunteers = addableVolunteers
            };

            return View(viewModel);
        }

        public ActionResult AddVolunteerToEvent(int? eventID, int? volunteerID)
        {
            if (eventID == null || volunteerID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var volEvent = calendarService.GetEventById((int)eventID);
            var volunteerEvents = volEvent.VolunteerVolunteerEvents.Where(ctx => ctx.VolunteerId == (int)volunteerID).ToList();

            //If we find VolunteerVolunteerEvents in the current volunteer event then we already added this volunteer to the event
            if (volunteerEvents == null || volunteerEvents.Count < 1)
            {
                volunteerService.AddVolunteerVolunteerEvent((int)volunteerID, (int)eventID);
            }

            //Change it so that the volunteer is no longer canceled, avoids duplicates
            else
            {
                volunteerEvents[0].IsItCanceled = false;
                calendarService.SaveEdits(volEvent);
            }

            return RedirectToAction("AddVolunteers", new { id = (int)eventID });
        }

        public ActionResult RemoveVolunteerFromEvent(int? eventID, int? volunteerID)
        {
            if (eventID == null || volunteerID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            volunteerService.RemoveVolunteerVolunteerEvent((int)volunteerID, (int)eventID);

            calendarService.VolunteerCanceled((int)eventID);
            return RedirectToAction("ViewVolunteers", new { eventID = (int)eventID });
        }

        public ActionResult MealSiteDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MealSite mealSite = mealService.GetMealSiteById((int)id);
            if (mealSite == null)
            {
                return HttpNotFound();
            }
            MealSiteViewModel viewModel = new MealSiteViewModel(mealSite);
            return View(viewModel);
        }

        public ActionResult EventDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            VolunteerEvent volunteerEvent = calendarService.GetEventById((int)id);

            if (volunteerEvent == null)
            {
                return HttpNotFound();
            }

            List<int> IdList = volunteerEvent.VolunteerVolunteerEvents.Where(vve => !vve.IsItCanceled).Select(vve => vve.VolunteerId).ToList();

            volunteerEvent.MealSite = mealService.GetMealSiteById(volunteerEvent.MealSite_Id);
            SignUpEventViewModel signUpEventViewModel = new SignUpEventViewModel
            {
                Volunteer = GetCurrentVolunteer(),
                VolunteerEvent = volunteerEvent,
                Volunteers = volunteerService.GetVolunteersById(IdList)
            };

            return View(signUpEventViewModel);
        }

        public ActionResult VolunteerCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VolunteerCreate(Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                volunteerService.Create(volunteer);
                return RedirectToAction("Users");
            }

            return View();
        }


        public ActionResult VolunteerEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Volunteer volunteer = volunteerService.GetById((int)id);

            if (volunteer == null)
            {
                return HttpNotFound();
            }

            return View(volunteer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VolunteerEdit(Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                volunteerService.Edit(volunteer);
                return RedirectToAction("Users");
            }
            return View(volunteer);
        }

        public ActionResult VolunteerDetails(int id)
        {
            return View(volunteerService.GetById(id));
        }

        public ActionResult VolunteerDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Volunteer volunteer = volunteerService.GetById((int)id);

            if (volunteer == null)
            {
                return HttpNotFound();
            }

            return View(volunteer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VolunteerDelete(int id)
        {
            Volunteer volunteer = volunteerService.GetById((int)id);
            volunteerService.DeleteVolunteer(volunteer);
            return RedirectToAction("Users");
        }

        public ActionResult ManageEvents()
        {
            return View(calendarService.GetAllEvents());
        }

        public ActionResult SignUpEvent(int eventId, string userId)
        {
            SignUpEventViewModel model = new SignUpEventViewModel();
            model.Volunteer = volunteerService.GetByApplicationUserId(userId);
            //TODO: change Volunteer get
            model.VolunteerEvent = calendarService.GetEventById(eventId);
            var volunteerEventIds = volunteerService.GetVolunteerEventIdsByVolunteerId(model.Volunteer.Id);
            var events = calendarService.GetEventsByIds(volunteerEventIds);
            foreach (int id in volunteerEventIds)
            {
                if (id == eventId)
                {
                    return RedirectToAction("EventAlreadyRegistered");
                }
            }
            return View(model);
        }

        public ActionResult CancelSignUpEvent(int eventId, string userId)
        {
            SignUpEventViewModel model = new SignUpEventViewModel();
            model.Volunteer = volunteerService.GetByApplicationUserId(userId);
            //TODO: change Volunteer get
            model.VolunteerEvent = calendarService.GetEventById(eventId);
            var volunteerEventIds = volunteerService.GetVolunteerEventIdsByVolunteerId(model.Volunteer.Id);
            if (!volunteerEventIds.Contains(eventId))
            {
                return RedirectToAction("EventNotRegistered");
            }
            return View(model);
        }
    }
}