using System;
using System.Collections.Generic;
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
            var newVolunteer = _context.Volunteers.Add(volunteer);
            _context.SaveChanges();
        }

        public Volunteer GetVolunteerById(int id)
        {
            return _context.Volunteers.Find(id);
        }

        public void DeleteVolunteer(Volunteer volunteer)
        {
            _context.Volunteers.Remove(volunteer);
            _context.SaveChanges();
        }
    }
}