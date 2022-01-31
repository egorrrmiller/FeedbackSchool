using System.Diagnostics;
using System.Threading.Tasks;
using FeedbackSchool.Data;
using FeedbackSchool.Data.EntityFramework;
using FeedbackSchool.Models;
using Microsoft.AspNetCore.Mvc;

namespace FeedbackSchool.Controllers;

public class HomeController : Controller
{
    private readonly IRepository<FeedbackList, FeedbackModel> _repository;

    public HomeController()
    {
        _repository = new EfRepository();
    }

    [HttpPost]
    public async Task<IActionResult> Index(FeedbackList item)
    {
        if (ModelState.IsValid)
            await _repository.AddFeedback(item);
        else
            return View();

        return View("Okay", item);
    }

    [HttpGet]
    public IActionResult Index()
        => View();

    [HttpGet]
    public IActionResult Feedback()
        => View(_repository);

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
        => View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
}