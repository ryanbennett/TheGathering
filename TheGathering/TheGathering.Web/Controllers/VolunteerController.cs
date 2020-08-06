using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using TheGathering.Web.Models;
using TheGathering.Web.Services;
using TheGathering.Web.ViewModels.VolunteerModels;

namespace TheGathering.Web.Controllers
{

    public class VolunteerController : BaseController
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
            Volunteer volunteer;
            if (id == null)
            {
                volunteer = GetCurrentVolunteer();
            }
            else
            {
                volunteer = _service.GetById((int)id);
            }
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
            DateTime local = volunteer.Birthday.ToUniversalTime();
            DateTime server = DateTime.Now.ToUniversalTime();
            var age = server.Subtract(local);
            if (local.Year < 1900)
            {
                ModelState.AddModelError("Birthday", "Birthday date is out of range");
            }
            if (local >= server)
            {
                ModelState.AddModelError("Birthday", "Birthday date does not exist");
            }
            if (age.TotalDays / 365 < 18)
            {
                ModelState.AddModelError("Birthday", "Volunteer must be older than 18");
            }
            if (volunteer.FirstName.Any(char.IsDigit) == true)
            {
                ModelState.AddModelError("FirstName", "First name cannot contain numbers");
            }
            if (volunteer.LastName.Any(char.IsDigit) == true)
            {
                ModelState.AddModelError("LastName", "Last name cannot contain numbers");
            }
            if (volunteer.PhoneNumber.Length >= 11)
            {
                ModelState.AddModelError("PhoneNumber", "Phone number must be shorter than 11 numbers");
            }

            if (ModelState.IsValid)
            {
                _service.Edit(volunteer);
                return RedirectToAction("VolunteerCalendar", "VolunteerEvent", null);
            }
            return View(volunteer);
        }

        public ActionResult Details(int? id)
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
            VolunteerViewModel viewModel = new VolunteerViewModel(volunteer);

            var volunteerEvents = _service.GetVolunteerEventIdsByVolunteerId((int)id);
            var events = _eventService.GetEventsByIds(volunteerEvents);

            viewModel.VolunteerEvents = events.ToList();
            viewModel.VolunteerEvents.Sort(new SortByDate());
            viewModel.VolunteerEvents = viewModel.VolunteerEvents.Take(3).ToList();

            return View(viewModel);
        }
        public async Task<ActionResult> SignUpEvent(int eventId, string userId)
        {
            SignUpEventViewModel model = new SignUpEventViewModel();
            
            model.Volunteer = GetCurrentVolunteer();
            //TODO: change Volunteer get
            model.VolunteerEvent = _eventService.GetEventById(eventId);
            var volunteerEventIds = _service.GetVolunteerEventIdsByVolunteerId(model.Volunteer.Id);
            var openSlots = model.VolunteerEvent.OpenSlots;
            if (openSlots <= 0)
                return RedirectToAction("EventFull");
            foreach (int id in volunteerEventIds)
            {
                if (id == eventId)
                {
                    return RedirectToAction("EventAlreadyRegistered");
                }
            }
            //Confirmation Email stuff

            string code = await UserManager.GenerateEmailConfirmationTokenAsync(model.Volunteer.ApplicationUserId);
            var callbackUrl = Url.Action("ConfirmEmailEvent", "Volunteer",
               new { userId = userId, code = code, eventId = eventId, volunteerId = model.Volunteer.Id }, protocol: Request.Url.Scheme);

            Console.WriteLine(callbackUrl);

            string subject = "The Gathering Event Confirmation";
            string mealSite = model.VolunteerEvent.MealSite.Name;
            string address = model.VolunteerEvent.MealSite.AddressLine1;
            string city = model.VolunteerEvent.MealSite.City;
            string state = model.VolunteerEvent.MealSite.State;
            string zipcode = model.VolunteerEvent.MealSite.Zipcode;
            string startTime = model.VolunteerEvent.StartingShiftTime.ToString();
            string endTime = model.VolunteerEvent.EndingShiftTime.ToString();
            string description = "Description: " + model.VolunteerEvent.Description;

            string plainText = "Hello " + model.Volunteer.FirstName + ", Thank you for sigining up for this event. Location: " + mealSite + "-- " + address + ", " + city + ", " + state + " " + zipcode + ". Start time: " + startTime + ", End time: " + endTime + ". "+description+". Click the link to confirm that you will be at the event!" + callbackUrl;
            string htmlText = "Hello " + model.Volunteer.FirstName + ", <br/><br/> Thank you for sigining up for this event:<br/>Location: " + mealSite +"-- "+ address+", "+ city+", "+ state+" "+zipcode+"<br/>Start time: "+startTime+"<br/>End time: "+endTime+"<br/>"+description+ "<br/><br/>Click the link below to confirm that you will be at the event. <br/> <a href='" + callbackUrl + "' target='_new'>Click Here</a> <br/> <img src='https://trello-attachments.s3.amazonaws.com/5ec81f7ae324c641265eab5e/5f046a07b1869070763f0493/3127105983ac3dd06e02da13afa54a02/The_Gathering_F2_Full_Color_Black.png' width='600px' style='pointer-events: none; display: block; margin-left: auto; margin-right: auto; width: 50%;'>["+DateTime.Now+"]";

            await ConfirmationEmail(model.Volunteer.FirstName, model.Volunteer.Email, subject, plainText, htmlText);

            return RedirectToAction("EventRegistered", new { volunteerId = model.Volunteer.Id, eventId = eventId});
        }

        [AllowAnonymous]
        public ActionResult ConfirmEmailEvent(string userId, string code, int eventId, int volunteerId)
        {
            VolunteerEvent volunteerEvent = _eventService.GetEventById(eventId);
            var volunteer = volunteerEvent.VolunteerVolunteerEvents.Where(vol => vol.VolunteerEventId == volunteerEvent.Id).ToList();
            if (volunteer.Count > 0)
            {
                volunteer[0].Confirmed = true;
                _eventService.SaveEdits(volunteerEvent);
            }
            return RedirectToAction("VolunteerCalendar", "VolunteerEvent", null);

        }


        public ActionResult CancelSignUpEvent(int eventId, string userId)
        {
            SignUpEventViewModel model = new SignUpEventViewModel();
            model.Volunteer = _service.GetByApplicationUserId(userId);
            //TODO: change Volunteer get
            model.VolunteerEvent = _eventService.GetEventById(eventId);
            var volunteerEventIds = _service.GetVolunteerEventIdsByVolunteerId(model.Volunteer.Id);
            if (!volunteerEventIds.Contains(eventId))
            {
                return RedirectToAction("EventNotRegistered");
            }
            return View(model);
        }

        public ActionResult EventAlreadyRegistered()
        {
            return View();
        }
        public ActionResult EventNotRegistered()
        {
            return View();
        }
        public ActionResult LeadershipInfo()
        {
            return View();
        }

        [Authorize]
        public async Task<ActionResult> LeadershipEmail()
        {
            Volunteer volunteer = GetCurrentVolunteer();
            String plainText = "Hello Natalee, \n " + volunteer.FirstName + " " + volunteer.LastName + " is interested in becoming a leader \n Email: " + volunteer.Email + "\n Phone Number: " + volunteer.PhoneNumber;
            String htmlText = "Hello Natalee, <br /> " + volunteer.FirstName + " " + volunteer.LastName + " is interested in becoming a leader <br /> Email: " + volunteer.Email + "<br /> Phone Number: " + volunteer.PhoneNumber;
            await ConfirmationEmail("Natalee", "21ahmeda@elmbrookstudents.org", "Someone is interested in leadership!", plainText, htmlText);
            return RedirectToAction("VolunteerCalendar", "VolunteerEvent", null);
           
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
            _service.ChangeVolunteerActivation(volunteer, false);
            return RedirectToAction("Index");
        }

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult GetEventsByIds(List<int> eventId)
        {
            List<VolunteerEvent> volunteerEvents = _eventService.GetEventsByIds(eventId);
            return View(volunteerEvents);
        }

        public ActionResult UserEventsList()
        {
            var volunteer = GetCurrentVolunteer();
            var eventIds = _service.GetVolunteerEventIdsByVolunteerId(volunteer.Id);
            var events = _eventService.GetEventsByIds(eventIds);

            var cancelledEventIds = _service.GetCancelledVolunteerEventIdsByVolunteerId(volunteer.Id);
            var cancelledEvents = _eventService.GetEventsByIds(cancelledEventIds);

            events.Sort(new SortByDate());
            cancelledEvents.Sort(new SortByDate());
            List<VolunteerEvent> CurrentEvents = new List<VolunteerEvent>();
            List<VolunteerEvent> PastEvents = new List<VolunteerEvent>();
            foreach (VolunteerEvent item in events)
            {
                if (item.StartingShiftTime > DateTime.Now)
                    CurrentEvents.Add(item);
                else
                    PastEvents.Add(item);
            }

            UserEventsListViewModel viewModel = new UserEventsListViewModel();
            viewModel.VolunteerEvents = events;
            viewModel.CancelledEvents = cancelledEvents;
            viewModel.CurrentEvents = CurrentEvents;
            viewModel.PastEvents = PastEvents;
            viewModel.Volunteer = volunteer;

            return View(viewModel);
        }

        public ActionResult EventRegistered(int volunteerId, int eventId)
        {
            if (_service.GetCancelledVolunteerEventIdsByVolunteerId(volunteerId).Contains(eventId))
            {
                _service.ReSignUpForVolunteerVolunteerEvent(volunteerId, eventId);
            }
            else
            {
                _service.AddVolunteerVolunteerEvent(volunteerId, eventId);
            }

            SignUpEventViewModel model = new SignUpEventViewModel();
            model.Volunteer = GetCurrentVolunteer();
            //TODO: change Volunteer get
            model.VolunteerEvent = _eventService.GetEventById(eventId);
            var openSlots = model.VolunteerEvent.OpenSlots;
            _eventService.ReduceOpenSlots(model.VolunteerEvent, openSlots);
            return View();
        }
        public ActionResult EventUnregistered(int volunteerId, int eventId)
        {
            SignUpEventViewModel model = new SignUpEventViewModel();
            _service.RemoveVolunteerVolunteerEvent(volunteerId, eventId);
            model.VolunteerEvent = _eventService.GetEventById(eventId);
            var openSlots = model.VolunteerEvent.OpenSlots;

            _eventService.IncreaseOpenSlots(model.VolunteerEvent, openSlots);

            return View();
        }
        public ActionResult EventFull()
        {
            return View();
        }
    }

    // IComparer for Sorting Volunteer Events By Date
    public class SortByDate : IComparer<VolunteerEvent>
    {
        public int Compare(VolunteerEvent x, VolunteerEvent y)
        {
            return y.StartingShiftTime.CompareTo(x.StartingShiftTime);
        }
    }
}