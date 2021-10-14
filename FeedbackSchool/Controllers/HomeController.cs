using System.Diagnostics;
using System.Threading.Tasks;
using FeedbackSchool.Data;
using FeedbackSchool.Data.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FeedbackSchool.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedbackSchool.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IRepository<Guest, FeedbackModel> _repository;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _repository = new EfRepository();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Guest item)
        {
            if (!ModelState.IsValid)
                return View();
            
            await _repository.AddFeedback(item);
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
}