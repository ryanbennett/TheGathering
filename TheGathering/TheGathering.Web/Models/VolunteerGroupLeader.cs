﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheGathering.Web.Models
{
    public class VolunteerGroupLeader
    {
        public int Id { get; set; }
        public string LeaderFirstName { get; set; }
        public string LeaderLastName { get; set; }
        public DateTime LeaderBirthday { get; set; }
        public string LeaderEmail { get; set; }
        public string LeaderPhoneNumber { get; set; }
        public string GroupName { get; set; }
        public Boolean SignUpForNewsLetter { get; set; }
        public List<VolunteerGroupVolunteerEvent> VolunteerGroupVolunteerEvents { get; set; } = new List<VolunteerGroupVolunteerEvent>();
        public String ApplicationUserId { get; set; }
        public int TotalGroupMembers { get; set; }

        public bool IsAccountActive { get; set; }
        public VolunteerGroupLeader()
        {
            IsAccountActive = true;
        }
    }
}