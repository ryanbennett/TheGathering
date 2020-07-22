using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TheGathering.Web.Models;

namespace TheGathering.Web.Repositories
{
    public class MealSiteRepository
    {
        private ApplicationDbContext dbContext;

        public MealSiteRepository()
        {
            dbContext = new ApplicationDbContext();
        }

        public List<MealSite> GetAllMealSites()
        {
            return dbContext.MealSites.ToList();
        }

        public void AddMealSite(MealSite mealSite)
        {
            dbContext.MealSites.Add(mealSite);
            dbContext.SaveChanges();
        }

        public void DeleteMealSite(MealSite mealSite)
        {
            dbContext.MealSites.Remove(mealSite);
            dbContext.SaveChanges();
        }

        public MealSite GetMealSiteById(int id)
        {
            MealSite mealSite = dbContext.MealSites.Include(m => m.VolunteerEvents).SingleOrDefault(m => m.Id == id);

            return mealSite;
        }

        public void UpdateMealSite(MealSite mealSite)
        {
            dbContext.Entry(mealSite).State = EntityState.Modified;
            dbContext.SaveChanges();
        }

    }
}