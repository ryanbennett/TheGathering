using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheGathering.Web.Models;

namespace TheGathering.Web.ViewModels
{
    public class VolunteerEmailViewModel
    {
        public VolunteerEvent VolunteerEvent { get; set; }

        public string Message { get; set; }
    }
}