using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheGathering.Web.Models;
using TheGathering.Web.Repositories;

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
        public void CreateEvent(VolunteerEvent Event)
        {
            repository.CreateEvent(Event);
        }
        public VolunteerEvent GetEventById(int id)
        {
            return repository.GetEventById(id);
        }
        public void AddEvent(VolunteerEvent toAdd)
        {
            repository.AddEvent(toAdd);
        }
        }
            repository.DeleteEvent(Event);
        {
        public void DeleteEvent(VolunteerEvent Event)
    }
}
