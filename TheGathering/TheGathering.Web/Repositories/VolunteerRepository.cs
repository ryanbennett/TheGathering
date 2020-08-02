using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TheGathering.Web.Models;

namespace TheGathering.Web.Repositories
{
    public class VolunteerRepository
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        public void Create(Volunteer volunteer)
        {
            //volunteer.VolunteerVolunteerEvents = new List<VolunteerVolunteerEvent>();
            _context.Volunteers.Add(volunteer);
            _context.SaveChanges();
        }


        public void Edit(Volunteer volunteer)
        {
            _context.Entry(volunteer).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public Volunteer GetById(int id)
        {
            var result = _context.Volunteers.SingleOrDefault(volunteer => volunteer.Id == id);
            return result;
        }
        public Volunteer GetByApplicationUserId(String applicationUserId)
        {
            var result = _context.Volunteers.SingleOrDefault(volunteer => volunteer.ApplicationUserId == applicationUserId);
            return result;
        }

        public Volunteer GetVolunteerById(int id)
        {
            Volunteer result = _context.Volunteers.Find(id);
            return result;
        }

        public List<Volunteer> GetAllVolunteers()
        {
            return _context.Volunteers.ToList();
        }

        public List<VolunteerVolunteerEvent> GetVolunteerEventIdsByVolunteerId(int volunteerId)
        {
            var volunteer = _context.Volunteers.Include(v=>v.VolunteerVolunteerEvents).SingleOrDefault(v => v.Id == volunteerId);
            return volunteer.VolunteerVolunteerEvents.Where(vve => !vve.IsItCanceled).ToList();
        }

        public List<VolunteerVolunteerEvent> GetCancelledVolunteerEventIdsByVolunteerId(int volunteerId)
        {
            var volunteer = _context.Volunteers.Include(v => v.VolunteerVolunteerEvents).SingleOrDefault(v => v.Id == volunteerId);
            return volunteer.VolunteerVolunteerEvents.Where(vve => vve.IsItCanceled).ToList();
        }

        public void DeleteVolunteer(Volunteer volunteer)
        {
            _context.Volunteers.Remove(volunteer);
            _context.SaveChanges();
        }

        public void AddVolunteerVolunteerEvent(Volunteer volunteer, VolunteerVolunteerEvent vve)
        {
            if(volunteer.VolunteerVolunteerEvents == null)
                volunteer.VolunteerVolunteerEvents = new List<VolunteerVolunteerEvent>();
            volunteer.VolunteerVolunteerEvents.Add(vve);
            _context.Entry(volunteer).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public List<Volunteer> GetVolunteersById(List<int> Ids)
        {
            return _context.Volunteers.Where(vol => Ids.Contains(vol.Id)).ToList();

        }

        public void RemoveVolunteerVolunteerEvent(int volunteerId, int eventId)
        {
            Volunteer volunteer = _context.Volunteers.Include(v => v.VolunteerVolunteerEvents).SingleOrDefault(v => v.Id == volunteerId);
            VolunteerVolunteerEvent toDelete = volunteer.VolunteerVolunteerEvents.Find(vve => vve.VolunteerEventId == eventId);
            toDelete.IsItCanceled = true; // Fixes issue with Cacading Deletes (Do not delete VolunteerVolunteerEvents)
            _context.Entry(volunteer).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void ReSignUpForVolunteerVolunteerEvent(int volunteerId, int eventId)
        {
            Volunteer volunteer = _context.Volunteers.Include(v => v.VolunteerVolunteerEvents).SingleOrDefault(v => v.Id == volunteerId);
            VolunteerVolunteerEvent toAdd = volunteer.VolunteerVolunteerEvents.Find(vve => vve.VolunteerEventId == eventId);
            toAdd.IsItCanceled = false;//If vve is there but has been cancelled, will undo that
            _context.Entry(volunteer).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}