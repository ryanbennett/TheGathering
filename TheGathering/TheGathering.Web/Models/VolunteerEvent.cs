using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheGathering.Web.Models
{
    public class VolunteerEvent
    {
        public int Id { get; set; }
        public DateTime StartingShiftTime { get; set; }
        public TimeSpan ShiftSpan { get; set; }
        public int OpenSlots { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
    }
}