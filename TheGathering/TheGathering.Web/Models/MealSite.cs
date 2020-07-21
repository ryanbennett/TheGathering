using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheGathering.Web.Models
{
    public class MealSite
    {
        public int Id { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string CrossStreet1 { get; set; }
        public string CrossStreet2 { get; set; }
        public string MealServed { get; set; }
        public string DaysServed { get; set; }
        public int MaximumGuestsServed { get; set; }
        public int MinimumGuestsServed { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<VolunteerEvent> VolunteerEvents { get; set; }
    }
}