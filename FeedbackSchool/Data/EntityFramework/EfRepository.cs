using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FeedbackSchool.Interfaces;
using FeedbackSchool.Models;

namespace FeedbackSchool.Data.EntityFramework
{
    public class EfRepository : IRepository<Guest>
    {
        public IEnumerable<Guest> GetAllList()
        {
            using var context = new ApplicationContext();
            return context.FeedbackList.ToList();
        }

        public Task Add(Guest item)
        {
            using var context = new ApplicationContext();

            context.FeedbackList.Add(new Guest()
            {
                School = item.School,
                Class = item.Class,
                Name = item.Name,
                Feedback = item.Feedback,
                FavoriteLessons = item.FavoriteLessons,
                DateTime = DateTime.Now.ToString(CultureInfo.CurrentCulture)
            });

            context.SaveChanges();

            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            using var context = new ApplicationContext();
            context.FeedbackList.Remove(new Guest
            {
                Id = id
            });
            context.SaveChanges();

            return Task.CompletedTask;
        }

        public Task DeleteAll()
        {
            
            //BUG ФУЛЛ ПАРАША, ДЛЯ AdminController ЛУЧШЕ ЮЗАТЬ РЕАЛИЗАЦИЮ С ДАППЕРОМ
            
            using var context = new ApplicationContext();

            foreach (var guest in new ApplicationContext().FeedbackList)
                context.FeedbackList.Remove(new Guest()
                {
                    Id = guest.Id
                });

            context.SaveChanges();
            return Task.CompletedTask;
        }
    }
}