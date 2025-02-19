using JobMatch.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobMatch.Controllers
{
    public class JobController : Controller
    {
        private readonly JobMatchContext _dbContext;

        // Inject DbContext vào constructor
        public JobController(JobMatchContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult JobDetail(int jobID)
        {
            // Lấy thông tin Job từ database
            var job = _dbContext.Jobs
                .Include(j => j.Company) // Load thông tin công ty (nếu có quan hệ)
                .FirstOrDefault(j => j.Id == jobID);

            if (job == null)
            {
                return NotFound();
            }

            return View(job); 
        }
    }
}
