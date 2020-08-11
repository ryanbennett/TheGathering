using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheGathering.Web.Models;

namespace TheGathering.Web.Sorting.VolunteerEventSorts
{
    public static class VolunteerEventSortsManager
    {
        public static List<SelectListItem> GetSortSelectItems(EventSortType currentSelection = EventSortType.NewestAdded)
        {
            List<SelectListItem> selectItems = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Newest Added",
                    Value = EventSortType.NewestAdded.ToString()
                },

                new SelectListItem
                {
                    Text = "Open Slots",
                    Value = EventSortType.OpenSlots.ToString(),
                },

                new SelectListItem
                {
                    Text = "Latest Start Date",
                    Value = EventSortType.DateLatest.ToString()
                },

                new SelectListItem
                {
                    Text = "Earliest Start Date",
                    Value = EventSortType.DateEarliest.ToString()
                }
            };

            selectItems.Find(item => item.Value == currentSelection.ToString()).Selected = true;

            return selectItems;
        }
    }

    public class SortByOpenSlots : IComparer<VolunteerEvent>
    {
        public int Compare(VolunteerEvent x, VolunteerEvent y)
        {
            return x.OpenSlots.CompareTo(y.OpenSlots) * -1;
        }
    }

    public class SortByDateLatest : IComparer<VolunteerEvent>
    {
        public int Compare(VolunteerEvent x, VolunteerEvent y)
        {
            return x.StartingShiftTime.CompareTo(y.StartingShiftTime) * -1;
        }
    }

    public class SortByDateEarliest : IComparer<VolunteerEvent>
    {
        public int Compare(VolunteerEvent x, VolunteerEvent y)
        {
            return x.StartingShiftTime.CompareTo(y.StartingShiftTime);
        }
    }
}