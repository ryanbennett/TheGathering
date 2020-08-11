using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TheGathering.Web.Models;

namespace TheGathering.Web.ViewModels
{
    public class VolunteerEmailViewModel
    {
        public int EventId { get; set; }

        public Models.VolunteerEvent VolunteerEvent { get; set; }

        public string Subject { get; set; }

        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}