﻿using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeedbackSchool.Areas.Identity.Data;
using FeedbackSchool.Data;
using FeedbackSchool.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace FeedbackSchool.Controllers;

[Authorize]
public sealed class ManageController : Controller
{
    private readonly ApplicationContext _applicationContext;

    private readonly ILogger _logger;

    private readonly RedirectToActionResult _redirectToAction;

    private readonly UserManager<FeedbackSchoolUser> _userManager;

    public ManageController(UserManager<FeedbackSchoolUser> userManager, ILogger logger,
                            ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
        _logger = logger;
        _redirectToAction = RedirectToAction(nameof(Index));
        _userManager = userManager;
    }

    // GET
    [HttpGet]
    public IActionResult Index()
    {
        return View(_applicationContext.Manage.ToList());
    }

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
                    _applicationContext.Feedback.Remove(new FeedbackModel
                    {
                        Id = int.Parse(feedbackNumber)
                    });

                    await _applicationContext.SaveChangesAsync();
                    count++;
                }
            }
            catch (FormatException)
            {
                ModelState.AddModelError("FormatException", "Проверьте правильность введенных данных!");
                ModelState.AddModelError("FormatException", $"До ошибки было удалено {count} отзыв(-ов)");

                return View(_applicationContext.Manage.ToList());
            }
            finally
            {
                _logger.Information("Пользователь {UserName} удалил {Count} отзыв(-ов)",
                    _userManager.GetUserName(User),
                    count);
            }
        }

        return View(_applicationContext.Manage.ToList());
    }

    [Authorize]
    [HttpGet("/downloaddb")]
    public Task<ActionResult> DownloadDb()
    {
        _logger.Information("Пользователь {UserName} скачал базу даных", _userManager.GetUserName(User));

        return Task.FromResult<ActionResult>(File(
            Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_applicationContext.Feedback.ToList())),
            "application/json",
            "DataBase.json"));
    }

#region Trash.... Выглядит как говно если честно...

    public async Task<IActionResult> DeleteAllFeedbacks()
    {
        foreach (var guest in _applicationContext.Feedback)
        {
            _applicationContext.Feedback.Remove(new FeedbackModel
            {
                Id = guest.Id
            });
        }

        await _applicationContext.SaveChangesAsync();

        _logger.Information("Пользователь {UserName} удалил все отзывы", _userManager.GetUserName(User));

        return _redirectToAction;
    }

    public async Task<IActionResult> AddSchool(string addSchool)
    {
        _applicationContext.Manage.Add(new ManageModel
        {
            School = addSchool
        });

        await _applicationContext.SaveChangesAsync();

        _logger.Information("Пользователь {UserName} добавил школу {School}",
            _userManager.GetUserName(User),
            addSchool);

        return _redirectToAction;
    }

    public async Task<IActionResult> AddClass(string addClass)
    {
        _applicationContext.Manage.Add(new ManageModel
        {
            Class = addClass
        });

        await _applicationContext.SaveChangesAsync();

        _logger.Information("Пользователь {UserName} добавил {Class} класс", _userManager.GetUserName(User), addClass);

        return _redirectToAction;
    }

    public async Task<IActionResult> DeleteSchoolOrClass(string id)
    {
        var school = _applicationContext.Manage.FirstOrDefault(f => f.Id == id)?.School;
        var classes = _applicationContext.Manage.FirstOrDefault(f => f.Id == id)?.Class;

        if (school != null)
        {
            _logger.Information("Пользователь {UserName} удалил школу {School}",
                _userManager.GetUserName(User),
                school);
        } else
        {
            _logger.Information("Пользователь {UserName} удалил {Class} класс",
                _userManager.GetUserName(User),
                classes);
        }

        _applicationContext.Manage.Remove(new ManageModel
        {
            Id = id
        });

        await _applicationContext.SaveChangesAsync();

        return _redirectToAction;
    }

#endregion
}