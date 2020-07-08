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
            var newVolunteer = _context.Volunteers.Add(volunteer);
            _context.SaveChanges();
        }


        public void Edit(Volunteer volunteer)
        {
            _context.Entry(volunteer).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}