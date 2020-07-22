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

    }
}