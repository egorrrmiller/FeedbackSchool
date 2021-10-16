using System;
using System.Linq;
using System.Threading.Tasks;
using FeedbackSchool.Areas.Identity.Data;
using FeedbackSchool.Data;
using FeedbackSchool.Data.EntityFramework;
using FeedbackSchool.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FeedbackSchool.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly IRepository<Guest, FeedbackModel> _repository;
        private readonly RedirectToActionResult _redirectToAction;
        private readonly UserManager<FeedbackSchoolUser> _userManager;
        private readonly ILogger<ManageController> _logger;

        public ManageController(UserManager<FeedbackSchoolUser> userManager, ILogger<ManageController> logger)
        {
            _repository = new EfRepository();

            _redirectToAction = RedirectToAction(nameof(Index));
            _userManager = userManager;
            _logger = logger;
        }

        // GET
        [HttpGet]
        public IActionResult Index()
            => View(_repository);

        [HttpPost]
        public async Task<IActionResult> Index(string feedbacks)
        {
            // потом мб придумаю что-то лучше

            ViewResult view;

            var count = 0;
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var feedbackNumber in feedbacks.Split(','))
                    {
                        await _repository.DeleteFeedback(int.Parse(feedbackNumber));
                        count++;
                    }

                    view = View(_repository);
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

            _logger.LogInformation($"Пользователь {_userManager.GetUserName(User)} удалил {count} отзыв(-ов)");

            return view;
        }

        public async Task<ActionResult> DownloadDb()
        {
            await System.IO.File.WriteAllTextAsync("DataBase.json",
                JsonConvert.SerializeObject(_repository.GetAllList()));
            Response.Headers.Add("Content-Disposition", "attachment; filename=DataBase.json");

            _logger.LogInformation($"Пользователь {_userManager.GetUserName(User)} скачал базу даных");

            return new FileContentResult(await System.IO.File.ReadAllBytesAsync("DataBase.json"), "application/json");
        }

        #region Trash.... Выглядит как говно если честно...

        public async Task<IActionResult> DeleteAllFeedback()
        {
            await _repository.DeleteAllFeedback();

            _logger.LogInformation($"Пользователь {_userManager.GetUserName(User)} удалил все отзывы");

            return _redirectToAction;
        }

        public async Task<IActionResult> AddSchool(string addSchool)
        {
            await _repository.AddSchool(new FeedbackModel() {School = addSchool});

            _logger.LogInformation($"Пользователь {_userManager.GetUserName(User)} добавил школу {addSchool}");

            return _redirectToAction;
        }

        public async Task<IActionResult> DeleteSchool(string id)
        {
            var school = _repository.GetSchoolClass().FirstOrDefault(f => f.Id == id)?.School;

            _logger.LogInformation($"Пользователь {_userManager.GetUserName(User)} удалил школу {school}");
            
            
            await _repository.DeleteSchool(new FeedbackModel() {Id = id});

            return _redirectToAction;
        }

        public async Task<IActionResult> AddClass(string addClass)
        {
            await _repository.AddClass(new FeedbackModel() {Class = addClass});

            _logger.LogInformation($"Пользователь {_userManager.GetUserName(User)} добавил {addClass} класс ");

            return _redirectToAction;
        }

        public async Task<IActionResult> DeleteClass(string id)
        {
            var classes = _repository.GetSchoolClass().FirstOrDefault(f => f.Id == id)?.Class;

            _logger.LogInformation($"Пользователь {_userManager.GetUserName(User)} удалил {classes} класс");

            
            await _repository.DeleteClass(new FeedbackModel() {Id = id});
            
            return _redirectToAction;
        }

        #endregion
    }
}