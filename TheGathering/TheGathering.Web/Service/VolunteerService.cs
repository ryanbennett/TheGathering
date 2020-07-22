using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using TheGathering.Web.Models;
using TheGathering.Web.Repositories;

namespace TheGathering.Web.Service
{
    public class VolunteerService
    {
        VolunteerRepository _repository = new VolunteerRepository();
        public void Create(Volunteer volunteer)
        {
            _repository.Create(volunteer);
        }
        public Volunteer GetById(int id)
        {

            return _repository.GetById(id);
        }

        public Volunteer GetByApplicationUserId(String applicationUserId)
        {

            return _repository.GetByApplicationUserId(applicationUserId);
        }

        public List<Volunteer> GetAllVolunteers()
        {
            return _repository.GetAllVolunteers();
        }

        public void DeleteVolunteer(Volunteer volunteer)
        {
            _repository.DeleteVolunteer(volunteer);
        }
        public void Edit(Volunteer volunteer)
        {
            _repository.Edit(volunteer);
        }
    }
}