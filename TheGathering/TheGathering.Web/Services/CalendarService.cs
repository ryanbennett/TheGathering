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
    }
}
