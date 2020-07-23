using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace TheGathering.Web.Models
{
    public class VolunteerVolunteerEvent
    {
        public int Id { get; set; }
        public int VolunteerId { get; set; }
        public int VolunteerEventId { get; set; }
        public bool Confirmed { get; set; }
        public bool IsItCanceled { get; set; }

        public VolunteerVolunteerEvent()
        {
            IsItCanceled = false;
        }
    }
}