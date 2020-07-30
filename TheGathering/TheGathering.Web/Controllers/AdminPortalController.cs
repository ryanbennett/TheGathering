
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TheGathering.Web.Models;
using TheGathering.Web.Services;
using TheGathering.Web.ViewModels;
using TheGathering.Web.ViewModels.MealSite;
using TheGathering.Web.ViewModels.VolunteerModels;
using TheGathering.Web.ViewModels.VolunteerGroup;

namespace TheGathering.Web.Controllers
{
    public class AdminPortalController : BaseController
    {

        // Services

        private VolunteerService volunteerService = new VolunteerService();
        private MealSiteService mealService = new MealSiteService();
        private CalendarService calendarService = new CalendarService();
        private VolunteerGroupService volunteerGroupService = new VolunteerGroupService();


        // Basic pages


        // GET: AdminPortal
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Users()
        {
            var volunteers = volunteerService.GetAllActiveAndInactiveVolunteers();

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

        public ActionResult ChangeMealSiteActivationConfirmation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MealSite meal = mealService.GetMealSiteById((int)id);

            if (meal == null)
            {
                return HttpNotFound();
            }

            return View(meal);
        }

        public ActionResult MealSiteActivationChange(int? id, bool? active)
        {
            if (active == null || id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MealSite meal = mealService.GetMealSiteById((int)id);

            if (meal == null)
            {
                return HttpNotFound();
            }

            mealService.ChangeMealSiteActivation(meal, (bool)active);

            return RedirectToAction("MealSiteDetails", new { id = (int)id });
        }

        // Volunteer



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

        public ActionResult ChangeAccountActivationConfirmation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Volunteer vol = volunteerService.GetById((int)id);

            if (vol == null)
            {
                return HttpNotFound();
            }

            return View(vol);
        }

        public ActionResult AccountActivationChange(int? id, bool? active)
        {
            if (active == null || id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Volunteer vol = volunteerService.GetById((int)id);

            if (vol == null)
            {
                return HttpNotFound();
            }

            volunteerService.ChangeVolunteerActivation(vol, (bool)active);

            return RedirectToAction("VolunteerDetails", new { id = (int)id });
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

        public async Task<ActionResult> ResetUserPass(int? volunteerID)
        {
            if (volunteerID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Volunteer volunteer = volunteerService.GetById((int)volunteerID);

            ApplicationUser user = await UserManager.FindByNameAsync(volunteer.Email);

            // Secret Sauce ;D
            string userCode = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            string callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = userCode }, protocol: Request.Url.Scheme);

            //await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
            string message = $"A request to reset your account's password has been made Please reset your password by clicking <a href=\"{ callbackUrl }\">here</a>";                                                                                                                                                                                                                                                                                           /* Why do I hear boss music? */

            await SendGatheringEmail(volunteer.FirstName, user.Email, "The Gathering Account Password Reset", message, message);
            return RedirectToAction("ForgotPasswordConfirmation", "Account");
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

        public ActionResult EmailEventVolunteers(int? eventID)
        {
            if (eventID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            VolunteerEvent volEvent = calendarService.GetEventById((int)eventID);

            if (volEvent == null)
            {
                return HttpNotFound();
            }

            VolunteerEmailViewModel emailModel = new VolunteerEmailViewModel
            {
                EventId = volEvent.Id,
                VolunteerEvent = volEvent,
                Message = string.Empty
            };

            return View(emailModel);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EmailEventVolunteers([Bind(Include = "EventId, Subject, Message")] VolunteerEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                VolunteerEvent volunteerEvent = calendarService.GetEventById(model.EventId);

                List<string> emails = new List<string>(volunteerEvent.VolunteerVolunteerEvents.Count);

                foreach (VolunteerVolunteerEvent item in volunteerEvent.VolunteerVolunteerEvents)
                {
                    if (item.IsItCanceled) { continue; }

                    Volunteer vol = volunteerService.GetById(item.VolunteerId);

                    emails.Add(vol.Email);
                }

                await SendGatheringEmail(emails, model.Subject, model.Message, model.Message);
                return RedirectToAction("EventDetails", new { id = model.EventId });
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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



        public ActionResult VolunteerReport(int? id)
        {
            var volunteer = GetCurrentVolunteer();
            var eventIds = volunteerService.GetVolunteerEventIdsByVolunteerId(volunteer.Id);
            var events = calendarService.GetEventsByIds(eventIds);

            var cancelledEventIds = volunteerService.GetCancelledVolunteerEventIdsByVolunteerId(volunteer.Id);
            var cancelledEvents = calendarService.GetEventsByIds(cancelledEventIds);


            events.Sort(new SortByDate());
            cancelledEvents.Sort(new SortByDate());

            var maybeLastEvent = events[0];
            var maybeFirstEvent = events[events.Count() - 1];

            DateTime now = DateTime.UtcNow;
            now = now.AddHours(-5);
            TimeSpan timeInBetween = now - maybeFirstEvent.StartingShiftTime;
            int months = timeInBetween.Days / 31;
            double frequency = 0.0;
            TimeSpan volunteerHours = new TimeSpan(0, 0, 0, 0);

            foreach (var finishedEvent in events)
            {
                if (finishedEvent.EndingShiftTime <= now)
                {
                    volunteerHours += (finishedEvent.EndingShiftTime - finishedEvent.StartingShiftTime);
                }
            }

            ViewBag.totalHours = volunteerHours.Hours;
            ViewBag.totalMinutes = volunteerHours.Minutes;

            if (months > 0)
            {
                frequency = events.Count() / months;
            }

            VolunteerReportViewModel viewModel = new VolunteerReportViewModel();
            viewModel.VolunteerEvents = events;
            viewModel.CancelledEvents = cancelledEvents;
            viewModel.Volunteer = volunteerService.GetById((int)id);

            ViewBag.AmountOfSignedUpEvents = events.Count();
            ViewBag.AmountOfCancelledEvents = cancelledEvents.Count();


            ViewBag.timeWithGathering = timeInBetween.Days;
            ViewBag.monthlyFrequency = frequency;

            return View(viewModel);
        }

        // Volunteer Events


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



        // Volunteer Groups

        public ActionResult GroupLeaderEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolunteerGroupLeader volunteergroupleader = volunteerGroupService.GetLeaderById((int)id);
            if (volunteergroupleader == null)
            {
                return HttpNotFound();
            }
            return View(volunteergroupleader);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GroupLeaderEdit(VolunteerGroupLeader volunteergroupleader)
        {
            if (ModelState.IsValid)
            {
                volunteerGroupService.EditLeader(volunteergroupleader);
                return RedirectToAction("Index");
            }
            return View(volunteergroupleader);
        }

        public ActionResult GroupLeaderDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolunteerGroupLeader volunteergroupleader = volunteerGroupService.GetLeaderById((int)id);
            if (volunteergroupleader == null)
            {
                return HttpNotFound();
            }
            return View(volunteergroupleader);
        }

        [HttpPost]
        public ActionResult GroupLeaderDelete(int id)
        {
            VolunteerGroupLeader volunteergroupleader = volunteerGroupService.GetLeaderById(id);
            volunteerGroupService.DeleteLeader(volunteergroupleader);
            return RedirectToAction("Index");
        }

        public ActionResult GroupLeaderDetails(int id)
        {
            return View(volunteerGroupService.GetLeaderById(id));
        }

        public ActionResult SignUpGroupEvent(int volunteerId, int eventId)
        {
            SignUpGroupViewModel model = new SignUpGroupViewModel();
            var volunteerEventIds = volunteerGroupService.GetVolunteerGroupEvents(volunteerId);
            bool alreadyRegistered = volunteerEventIds.Any(id => id == eventId);
            if (alreadyRegistered)
            {
                return RedirectToAction("EventAlreadyRegistered");
            }
            model.VolunteerGroupLeader = volunteerGroupService.GetLeaderById(volunteerId);
            model.VolunteerEvent = calendarService.GetEventById((int)eventId);
            model.VolunteerGroupLeaderID = model.VolunteerGroupLeader.Id;
            model.VolunteerEventID = model.VolunteerEvent.Id;
            //TO DO: Change Hardcoding (model.VolunteerGroupLeader.Id)
            var events = calendarService.GetEventsByIds(volunteerEventIds);
            return View(model);
        }
        [HttpPost]
        public ActionResult SignUpGroupEvent(SignUpGroupViewModel signUpGroupViewModel)
        {
            int numVolunteers = signUpGroupViewModel.VolunteerSlots;
            int eventId = signUpGroupViewModel.VolunteerEventID;
            int volunteerId = signUpGroupViewModel.VolunteerGroupLeaderID;
            var origOpenSlots = calendarService.GetEventById(eventId).OpenSlots;
            var volunteerEvent = calendarService.GetEventById(eventId);
            volunteerGroupService.ReduceOpenSlots(volunteerEvent, origOpenSlots, numVolunteers);
            volunteerGroupService.AddVolunteerGroupVolunteerEvent(volunteerId, eventId, numVolunteers);
            return RedirectToAction("GroupEventsList");
        }

        public ActionResult GroupLeaderEventAlreadyRegistered()
        {
            return View();
        }
        public ActionResult GroupLeaderEventList()
        {
            var volunteergroup = GetCurrentVolunteerGroupLeader();

            GroupNumberEventsListViewModel viewModel = new GroupNumberEventsListViewModel();
            viewModel.VolunteerGroupEvents = volunteergroup.VolunteerGroupVolunteerEvents;

            viewModel.VolunteerEvents = volunteergroup.VolunteerGroupVolunteerEvents.Select(vgve => vgve.VolunteerEvent).ToList();
            return View(viewModel);
        }
    }
}