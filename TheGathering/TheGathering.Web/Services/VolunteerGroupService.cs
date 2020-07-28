using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheGathering.Web.Models;
using TheGathering.Web.Repositories;

namespace TheGathering.Web.Services
{
    public class VolunteerGroupService
    {
        VolunteerGroupRepository _repository = new VolunteerGroupRepository();
        public void CreateLeader(VolunteerGroupLeader volunteergroupleader)
        {
            _repository.CreateLeader(volunteergroupleader);
        }

        public VolunteerGroupLeader GetLeaderById(int id)
        {

            return _repository.GetLeaderById(id);
        }
        public void DeleteLeader(VolunteerGroupLeader volunteergroupleader)
        {
            _repository.DeleteLeader(volunteergroupleader);
        }
        public List<VolunteerGroupLeader> GetAllVolunteerGroups()
        {
            return _repository.GetAllVolunteerGroups();
        }

        public VolunteerGroupLeader GetLeaderByApplicationUserId(string userId)
        {
            return _repository.GetLeaderByApplicationUserId(userId);
        }

        public void EditLeader(VolunteerGroupLeader volunteergroupleader)
        {
            _repository.EditLeader(volunteergroupleader);
        }
        public List<int> GetVolunteerGroupEvents(int volunteerId)
        {
            List<int> volunteerEventIds = new List<int>();
            foreach (VolunteerGroupVolunteerEvent vve in _repository.GetVolunteerEventIdsByVolunteerGroupId(volunteerId))
            {
                volunteerEventIds.Add(vve.VolunteerEventId);
            }
            return volunteerEventIds;
        }
        public void AddVolunteerGroupVolunteerEvent(int volunteerGroupId, int eventId, int numVolunteers)
        {
            
            VolunteerGroupVolunteerEvent vgve = new VolunteerGroupVolunteerEvent();
            //vve.Id = 1;
            //TODO: Changes vve.Id value
            vgve.VolunteerGroupId = volunteerGroupId;
            vgve.VolunteerEventId = eventId;
            vgve.Confirmed = false;
            vgve.NumberOfGroupMembersSignedUp = numVolunteers;
            VolunteerGroupLeader volunteergroupleader = _repository.GetLeaderById(volunteerGroupId);
            _repository.AddVolunteerGroupVolunteerEvent(volunteergroupleader, vgve);
        }
       

    }
}