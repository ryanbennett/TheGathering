using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TheGathering.Web.Models
{
    public class VolunteerEventViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Starting Shift Time")]
        public string StartingShiftTime { get; set; }

        [Display(Name = "Ending Shift Time")]
        public string EndingShiftTime { get; set; }
        [Display(Name = "Open Slots")]
        public int OpenSlots { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
    }
}