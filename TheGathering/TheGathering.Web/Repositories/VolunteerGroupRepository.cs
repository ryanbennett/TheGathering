using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TheGathering.Web.Models;

namespace TheGathering.Web.Repositories
{
    public class VolunteerGroupRepository
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        public void CreateLeader(VolunteerGroupLeader volunteergroupleader)
        {
            //volunteer.VolunteerVolunteerEvents = new List<VolunteerVolunteerEvent>();
            _context.VolunteerGroupLeaders.Add(volunteergroupleader);
            _context.SaveChanges();
        }
        public List<VolunteerGroupVolunteerEvent> GetVolunteerEventIdsByVolunteerGroupId(int volunteerId)
        {
            var volunteer = _context.VolunteerGroupLeaders.Include(v => v.VolunteerGroupVolunteerEvents).SingleOrDefault(v => v.Id == volunteerId);
            return volunteer.VolunteerGroupVolunteerEvents.ToList();
        }

        public void EditLeader(VolunteerGroupLeader volunteergroupleader)
        {
            _context.Entry(volunteergroupleader).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public List<VolunteerGroupLeader> GetAllVolunteerGroups()
        {
            return _context.VolunteerGroupLeaders.ToList();
        }
        public VolunteerGroupLeader GetLeaderById(int id)
        {
            var result = _context.VolunteerGroupLeaders.Include(vgl=>vgl.VolunteerGroupVolunteerEvents.Select(vgve=>vgve.VolunteerEvent)).SingleOrDefault(volunteergroupleader => volunteergroupleader.Id == id);
            return result;
        }
        public void DeleteLeader(VolunteerGroupLeader volunteergroupleader)
        {
            _context.VolunteerGroupLeaders.Remove(volunteergroupleader);
            _context.SaveChanges();
        }
        public VolunteerGroupLeader GetLeaderByApplicationUserId(String applicationUserId)
        {
            var result = _context.VolunteerGroupLeaders.Include(vgl => vgl.VolunteerGroupVolunteerEvents.Select(vgve => vgve.VolunteerEvent)).SingleOrDefault(volunteergroupleader => volunteergroupleader.ApplicationUserId == applicationUserId);
            return result;
        }
        public void AddVolunteerGroupVolunteerEvent(VolunteerGroupLeader volunteergroupleader, VolunteerGroupVolunteerEvent vgve)
        {
            if (volunteergroupleader.VolunteerGroupVolunteerEvents == null)
                volunteergroupleader.VolunteerGroupVolunteerEvents = new List<VolunteerGroupVolunteerEvent>();
            volunteergroupleader.VolunteerGroupVolunteerEvents.Add(vgve);
            _context.Entry(volunteergroupleader).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void ReduceOpenSlots(VolunteerEvent volunteerEvent, int origOpenSlots, int numVolunteers)
        {
            volunteerEvent.OpenSlots = origOpenSlots - numVolunteers;
            _context.Entry(volunteerEvent).State = EntityState.Modified;
            _context.SaveChanges();

        }
    }
}