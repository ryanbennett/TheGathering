using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheGathering.Web.Sorting.VolunteerEventSorts
{
    public enum EventSortType
    {
        /// <summary>
        /// No sorting
        /// </summary>
        NewestAdded,
        /// <summary>
        /// Sorts the list by number of open slots, starting with the most slots
        /// </summary>
        OpenSlots,
        /// <summary>
        /// Sorts the list by location, starting with the closest location
        /// </summary>
        LocationDistance,
        /// <summary>
        /// Sorts the list by start date, starting with the latest events
        /// </summary>
        DateLatest,
        /// <summary>
        /// Sorts the list by start date, starting with the earliest events
        /// </summary>
        DateEarliest
    }
}