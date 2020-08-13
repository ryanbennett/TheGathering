using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using TheGathering.Web.Models;
using TheGathering.Web.Services;
using TheGathering.Web.ViewModels;
using TheGathering.Web.ViewModels.MealSite;
using TheGathering.Web.ViewModels.VolunteerModels;
using TheGathering.Web.ViewModels.VolunteerGroup;
using TheGathering.Web.ViewModels.Account;
using Microsoft.AspNet.Identity;
using System.Globalization;
using System.Text;
using TheGathering.Web.Sorting.VolunteerEventSorts;

namespace TheGathering.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminPortalController : BaseController
    {

        // Services

        private VolunteerService volunteerService = new VolunteerService();
        private MealSiteService mealService = new MealSiteService();
        private CalendarService calendarService = new CalendarService();
        private VolunteerGroupService volunteerGroupService = new VolunteerGroupService();

        private Random rnd = new Random();

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

        public ActionResult ManageGroupLeaders()
        {
            return View(volunteerGroupService.GetAllVolunteerGroups());
        }

    public ActionResult MealSites()
        {

            return View(mealService.GetAllActiveAndInActiveMealSites());
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
            MealSiteViewModel site = new MealSiteViewModel(meal);
            return View(site);
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
        public ActionResult ChangeLeaderActivationConfirmation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            VolunteerGroupLeader lead = volunteerGroupService.GetLeaderById((int)id);

            if (lead == null)
            {
                return HttpNotFound();
            }

            return View(lead);
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
        public ActionResult LeaderActivationChange(int? id, bool? active)
        {
            if (active == null || id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            VolunteerGroupLeader lead = volunteerGroupService.GetLeaderById((int)id);

            if (lead == null)
            {
                return HttpNotFound();
            }

            volunteerGroupService.ChangeGroupLeaderActivation(lead, (bool)active);

            return RedirectToAction("GroupLeaderDetails", new { id = (int)id });
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
            string message = $"A request to reset your account's password has been made. Please reset your password by clicking <a href=\"{ callbackUrl }\">here.</a>";                                                                                                                                                                                                                                                                                           /* Why do I hear boss music? */

            await SendGatheringEmail(volunteer.FirstName, user.Email, "The Gathering Account Password Reset", message, message);
            return RedirectToAction("ForgotPasswordConfirmation", "Account");
        }

        [AllowAnonymous]
        public ActionResult VolunteerAccountRegister()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VolunteerAccountRegister(AdminAccountRegisterViewModel model)
        {
            DateTime local = model.Birthday.ToUniversalTime();
            DateTime server = DateTime.Now.ToUniversalTime();
            TimeSpan age = server.Subtract(local);

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

            if (model.FirstName.Any(char.IsDigit) == true)
            {
                ModelState.AddModelError("FirstName", "First name cannot contain numbers");
            }

            if (model.LastName.Any(char.IsDigit) == true)
            {
                ModelState.AddModelError("LastName", "Last name cannot contain numbers");
            }

            if (model.Email.Contains('.') == false)
            {
                ModelState.AddModelError("Email", "Email must contain a period");
            }

            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user, MakeRandomPassword());

                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "volunteer");

                    VolunteerService VolunteerService = new VolunteerService();

                    Volunteer volunteer = new Volunteer
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Birthday = model.Birthday,
                        PhoneNumber = model.PhoneNumber,
                        InterestInLeadership = false,
                        SignUpForNewsLetter = model.SignUpForNewsLetter,
                        ApplicationUserId = user.Id,
                        Email = model.Email
                    };

                    VolunteerService.Create(volunteer);

                    string subject = "The Gathering Registration Confirmation";
                    string plainText = "Hello " + model.FirstName + ", Thank you for creating an account with The Gathering! Our volunteers are the backbone of our organization. We are dedicated to Feeding the Hungry & Keeping Hearts Full and we look forward to seeing you soon.";
                    string htmlText = "Hello " + model.FirstName + ", Thank you for creating an account with The Gathering! Our volunteers are the backbone of our organization. We are dedicated to Feeding the Hungry & Keeping Hearts Full and we look forward to seeing you soon.<br/> <a href='" + "' target='_new'>Click here to confirm your account</a> <br/> <img src='https://trello-attachments.s3.amazonaws.com/5ec81f7ae324c641265eab5e/5f046a07b1869070763f0493/3127105983ac3dd06e02da13afa54a02/The_Gathering_F2_Full_Color_Black.png' width='600px' style='pointer-events: none; display: block; margin-left: auto; margin-right: auto; width: 50%;'>";

                    await ConfirmationEmail(model.FirstName, model.Email, subject, plainText, htmlText);

                    // Secret Sauce ;D
                    string userCode = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    string callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = userCode }, protocol: Request.Url.Scheme);

                    //await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    string message = $"A request to reset your account's password has been made. Please reset your password by clicking <a href=\"{ callbackUrl }\">here.</a>";                                                                                                                                                                                                                                                                                           /* Why do I hear boss music? */

                    await SendGatheringEmail(volunteer.FirstName, user.Email, "The Gathering Account Password Reset", message, message);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Users");
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public string MakeRandomPassword()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append(Guid.NewGuid().ToString());

            int indCheck = rnd.Next(0, strBuilder.Length);

            while (!char.IsLetter(strBuilder[indCheck]))
            {
                indCheck = rnd.Next(0, strBuilder.Length);
            }

            strBuilder[indCheck] = char.ToUpper(strBuilder[indCheck]);

            return strBuilder.ToString();
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

                List<string> emails = new List<string>(volunteerEvent.VolunteerVolunteerEvents.Count + volunteerEvent.VolunteerGroupVolunteerEvents.Count);

                foreach (VolunteerVolunteerEvent item in volunteerEvent.VolunteerVolunteerEvents)
                {
                    if (item.IsItCanceled) { continue; }

                    Volunteer vol = volunteerService.GetById(item.VolunteerId);

                    emails.Add(vol.Email);
                }

                foreach (VolunteerGroupVolunteerEvent group in volunteerEvent.VolunteerGroupVolunteerEvents)
                {
                    if (group.IsItCanceled) { continue; }

                    VolunteerGroupLeader leader = volunteerGroupService.GetLeaderById(group.VolunteerGroupId);

                    emails.Add(leader.LeaderEmail);
                }

                await SendGatheringEmail(emails, model.Subject, model.Message, model.Message);
                return RedirectToAction("EventDetails", new { id = model.EventId });
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
            if (volunteer.Email.Contains('.') == false)
            {
                ModelState.AddModelError("Email", "Email must contain a period");
            }
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


        //if you edit this please go to [VolunteerController-->Edit] and implement the fixes in there too
        public ActionResult VolunteerReport(int? id)
        {
            var volunteer = GetVolunteerById((int)id);
            var eventIds = volunteerService.GetVolunteerEventIdsByVolunteerId(volunteer.Id);
            var events = calendarService.GetEventsByIds(eventIds);

            var cancelledEventIds = volunteerService.GetCancelledVolunteerEventIdsByVolunteerId(volunteer.Id);
            var cancelledEvents = calendarService.GetEventsByIds(cancelledEventIds);


            events.Sort(new SortByDate());
            cancelledEvents.Sort(new SortByDate());
            var maybeFirstEvent = new VolunteerEvent();
            var maybeLastEvent = new VolunteerEvent();
            if (events.Count() > 0) {
                maybeFirstEvent = events[events.Count() - 1];
                maybeLastEvent = events[0];
            }
            else
            {
                               //Nothing happens
            }

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

            

            if (events.Count() > 0)
            {
                ViewBag.timeWithGathering = timeInBetween.Days;
            }
            else
            {
                ViewBag.timeWithGathering = 0;
            }

            if (ViewBag.timeWithGathering < 0)
            {
                ViewBag.timeWithGathering = 0;
            }


            ViewBag.monthlyFrequency = frequency;

            return View(viewModel);
        }

        // Volunteer Events


        public ActionResult ManageEvents(EventSortType? requestedSort)
        {
            EventSortType type = requestedSort ?? EventSortType.NewestAdded; //Set it to the current requested sort or default to none if there is none

            SortedEventsViewModel view = new SortedEventsViewModel
            {
                Events = calendarService.GetSortedEvents(type),
                SortItems = VolunteerEventSortsManager.GetSortSelectItems(type)
            };

            return View(view);
        }

        public ActionResult ManageCalendar()
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
            DateTime local = volunteergroupleader.LeaderBirthday.ToUniversalTime();
            DateTime server = DateTime.Now.ToUniversalTime();
            var age = server.Subtract(local);
            if (local.Year < 1900)
            {
                ModelState.AddModelError("LeaderBirthday", "Birthday date is out of range");
            }
            if (local >= server)
            {
                ModelState.AddModelError("LeaderBirthday", "Birthday date does not exist");
            }
            if (age.TotalDays / 365 < 18)
            {
                ModelState.AddModelError("LeaderBirthday", "Volunteer must be older than 18");
            }
            if (volunteergroupleader.LeaderFirstName.Any(char.IsDigit) == true)
            {
                ModelState.AddModelError("LeaderFirstName", "First name cannot contain numbers");
            }
            if (volunteergroupleader.LeaderLastName.Any(char.IsDigit) == true)
            {
                ModelState.AddModelError("LeaderLastName", "Last name cannot contain numbers");
            }
            if (volunteergroupleader.LeaderEmail.Contains('.') == false)
            {
                ModelState.AddModelError("LeaderEmail", "Email must contain a period");
            }
            if (volunteergroupleader.TotalGroupMembers <= 0)
            {
                ModelState.AddModelError("TotalGroupMembers", "Total group members must be greater than 0");
            }
            if (ModelState.IsValid)
            {
                volunteerGroupService.EditLeader(volunteergroupleader);
                return RedirectToAction("ManageGroupLeaders");
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

        public async Task<ActionResult> GroupLeaderResetUserPass(int? groupLeaderID)
        {
            if (groupLeaderID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            VolunteerGroupLeader volunteer = volunteerGroupService.GetLeaderById((int)groupLeaderID);

            ApplicationUser user = await UserManager.FindByNameAsync(volunteer.LeaderEmail);

            // Secret Sauce ;D
            string userCode = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            string callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = userCode }, protocol: Request.Url.Scheme);

            //await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
            string message = $"An admin has created an account using this email address. If this is an error, <a href=\"thegatheringwis.org\"> please visit our website and contact us.</a> Please set your password by clicking <a href=\"" + callbackUrl + "\">here</a>";                                                                                                                                                                                                                                                                       /* Why do I hear boss music? */

            await SendGatheringEmail(volunteer.LeaderFirstName, user.Email, "The Gathering Account Password Reset", message, message);
            return RedirectToAction("ForgotPasswordConfirmation", "Account");
        }

    


        public ActionResult GroupLeaderCreate()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GroupLeaderCreate(AdminGroupRegistrationViewModel model)
        {

            DateTime local = model.LeaderBirthday.ToUniversalTime();
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
            if (model.LeaderFirstName.Any(char.IsDigit) == true)
            {
                ModelState.AddModelError("FirstName", "First name cannot contain numbers");
            }
            if (model.LeaderLastName.Any(char.IsDigit) == true)
            {
                ModelState.AddModelError("LastName", "Last name cannot contain numbers");
            }
            if (model.Email.Contains('.') == false)
            {
                ModelState.AddModelError("Email", "Email must contain a period");
            }
            if (model.TotalGroupMembers <= 0)
            {
                ModelState.AddModelError("TotalGroupMembers", "Total group members must be greater than 0");
            }
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, MakeRandomPassword());
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "groupleader");

                    var VolunteerGroupService = new VolunteerGroupService();

                    VolunteerGroupLeader volunteerLeader = new VolunteerGroupLeader();
                    volunteerLeader.LeaderFirstName = model.LeaderFirstName;
                    volunteerLeader.LeaderLastName = model.LeaderLastName;
                    volunteerLeader.LeaderBirthday = model.LeaderBirthday;
                    volunteerLeader.LeaderPhoneNumber = model.LeaderPhoneNumber;
                    volunteerLeader.SignUpForNewsLetter = model.SignUpForNewsLetter;
                    volunteerLeader.ApplicationUserId = user.Id;
                    volunteerLeader.LeaderEmail = model.Email;
                    volunteerLeader.GroupName = model.GroupName;
                    volunteerLeader.TotalGroupMembers = model.TotalGroupMembers;

                    VolunteerGroupService.CreateLeader(volunteerLeader);

                    ApplicationUser user2 = await UserManager.FindByNameAsync(volunteerLeader.LeaderEmail);

                    // Secret Sauce ;D
                    string userCode = await UserManager.GeneratePasswordResetTokenAsync(user2.Id);
                    string callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user2.Id, code = userCode }, protocol: Request.Url.Scheme);

                    //await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    String subject = "The Gathering Registration Confirmation";
                    String plainText = "Hello " + model.LeaderFirstName + ", Thank you for registering with The Gathering! Our volunteers are a vital part of our " +
                        "organization. We look forward to seeing you soon. An admin has created an account using this email address. If this is an error, please visit our website at thegatheringwis.org.";
                    String htmlText = "<strong>Hello " + model.LeaderFirstName + ",</strong><br/> Thank you for registering with The Gathering! Our volunteers are a vital part of our" +
                        "organization. We look forward to seeing you soon. An admin has created an account using this email address. If this is an error, <a href=\"thegatheringwis.org\"> please visit our website and contact us.</a> Please set your password by clicking <a href=\"" + callbackUrl + "\">here. </a> <img src='https://trello-attachments.s3.amazonaws.com/5ec81f7ae324c641265eab5e/5f046a07b1869070763f0493/3127105983ac3dd06e02da13afa54a02/The_Gathering_F2_Full_Color_Black.png' width='600px' style='pointer-events: none; display: block; margin-left: auto; margin-right: auto; width: 50%;'>";

                    await ConfirmationEmail(model.LeaderFirstName, model.Email, subject, plainText, htmlText);


                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");



                    return RedirectToAction("ManageGroupLeaders");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}