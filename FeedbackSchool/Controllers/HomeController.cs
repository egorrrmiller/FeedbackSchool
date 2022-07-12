using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FeedbackSchool.Areas.Identity.Data;
using FeedbackSchool.Data;
using FeedbackSchool.Models;
using FeedbackSchool.ViewModels.ErrorViewModels;
using FeedbackSchool.ViewModels.IndexViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FeedbackSchool.Controllers;

public sealed class HomeController : Controller
{
    private readonly ApplicationContext _applicationContext;

    private readonly ILogger _logger;

    private readonly RedirectToActionResult _redirectToActionFeedback;

    private readonly UserManager<FeedbackSchoolUser> _userManager;

    public HomeController(ApplicationContext applicationContext, UserManager<FeedbackSchoolUser> userManager, ILogger logger)
    {
        _applicationContext = applicationContext;
        _userManager = userManager;
        _logger = logger;
        _redirectToActionFeedback = RedirectToAction(nameof(Feedback));
    }

    [HttpPost]
    public async Task<IActionResult> Index(IndexViewModel item)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        item.Feedback.DateTime = DateTime.Now.ToString(CultureInfo.CurrentCulture);

        _applicationContext.Feedback.Add(item.Feedback);
        await _applicationContext.SaveChangesAsync();

        return View("Okay", item.Feedback);
    }

    [HttpGet]
    public IActionResult Index()
    {

        var model = new IndexViewModel()
        {
            ClassListItems = _applicationContext.Class.Select(classModel => new SelectListItem()
            {
                Text = classModel.Class,
                Value = classModel.Class,
            }).ToList(),

            SchoolListItems = _applicationContext.School.Select(schoolModel => new SelectListItem()
            {
                Text = schoolModel.School,
                Value = schoolModel.School,
            }).ToList(),
        };
        return View(model);
    }

    [HttpGet]
    public async Task<ViewResult> Feedback()
    {
        var model = new FeedbackViewModel()
        {
            Feedback = await _applicationContext.Feedback.ToListAsync()
        };

        return View(model);
    }

    [HttpGet("feedback/delete/{feedbackId:int}")]
    public async Task<IActionResult> DeleteFeedback(int feedbackId)
    {
        var feedback = await _applicationContext.Feedback.FindAsync(feedbackId);

        if (feedback == null)
        {
            return _redirectToActionFeedback;
        }

        _applicationContext.Feedback.Remove(feedback);

        await _applicationContext.SaveChangesAsync();

        _logger.Information("Пользователь {UserName} удалил отзыв с id {feedbackId}",
            _userManager.GetUserName(User),
            feedbackId);

        return _redirectToActionFeedback;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}