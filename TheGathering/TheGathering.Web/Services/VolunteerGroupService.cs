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


        public VolunteerGroupLeader GetLeaderByApplicationUserId(string userId)
        {
            return _repository.GetLeaderByApplicationUserId(userId);
        }

        public void EditLeader(VolunteerGroupLeader volunteergroupleader)
        {
            _repository.EditLeader(volunteergroupleader);
        }

    }
}