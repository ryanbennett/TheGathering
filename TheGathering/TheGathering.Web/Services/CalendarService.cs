using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheGathering.Web.Models;
using TheGathering.Web.Repositories;
using TheGathering.Web.Sorting.VolunteerEventSorts;

namespace TheGathering.Web.Services
{
    public class CalendarService
    {
        private CalendarRepository repository;
        public CalendarService()
        {
            repository = new CalendarRepository();
        }
        

        public List<VolunteerEvent> GetAllEvents()
        {
            return repository.GetAllEvents();
        }

        public List<VolunteerEvent> GetSortedEvents(EventSortType sort)
        {
            switch (sort)
            {
                case EventSortType.NewestAdded:
                    return GetAllEvents();

                case EventSortType.OpenSlots:
                    var openSlotEvents = GetAllEvents();
                    openSlotEvents.Sort(new SortByOpenSlots());

                    return openSlotEvents;

                case EventSortType.LocationDistance:

                    break;

                case EventSortType.DateLatest:
                    var dateLatestEvents = GetAllEvents();
                    dateLatestEvents.Sort(new SortByDateLatest());

                    return dateLatestEvents;

                case EventSortType.DateEarliest:
                    var dateEarliestEvents = GetAllEvents();
                    dateEarliestEvents.Sort(new SortByDateEarliest());

                    return dateEarliestEvents;

                default:
                    //This shouldn't happen, all sort types should be handled

                    return GetAllEvents();
            }

            return GetAllEvents();
        }

        public VolunteerEvent GetEventById(int id)
        {
            return repository.GetEventById(id);
        }
        public List<VolunteerEvent> GetEventsByIds(List<int> id)
        {
            return repository.GetEventsByIds(id);
        }
        public void SaveEdits(VolunteerEvent Event)
        {
            repository.SaveEdits(Event);
        }
        public void DeleteEvent(VolunteerEvent Event)
        {
            repository.DeleteEvent(Event);
        }
        public void AddEvent(VolunteerEvent toAdd)
        {
            repository.AddEvent(toAdd);
        }
        public void ReduceOpenSlots(VolunteerEvent volunteerEvent, int openSlots)
        {
            repository.ReduceOpenSlots(volunteerEvent, openSlots);
        }

        public void IncreaseOpenSlots(VolunteerEvent volunteerEvent, int openSlots)
        {
            repository.IncreaseOpenSlots(volunteerEvent, openSlots);
        }
        public void VolunteerCanceled(int volunteerEventId)
        {
            repository.VolunteerCanceled(volunteerEventId);
        }
    }
}
