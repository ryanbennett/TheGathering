using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheGathering.Web.Models;

namespace TheGathering.Web.Repositories
{
    public class CalendarRepository
    {
        private ApplicationDbContext dbContext;
        public CalendarRepository()
        {
            dbContext = new ApplicationDbContext();
        }

        public List<VolunteerEvent> GetAllEvents()
        {
            return dbContext.VolunteerEvents.ToList();
        }

        public void CreateEvent(VolunteerEvent Event)
        {
            dbContext.VolunteerEvents.Add(Event);
            dbContext.SaveChanges();
        }
        public VolunteerEvent GetEventById(int id)
        {
            return dbContext.VolunteerEvents.Find(id);

        }
        public void AddEvent(VolunteerEvent toAdd)
        {
            dbContext.VolunteerEvents.Add(toAdd);
            dbContext.SaveChanges();
        }
    }
}