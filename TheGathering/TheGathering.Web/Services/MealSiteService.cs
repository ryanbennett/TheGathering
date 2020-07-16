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

        public MealSiteService()
        {
            mealSiteRepository = new MealSiteRepository();
        }

        public List<MealSite> GetAllMealSites()
        {
            return mealSiteRepository.GetAllMealSites();
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
            mealSite.VolunteerEventIdsAtMealSite.Add(volunteerEventId);
            mealSiteRepository.UpdateMealSite(mealSite);
        }
        public void DeleteVolunteerEvent(int volunteerEventId, int mealSiteId) {
            MealSite mealSite = mealSiteRepository.GetMealSiteById(mealSiteId);
            mealSite.VolunteerEventIdsAtMealSite.Remove(volunteerEventId);
            mealSiteRepository.UpdateMealSite(mealSite);
        }

    }
}