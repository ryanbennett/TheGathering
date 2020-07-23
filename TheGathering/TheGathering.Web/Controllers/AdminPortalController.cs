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

        public Volunteer GetVolunteerById(int id)
        {
            return volunteerService.GetById(id);
        }

        public ActionResult ViewVolunteers(int eventID)
        {
            var evt = calendarService.GetEventById(eventID);
                var volunteerEventViewModel = new VolunteerEventViewModel(evt);
                List<int> IdList = evt.VolunteerVolunteerEvents.Select(vve => vve.VolunteerId).ToList();
            volunteerEventViewModel.SignedUpVolunteers = volunteerService.GetVolunteersById(IdList);
            return View(volunteerEventViewModel);
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
            return View(mealSite);
        }

        public ActionResult CreateVolunteer()
        {
            return View();
        }

        [HttpPost]

        public ActionResult CreateVolunteer(Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                volunteerService.Create(volunteer);
                return RedirectToAction("Index");
            }
            return View();

        }

        public ActionResult EditVolunteer(int? id)
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
        public ActionResult EditVolunteer(Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                volunteerService.Edit(volunteer);
                return RedirectToAction("Index");
            }
            return View(volunteer);
        }

        public ActionResult DeleteVolunteer(int? id)
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
        public ActionResult DeleteVolunteer(int id)
        {
            Volunteer volunteer = volunteerService.GetById((int)id);
            volunteerService.DeleteVolunteer(volunteer);
            return RedirectToAction("Index");
        }

        public ActionResult ManageCalender()
        {
            return View(calendarService.GetAllEvents());
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

            volunteerEvent.MealSite = mealService.GetMealSiteById(volunteerEvent.MealSite_Id);
            SignUpEventViewModel signUpEventViewModel = new SignUpEventViewModel
            {
                Volunteer = GetCurrentVolunteer(),
                VolunteerEvent = volunteerEvent
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