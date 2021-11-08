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
using Newtonsoft.Json;
using Serilog;

namespace FeedbackSchool.Controllers
{
    [Authorize]
    public sealed class ManageController : Controller
    {
        private readonly IRepository<Guest, FeedbackModel> _repository;
        private readonly RedirectToActionResult _redirectToAction;
        private readonly UserManager<FeedbackSchoolUser> _userManager;
        private readonly ILogger _logger;

        public ManageController(UserManager<FeedbackSchoolUser> userManager, ILogger logger)
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
                }
                catch (FormatException e)
                {
                    ModelState.AddModelError("FormatException", "Проверьте правильность введенных данных!");
                    ModelState.AddModelError("FormatException", $"До ошибки было удалено {count} отзыв(-ов)");
                    return View(_repository);
                }
                finally
                {
                    _logger.Information("Пользователь {UserName} удалил {Count} отзыв(-ов)", _userManager.GetUserName(User), count);
                }
            }

            return View(_repository);
        }

        public async Task<ActionResult> DownloadDb()
        {
            await System.IO.File.WriteAllTextAsync("DataBase.json",
                JsonConvert.SerializeObject(_repository.GetAllList()));
            Response.Headers.Add("Content-Disposition", "attachment; filename=DataBase.json");

            _logger.Information("Пользователь {UserName} скачал базу даных", _userManager.GetUserName(User));

            return new FileContentResult(await System.IO.File.ReadAllBytesAsync("DataBase.json"), "application/json");
        }

        #region Trash.... Выглядит как говно если честно...

        public async Task<IActionResult> DeleteAllFeedback()
        {
            await _repository.DeleteAllFeedback();

            _logger.Information("Пользователь {UserName} удалил все отзывы", _userManager.GetUserName(User));

            return _redirectToAction;
        }

        public async Task<IActionResult> AddSchool(string addSchool)
        {
            await _repository.AddSchool(new FeedbackModel() {School = addSchool});

            _logger.Information("Пользователь {UserName} добавил школу {School}", _userManager.GetUserName(User), addSchool);

            return _redirectToAction;
        }

        public async Task<IActionResult> AddClass(string addClass)
        {
            await _repository.AddClass(new FeedbackModel() {Class = addClass});

            _logger.Information("Пользователь {UserName} добавил {Class} класс", _userManager.GetUserName(User), addClass);

            return _redirectToAction;
        }

        public async Task<IActionResult> DeleteSchoolOrClass(string id)
        {
            var school = _repository.GetSchoolClass().FirstOrDefault(f => f.Id == id)?.School;
            var classes = _repository.GetSchoolClass().FirstOrDefault(f => f.Id == id)?.Class;

            if (school != null)
                _logger.Information("Пользователь {UserName} удалил школу {School}", _userManager.GetUserName(User), school);
            else
                _logger.Information("Пользователь {UserName} удалил {Class} класс", _userManager.GetUserName(User), classes);

            await _repository.DeleteSchoolOrClass(new FeedbackModel() {Id = id});

            return _redirectToAction;
        }

        #endregion
    }
}