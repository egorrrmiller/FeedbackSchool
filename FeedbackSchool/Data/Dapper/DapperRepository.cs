using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using Dapper;
using FeedbackSchool.Models;
using Microsoft.Data.Sqlite;

namespace FeedbackSchool.Data.Dapper
{
    public class DapperRepository : IRepository<Guest, FeedbackModel>
    {
        public IEnumerable<Guest> GetAllList()
        {
            using IDbConnection db = new SqliteConnection(Startup.Connection);
            if (db.State == ConnectionState.Closed)
                db.Open();

            return db.Query<Guest>("SELECT * FROM FeedbackList", commandType: CommandType.Text);
        }

        public Task AddFeedback(Guest item)
        {
            using IDbConnection db = new SqliteConnection(Startup.Connection);
            if (db.State == ConnectionState.Closed)
                db.Open();

            db.Query<Guest>($"INSERT INTO FeedbackList (School, Class, Name, Feedback, FavoriteLessons, DateTime) VALUES (@School, @Class, @Name, @Feedback, @FavoriteLessons, @DateTime)",
                new
                {
                    School = item.School,
                    Class = item.Class,
                    Name = item.Name,
                    Feedback = item.Feedback,
                    FavoriteLessons = item.FavoriteLessons,
                    DateTime = DateTime.Now.ToString(CultureInfo.CurrentCulture)
                }, commandType: CommandType.Text);
            
            return Task.CompletedTask;
        }

        public Task DeleteFeedback(int id)
        {
            using IDbConnection db = new SqliteConnection(Startup.Connection);
            if (db.State == ConnectionState.Closed)
                db.Open();
            db.Query<Guest>($"DELETE FROM FeedbackList where Id =@Id", new { Id = id }, commandType: CommandType.Text);
            
            return Task.CompletedTask;
        }

        public Task DeleteAllFeedback()
        {
            using IDbConnection db = new SqliteConnection(Startup.Connection);
            if (db.State == ConnectionState.Closed)
                db.Open();
            
            db.Query<Guest>($"DELETE FROM FeedbackList WHERE 1", commandType: CommandType.Text);
            
            return Task.CompletedTask;
        }

        public IEnumerable<FeedbackModel> GetSchoolClass()
        {
            using IDbConnection db = new SqliteConnection(Startup.Connection);
            if (db.State == ConnectionState.Closed)
                db.Open();

            return db.Query<FeedbackModel>("SELECT * FROM FeedbackModel", commandType: CommandType.Text);
        }

        public Task AddSchool(FeedbackModel item)
        {
            using IDbConnection db = new SqliteConnection(Startup.Connection);
            if (db.State == ConnectionState.Closed)
                db.Open();

            db.Query<FeedbackModel>($"INSERT INTO FeedbackModel (School) VALUES (@School)",
                new
                {
                    School = item.School
                }, commandType: CommandType.Text);
            
            return Task.CompletedTask;
        }

        public Task DeleteSchool(FeedbackModel item)
        {
            using IDbConnection db = new SqliteConnection(Startup.Connection);
            if (db.State == ConnectionState.Closed)
                db.Open();
            db.Query<FeedbackModel>($"DELETE FROM FeedbackModel where Id =@Id", new { Id = item.Id }, commandType: CommandType.Text);
            
            return Task.CompletedTask;
        }

        public Task AddClass(FeedbackModel item)
        {
            using IDbConnection db = new SqliteConnection(Startup.Connection);
            if (db.State == ConnectionState.Closed)
                db.Open();

            db.Query<FeedbackModel>($"INSERT INTO FeedbackModel (Class) VALUES (@Class)",
                new
                {
                    Class = item.Class
                }, commandType: CommandType.Text);
            
            return Task.CompletedTask;
        }

        public Task DeleteClass(FeedbackModel item)
        {
            using IDbConnection db = new SqliteConnection(Startup.Connection);
            if (db.State == ConnectionState.Closed)
                db.Open();
            db.Query<FeedbackModel>($"DELETE FROM FeedbackModel where Id =@Id", new { Id = item.Id }, commandType: CommandType.Text);
            
            return Task.CompletedTask;
        }
    }
}