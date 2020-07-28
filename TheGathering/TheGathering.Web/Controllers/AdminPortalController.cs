﻿using System;
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

        public ActionResult ManageCalender()
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
            List<int> IdList = evt.VolunteerVolunteerEvents.Select(vve => vve.VolunteerId).ToList();
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

            VolunteerEvent volEvent = calendarService.GetEventById((int)id);

            List<int> idList = volEvent.VolunteerVolunteerEvents.Select(vve => vve.VolunteerId).ToList();
            List<Volunteer> signedUpVolunteers = volunteerService.GetVolunteersById(idList);
            List<Volunteer> addableVolunteers = volunteerService.GetAllVolunteers();

            foreach (Volunteer v in signedUpVolunteers)
            {
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddVolunteerToEvent(int? eventID, int? volunteerID)
        {
            if (eventID == null || volunteerID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            volunteerService.AddVolunteerVolunteerEvent((int)eventID, (int)volunteerID);

            return RedirectToAction("AddVolunteers", (int)eventID);
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