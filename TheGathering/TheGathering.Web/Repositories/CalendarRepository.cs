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
        public void CreateEvent(VolunteerEvent Event)
        {
            dbContext.VolunteerEvents.Add(Event);
            dbContext.SaveChanges();
        }
    }
}