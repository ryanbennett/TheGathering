using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            return dbContext.VolunteerEvents.Include(ve => ve.MealSite).ToList();
        }
        public void DeleteEvent(VolunteerEvent Event)
        {
            dbContext.VolunteerEvents.Remove(Event);
            dbContext.SaveChanges();
        }
        public VolunteerEvent GetEventById(int id)
        {
            return dbContext.VolunteerEvents.Include(ve => ve.VolunteerVolunteerEvents).Include(ve => ve.VolunteerGroupVolunteerEvents).Include(ve => ve.MealSite).FirstOrDefault(e => e.Id == id);
        }
        public List<VolunteerEvent> GetEventsByIds(List<int> eventId)
        {
            return dbContext.VolunteerEvents.Include(ve => ve.MealSite).Where(ve => eventId.Contains(ve.Id)).ToList();
        }
        public void SaveEdits(VolunteerEvent toSave)
        {
            var ve = dbContext.VolunteerEvents.FirstOrDefault(x => x.Id == toSave.Id);
            ve.StartingShiftTime = toSave.StartingShiftTime;
            ve.EndingShiftTime = toSave.EndingShiftTime;
            ve.OpenSlots = toSave.OpenSlots;
            ve.MealSite_Id = toSave.MealSite_Id;
            ve.Description = toSave.Description;
            ve.VolunteerVolunteerEvents = toSave.VolunteerVolunteerEvents;
            dbContext.SaveChanges();
        }
        public void IncreaseOpenSlots(VolunteerEvent volunteerEvent, int openSlots)
        {
            volunteerEvent.OpenSlots = openSlots + 1;
            dbContext.Entry(volunteerEvent).State = EntityState.Modified;
            dbContext.SaveChanges();
        }
        public void AddEvent(VolunteerEvent toAdd)
        {
            dbContext.VolunteerEvents.Add(toAdd);
            dbContext.SaveChanges();
        }
        public void ReduceOpenSlots(VolunteerEvent volunteerEvent, int openSlots)
        {
            volunteerEvent.OpenSlots = openSlots - 1;
            dbContext.Entry(volunteerEvent).State = EntityState.Modified;
            dbContext.SaveChanges();
        }
        public void VolunteerCanceled(int volunteerEventId)
        {
            VolunteerEvent volunteerEvent = GetEventById(volunteerEventId);
            volunteerEvent.OpenSlots++;
            dbContext.Entry(volunteerEvent).State = EntityState.Modified;
            dbContext.SaveChanges();
        }
    }
}