
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using JobMatch.Models;
using Microsoft.EntityFrameworkCore;

namespace JobMatch.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly JobMatchContext _dbContext;

        public HomeController(ILogger<HomeController> logger, JobMatchContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            //job list
            var jobCategories = _dbContext.JobCategories.ToList();            
            ViewBag.JobCategories = jobCategories;

            // job list
            var jobs = _dbContext.Jobs.Include(j => j.Company).ToList();
            ViewBag.Jobs = jobs;

            //full time job
            var fulltimeJobs = _dbContext.Jobs.Include(j => j.Company).Where(j => j.JobType == "fulltime").ToList();
            ViewBag.FulltimeJobs = fulltimeJobs;

            //part time job
            var parttimeJobs = _dbContext.Jobs.Include(j => j.Company).Where(j => j.JobType == "parttime").ToList();

            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
