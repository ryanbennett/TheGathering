﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public List<Volunteer> GetAllVolunteers()
        {
            return _repository.GetAllVolunteers();
        }

        public Volunteer GetVolunteerById(int id)
        {
            return _repository.GetVolunteerById(id);
        }

        public void DeleteVolunteer(Volunteer volunteer)
        {
            _repository.DeleteVolunteer(volunteer);
        }
    }
}