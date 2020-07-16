using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using TheGathering.Web.Models;
using TheGathering.Web.Repositories;

namespace TheGathering.Web.Services
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

        public List<Volunteer> GetAllVolunteers()
        {
            return _repository.GetAllVolunteers();
        }

        public List<int> GetVolunteerEventsById(int volunteerId)
        {
            List<int> volunteerEventIds = new List<int>();
            foreach (VolunteerVolunteerEvent vve in _repository.GetVolunteerEventsById(volunteerId))
            {
                volunteerEventIds.Add(vve.VolunteerEventId);
            }
            return volunteerEventIds;
        }

        public void DeleteVolunteer(Volunteer volunteer)
        {
            _repository.DeleteVolunteer(volunteer);
        }
        public void Edit(Volunteer volunteer)
        {
            _repository.Edit(volunteer);
        }

        public void AddVolunteerVolunteerEvent(int volunteerId, int eventId)
        {
            VolunteerVolunteerEvent vve = new VolunteerVolunteerEvent();
            //vve.Id = 0;
            //TODO: Changes vve.Id value
            vve.VolunteerId = volunteerId;
            vve.VolunteerEventId = eventId;
            vve.Confirmed = false;
            Volunteer volunteer = _repository.GetById(volunteerId);
            _repository.AddVolunteerVolunteerEvent(volunteer, vve);
        }
    }
}