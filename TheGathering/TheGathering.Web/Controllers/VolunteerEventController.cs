using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data;
using TheGathering.Web.Models;
using TheGathering.Web.Services;
using TheGathering.Web.ViewModels.VolunteerModels;

namespace TheGathering.Web.Controllers
{
    public class VolunteerEventController : BaseController
    {
        private CalendarService service = new CalendarService();
        private MealSiteService mealSiteService = new MealSiteService();

        public const string INVALID_CALENDAR_DATES_ERROR = "The given Calendar dates are incorrect, make sure the start date is earlier than the end date.";

        private VolunteerService volunteerService = new VolunteerService();
        private VolunteerGroupService volunteerGroupService = new VolunteerGroupService();
        // GET: VolunteerEvent
        [Authorize(Roles = "volunteer,groupleader")]
        public ActionResult Index()
        {
            List<VolunteerEvent> events = service.GetAllEvents();

            return View(events);
        }
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            VolunteerEventViewModel model = new VolunteerEventViewModel();
            model.DropDownItems = AllLocations();
            return View(model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolunteerEvent toBeDeleted = service.GetEventById((int)id);
            if (toBeDeleted == null)
            {
                return HttpNotFound();
            }

            toBeDeleted.MealSite = mealSiteService.GetMealSiteById(toBeDeleted.MealSite_Id);

            return View(toBeDeleted);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VolunteerEvent toBeDeleted = service.GetEventById(id);
            service.DeleteEvent(toBeDeleted);
            //mealSiteService.DeleteVolunteerEvent(toBeDeleted.Id, toBeDeleted.MealSite.Id);
            return RedirectToAction("ManageEvents", "AdminPortal", null);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VolunteerEventViewModel viewModel)
        {
            viewModel.DropDownItems = AllLocations();
            if (viewModel.OpenSlots < 1)
            {
                ModelState.AddModelError("OpenSlots", "The number of open slots must be greater than 0");
            }
            if (ModelState.IsValid)
            {
                if (viewModel.StartingShiftTime.CompareTo(viewModel.EndingShiftTime) < 0)
                {
                    VolunteerEvent volunteerEvent = new VolunteerEvent(viewModel);
                    volunteerEvent.MealSite_Id = viewModel.MealSiteId;
                    service.AddEvent(volunteerEvent);

                    return RedirectToAction("ManageEvents", "AdminPortal", null);
                }

                viewModel.Error = INVALID_CALENDAR_DATES_ERROR;
                return View(viewModel);
            }

            return View(viewModel);
        }

        [Authorize(Roles = "groupleader")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolunteerEvent volunteerEvent = service.GetEventById((int)id);

            if (volunteerEvent == null)
            {
                return HttpNotFound();
            }
            volunteerEvent.MealSite = mealSiteService.GetMealSiteById(volunteerEvent.MealSite_Id);
            SignUpEventViewModel signUpEventViewModel = new SignUpEventViewModel();
            Volunteer volunteer = GetCurrentVolunteer();

            List<int> IdList = volunteerEvent.VolunteerVolunteerEvents.Where(vve => !vve.IsItCanceled).Select(vve => vve.VolunteerId).ToList();
            signUpEventViewModel.Volunteer = volunteer;
            signUpEventViewModel.VolunteerEvent = volunteerEvent;
            signUpEventViewModel.Volunteers = volunteerService.GetVolunteersById(IdList);

            return View(signUpEventViewModel);
        }

        [Authorize(Roles = "volunteer")]
        public ActionResult VolunteerDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolunteerEvent volunteerEvent = service.GetEventById((int)id);

            if (volunteerEvent == null)
            {
                return HttpNotFound();
            }
            volunteerEvent.MealSite = mealSiteService.GetMealSiteById(volunteerEvent.MealSite_Id);
            SignUpEventViewModel signUpEventViewModel = new SignUpEventViewModel();
            Volunteer volunteer = GetCurrentVolunteer();

            List<int> IdList = volunteerEvent.VolunteerVolunteerEvents.Where(vve => !vve.IsItCanceled).Select(vve => vve.VolunteerId).ToList();
            signUpEventViewModel.Volunteer = volunteer;
            signUpEventViewModel.VolunteerEvent = volunteerEvent;
            signUpEventViewModel.Volunteers = volunteerService.GetVolunteersById(IdList);

            return View(signUpEventViewModel);
        }

        [Authorize(Roles = "volunteer")]
        public ActionResult RemoveVolunteerFromEvent(int? eventID, int? volunteerID)
        {
            if (eventID == null || volunteerID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            volunteerService.RemoveVolunteerVolunteerEvent((int)volunteerID, (int)eventID);

            return RedirectToAction("EventUnregistered", "Volunteer", new { volunteerId = (int)volunteerID, eventId=(int)eventID});
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolunteerEvent volunteerEvent = service.GetEventById((int)id);

            if (volunteerEvent == null)
            {
                return HttpNotFound();
            }

            VolunteerEventViewModel viewModel = new VolunteerEventViewModel(volunteerEvent)
            {
                DropDownItems = AllLocations(),
                MealSite = mealSiteService.GetMealSiteById(volunteerEvent.MealSite_Id),
                MealSiteId = volunteerEvent.MealSite_Id
            };

            return View(viewModel);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MealSiteId, Id, StartingShiftTime, EndingShiftTime, OpenSlots, Location, Description")] VolunteerEventViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.StartingShiftTime.CompareTo(viewModel.EndingShiftTime) < 0)
                {
                    VolunteerEvent volunteerEvent = new VolunteerEvent(viewModel);
                    volunteerEvent.MealSite_Id = viewModel.MealSiteId;
                    service.SaveEdits(volunteerEvent);

                    return RedirectToAction("ManageEvents", "AdminPortal", null);
                }

                viewModel.Error = INVALID_CALENDAR_DATES_ERROR;
                viewModel.DropDownItems = AllLocations();
                return View(viewModel);
            }
            return View(viewModel);
        }


        [Authorize(Roles ="volunteer")]
        public ActionResult VolunteerCalendar()
        {
            ViewModels.VolunteerCalendarViewModel viewModel = new ViewModels.VolunteerCalendarViewModel();
            viewModel.Volunteer = GetCurrentVolunteer();
            viewModel.VolunteerEvents = new List<VolunteerEvent>();

            foreach (VolunteerEvent volunteerEvent in service.GetAllEvents())
            {
                VolunteerEvent ve = volunteerEvent;
                ve.VolunteerVolunteerEvents = new List<VolunteerVolunteerEvent>();
                var events = volunteerService.GetVolunteerVolunteerEvents(GetCurrentVolunteer().Id);
                if (events != null)
                {
                    ve.VolunteerVolunteerEvents = events;
                }
                viewModel.VolunteerEvents.Add(ve);
            } 
            return View(viewModel);
        }
        [Authorize(Roles = "groupleader")]
        public ActionResult Calendar()
        {
            ViewModels.VolunteerGroupCalendarViewModel viewModel = new ViewModels.VolunteerGroupCalendarViewModel();
            viewModel.VolunteerGroupLeader = GetCurrentVolunteerGroupLeader();
            viewModel.VolunteerEvents = new List<VolunteerEvent>();

            foreach (VolunteerEvent volunteerEvent in service.GetAllEvents())
            {
                VolunteerEvent ve = volunteerEvent;
                ve.VolunteerGroupVolunteerEvents = new List<VolunteerGroupVolunteerEvent>();
                var events = volunteerGroupService.GetVolunteerGroupVolunteerEvents(GetCurrentVolunteerGroupLeader().Id);
                if (events != null)
                {
                    ve.VolunteerGroupVolunteerEvents = events;
                }
                viewModel.VolunteerEvents.Add(ve);
            }
            return View(viewModel);
        }

        public JsonResult GetEvents()
        {
            var events = service.GetAllEvents();

            foreach (var item in events)
            {
                item.MealSite.VolunteerEvents = null;
            }

            return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public List<SelectListItem> AllLocations()
        {
            List<MealSite> AllLocations = mealSiteService.GetAllMealSites();
            List<SelectListItem> Locations = new List<SelectListItem>();

            foreach (MealSite mealSite in AllLocations)
            {
                SelectListItem item = new SelectListItem
                {
                    Text = mealSite.AddressLine1,
                    Value = mealSite.Id.ToString()
                };

                Locations.Add(item);
            }
            return Locations;
        }
    }
}