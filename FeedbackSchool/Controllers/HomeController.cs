using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using FeedbackSchool.Data.EntityFramework;
using FeedbackSchool.Models;
using Microsoft.AspNetCore.Mvc;

namespace FeedbackSchool.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationContext _applicationContext;

    public HomeController(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    [HttpPost]
    public async Task<IActionResult> Index(FeedbackModel item)
    {
        if (ModelState.IsValid)
        {
            _applicationContext.Feedback.Add(new FeedbackModel()
            {
                School = item.School,
                Class = item.Class,
                Name = item.Name,
                Feedback = item.Feedback,
                FavoriteLessons = item.FavoriteLessons ?? string.Empty,
                DateTime = DateTime.Now.ToString(CultureInfo.CurrentCulture)
            });

            _applicationContext.SaveChanges();
        }
        else
            return View();

        return View("Okay", item);
    }

    [HttpGet]
    public IActionResult Index()
        => View();

    [HttpGet]
    public IActionResult Feedback()
        => View(_applicationContext);

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
        => View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
}