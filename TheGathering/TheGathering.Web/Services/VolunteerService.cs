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
        CalendarRepository calendarRepo = new CalendarRepository();

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

        public List<int> GetVolunteerEventIdsByVolunteerId(int volunteerId)
        {
            List<int> volunteerEventIds = new List<int>();
            List<VolunteerVolunteerEvent> volunteerVolunteerEvents = _repository.GetVolunteerEventIdsByVolunteerId(volunteerId);
            foreach (VolunteerVolunteerEvent vve in volunteerVolunteerEvents)
            {
                volunteerEventIds.Add(vve.VolunteerEventId);
            }
            return volunteerEventIds;
        }

        public List<int> GetCancelledVolunteerEventIdsByVolunteerId(int volunteerId)
        {
            List<int> volunteerEventIds = new List<int>();
            List<VolunteerVolunteerEvent> volunteerVolunteerEvents = _repository.GetCancelledVolunteerEventIdsByVolunteerId(volunteerId);
            foreach (VolunteerVolunteerEvent vve in volunteerVolunteerEvents)
            {
                volunteerEventIds.Add(vve.VolunteerEventId);
            }
            return volunteerEventIds;
        }

        public void DeleteVolunteer(Volunteer volunteer)
        {
            _repository.DeleteVolunteer(volunteer);
        }

        public Volunteer GetByApplicationUserId(string userId)
        {
            return _repository.GetByApplicationUserId(userId);
        }

        public void Edit(Volunteer volunteer)
        {
            _repository.Edit(volunteer);
        }

        public void AddVolunteerVolunteerEvent(int volunteerId, int eventId)
        {
            VolunteerVolunteerEvent vve = new VolunteerVolunteerEvent();
            //vve.Id = 1;
            //TODO: Changes vve.Id value
            vve.VolunteerId = volunteerId;
            vve.VolunteerEventId = eventId;
            vve.Confirmed = false;
            Volunteer volunteer = _repository.GetVolunteerById(volunteerId);
            _repository.AddVolunteerVolunteerEvent(volunteer, vve);
        }

        public List<Volunteer> GetVolunteersById(List<int> Ids)
        {
            return _repository.GetVolunteersById(Ids);
        }

        public void RemoveVolunteerVolunteerEvent(int volunteerId, int eventId)
        {
            _repository.RemoveVolunteerVolunteerEvent(volunteerId, eventId);
        }
    }
}