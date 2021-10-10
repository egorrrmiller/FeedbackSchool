using System;
using System.Linq;
using System.Threading.Tasks;
using FeedbackSchool.Data.Dapper;
using FeedbackSchool.Data.EntityFramework;
using FeedbackSchool.Interfaces;
using FeedbackSchool.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FeedbackSchool.Controllers
{
    public class AdminController : Controller
    {
        private readonly IRepository<Guest> _repository;

        public AdminController()
        {
            _repository = new DapperRepository();
        }

        // GET
        [HttpGet]
        public IActionResult Admin()
            => View();


        [HttpPost]
        public async Task<IActionResult> Admin(Admin admin)
        {
            // потом мб придумаю что-то лучше

            if (!ModelState.IsValid)
                return View();

            var count = 0;
            try
            {
                foreach (var feedbackNumber in admin.FeedbackNumber.Split(','))
                {
                    await _repository.Delete(int.Parse(feedbackNumber));
                    count++;
                }
            }
            catch (FormatException e)
            {
                ModelState.AddModelError("FormatException", "Проверьте правильность введенных данных! <br/>" +
                                                            $"До ошибки было удалено {count} отзыв(-ов)");
                return View();
            }

            return Redirect("Admin");
        }


        public async Task<ActionResult> DownloadDb()
        {
            await System.IO.File.WriteAllTextAsync("DataBase.json", JsonConvert.SerializeObject(_repository.GetAllList()));
            Response.Headers.Add("Content-Disposition", "attachment; filename=DataBase.json");
            return new FileContentResult(await System.IO.File.ReadAllBytesAsync("DataBase.json"), "application/json");
        }

        public async Task<IActionResult> DeleteAllFeedback()
        {
            await _repository.DeleteAll();

            return Redirect("Admin");
        }
    }
}