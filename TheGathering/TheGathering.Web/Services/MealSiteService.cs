using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheGathering.Web.Models;
using TheGathering.Web.Repositories;

namespace TheGathering.Web.Services
{
    public class MealSiteService
    {
        private MealSiteRepository mealSiteRepository;
        private CalendarRepository volunteerEventRepository;

        public MealSiteService()
        {
            mealSiteRepository = new MealSiteRepository();
            volunteerEventRepository = new CalendarRepository();
        }

        public List<MealSite> GetAllMealSites()
        {
            return mealSiteRepository.GetAllMealSites();
        }

        public List<MealSite> GetAllActiveAndInActiveMealSites()
        {
            return mealSiteRepository.GetAllActiveAndInActiveMealSites();
        }

        public void AddMealSite(MealSite mealSite)
        {
            mealSiteRepository.AddMealSite(mealSite);
        }

        public void DeleteMealSite(MealSite mealSite)
        {
            mealSiteRepository.DeleteMealSite(mealSite);
        }

        public void DeleteMealSite(int mealSiteID)
        {
            mealSiteRepository.DeleteMealSite(GetMealSiteById(mealSiteID));
        }

        public void ChangeMealSiteActivation(MealSite mealSite, bool active)
        {
            mealSiteRepository.ChangeMealSiteActivation(mealSite, active);
        }

        public MealSite GetMealSiteById(int id)
        {
            return mealSiteRepository.GetMealSiteById(id);
        }

      
        public void UpdateMealSite(MealSite mealSite)
        {
            mealSiteRepository.UpdateMealSite(mealSite);
        }

        public void AddVolunteerEvent(int volunteerEventId, int mealSiteId)
        {
            MealSite mealSite = mealSiteRepository.GetMealSiteById(mealSiteId);
            VolunteerEvent volunteerEvent = volunteerEventRepository.GetEventById(volunteerEventId);

            mealSite.VolunteerEvents.Add(volunteerEvent);
            mealSiteRepository.UpdateMealSite(mealSite);
        }

        public void DeleteVolunteerEvent(int volunteerEventId, int mealSiteId) 
        {
            MealSite mealSite = mealSiteRepository.GetMealSiteById(mealSiteId);
            VolunteerEvent volunteerEvent = volunteerEventRepository.GetEventById(volunteerEventId);
            mealSite.VolunteerEvents.Remove(volunteerEvent);
            mealSiteRepository.UpdateMealSite(mealSite);
        }

    }
}