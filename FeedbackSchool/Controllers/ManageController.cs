using System;
using System.Threading.Tasks;
using FeedbackSchool.Data;
using FeedbackSchool.Data.EntityFramework;
using FeedbackSchool.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FeedbackSchool.Controllers
{
    public class ManageController : Controller
    {
        private readonly IRepository<Guest, FeedbackModel> _repository;
        private readonly RedirectToActionResult _redirectToAction;

        public ManageController()
        {
            _repository = new EfRepository();
            
            _redirectToAction = RedirectToAction(nameof(Index));
        }

        // GET
        [HttpGet]
        public IActionResult Index()
            => View(_repository);

        [HttpPost]
        public async Task<IActionResult> Index(string feedbacks)
        {
            // потом мб придумаю что-то лучше
            
            ViewResult view = null;

            if (ModelState.IsValid)
            {
                var count = 0;
                try
                {
                    foreach (var feedbackNumber in feedbacks.Split(','))
                    {
                        await _repository.DeleteFeedback(int.Parse(feedbackNumber));
                        count++;
                    }
                }
                catch (FormatException e)
                {
                    ModelState.AddModelError("FormatException", "Проверьте правильность введенных данных!");
                    ModelState.AddModelError("FormatException", $"До ошибки было удалено {count} отзыв(-ов)");
                    view = View(_repository);
                }
            }
            else
                view = View(_repository);

            return view;
        }

        public async Task<ActionResult> DownloadDb()
        {
            await System.IO.File.WriteAllTextAsync("DataBase.json",
                JsonConvert.SerializeObject(_repository.GetAllList()));
            Response.Headers.Add("Content-Disposition", "attachment; filename=DataBase.json");
            return new FileContentResult(await System.IO.File.ReadAllBytesAsync("DataBase.json"), "application/json");
        }

        #region Trash.... Выглядит как говно если честно...

        public async Task<IActionResult> DeleteAllFeedback()
        {
            await _repository.DeleteAllFeedback();

            return _redirectToAction;
        }

        public async Task<IActionResult> AddSchool(string addSchool)
        {
            await _repository.AddSchool(new FeedbackModel() {School = addSchool});

            return _redirectToAction;
        }

        public async Task<IActionResult> DeleteSchool(string id)
        {
            await _repository.DeleteSchool(new FeedbackModel() {Id = id});

            return _redirectToAction;
        }

        public async Task<IActionResult> AddClass(string addClass)
        {
            await _repository.AddClass(new FeedbackModel() {Class = addClass});

            return _redirectToAction;
        }

        public async Task<IActionResult> DeleteClass(string id)
        {
            await _repository.DeleteClass(new FeedbackModel() {Id = id});

            return _redirectToAction;
        }

        #endregion
    }
}