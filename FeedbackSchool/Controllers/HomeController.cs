using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using FeedbackSchool.Areas.Identity.Data;
using FeedbackSchool.Data;
using FeedbackSchool.Models.ErrorViewModels;
using FeedbackSchool.Models.FeedbackViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> Index(FeedbackModel item)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        item.DateTime = DateTime.Now.ToString(CultureInfo.CurrentCulture);

        _applicationContext.Feedback.Add(item);
        await _applicationContext.SaveChangesAsync();

        return View("Okay", item);
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<ViewResult> Feedback()
    {
        return View(await _applicationContext.Feedback.ToListAsync());
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