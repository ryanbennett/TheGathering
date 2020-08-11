using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheGathering.Web.Models;

namespace TheGathering.Web.ViewModels
{
    public class SortedEventsViewModel
    {
        public List<VolunteerEvent> Events { get; set; }
        public List<SelectListItem> SortItems { get; set; }
    }
}