using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FeedbackSchool.Models;

namespace FeedbackSchool.Data.EntityFramework
{
    public class EfRepository : IRepository<Guest, FeedbackModel>
    {
        public IEnumerable<Guest> GetAllList()
        {
            using var context = new ApplicationContext();
            return context.FeedbackList.ToList();
        }

        public Task AddFeedback(Guest item)
        {
            using var context = new ApplicationContext();

            context.FeedbackList.Add(new Guest()
            {
                School = item.School,
                Class = item.Class,
                Name = item.Name,
                Feedback = item.Feedback,
                FavoriteLessons = item.FavoriteLessons ?? string.Empty,
                DateTime = DateTime.Now.ToString(CultureInfo.CurrentCulture)
            });

            context.SaveChanges();

            return Task.CompletedTask;
        }

        public Task DeleteFeedback(int id)
        {
            using var context = new ApplicationContext();
            context.FeedbackList.Remove(new Guest
            {
                Id = id
            });
            context.SaveChanges();

            return Task.CompletedTask;
        }

        public Task DeleteAllFeedback()
        {           
            using var context = new ApplicationContext();

            foreach (var guest in new ApplicationContext().FeedbackList)
                context.FeedbackList.Remove(new Guest()
                {
                    Id = guest.Id
                });

            context.SaveChanges();
            return Task.CompletedTask;
        }

        public IEnumerable<FeedbackModel> GetSchoolClass()
        {
            using var context = new ApplicationContext();
            return context.FeedbackModel.ToList();
        }

        public Task AddSchool(FeedbackModel item)
        {
            using var context = new ApplicationContext();

            context.FeedbackModel.Add(new FeedbackModel()
            {
                School = item.School
            });

            context.SaveChanges();

            return Task.CompletedTask;
        }

        public Task DeleteSchoolOrClass(FeedbackModel item)
        {
            using var context = new ApplicationContext();

            context.FeedbackModel.Remove(new FeedbackModel()
            {
                Id = item.Id
            });

            context.SaveChanges();

            return Task.CompletedTask;
        }

        public Task AddClass(FeedbackModel item)
        {
            using var context = new ApplicationContext();

            context.FeedbackModel.Add(new FeedbackModel()
            {
                Class = item.Class
            });

            context.SaveChanges();

            return Task.CompletedTask;
        }
    }
}
