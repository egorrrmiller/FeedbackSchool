using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using FeedbackSchool.Data;
using FeedbackSchool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}