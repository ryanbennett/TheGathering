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



        public void EditLeader(VolunteerGroupLeader volunteergroupleader)
        {
            _context.Entry(volunteergroupleader).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public VolunteerGroupLeader GetLeaderById(int id)
        {
            var result = _context.VolunteerGroupLeaders.SingleOrDefault(volunteergroupleader => volunteergroupleader.Id == id);
            return result;
        }
        public VolunteerGroupLeader GetLeaderByApplicationUserId(String applicationUserId)
        {
            var result = _context.VolunteerGroupLeaders.SingleOrDefault(volunteergroupleader => volunteergroupleader.ApplicationUserId == applicationUserId);
            return result;
        }
    }
}