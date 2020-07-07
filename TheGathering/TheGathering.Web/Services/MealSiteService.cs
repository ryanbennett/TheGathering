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

        public MealSite GetMealSiteById(int id)
        {
            return mealSiteRepository.GetMealSiteById(id);
        }

        public void UpdateMealSite(MealSite mealSite)
        {
            mealSiteRepository.UpdateMealSite(mealSite);
        }
    }
}