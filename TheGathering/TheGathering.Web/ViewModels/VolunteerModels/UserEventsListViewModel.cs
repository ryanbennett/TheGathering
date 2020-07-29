using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheGathering.Web.Models;
namespace TheGathering.Web.ViewModels.VolunteerModels
{
    public class UserEventsListViewModel
    {
        public List<VolunteerEvent> VolunteerEvents { get; set; }
        public List<VolunteerEvent> CancelledEvents { get; set; }
    }

    public SeparateCurrentAndOldEvents()
    {
        List<VolunteerEvent> PastEvents = new List<VolunteerEvent>;
        List<VolunteerEvent> CurrentEvents = new List<VolunteerEvent>;
        foreach(var item in VolunteerEvents)
        {
            if (item.StartingShiftTime > DateTime.Now)
                CurrentEvents.Add(item);
            else
                PastEvents.Add(item);
        }
    }
}