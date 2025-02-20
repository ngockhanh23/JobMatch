using JobMatch.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobMatch.Services;
using Microsoft.AspNetCore.Builder;


namespace JobMatch.Controllers
{
    public class JobController : Controller
    {
        private readonly JobMatchContext _dbContext;
        private readonly UserService _userService;
		private readonly ResumeService _resumeService;


		// Inject DbContext vào constructor
		public JobController(JobMatchContext dbContext , UserService userService, ResumeService resumeService)
        {
            _dbContext = dbContext;
            _userService = userService;
			_resumeService = resumeService;
        }

        public IActionResult JobDetail(int jobID)
        {
            var user = _userService.GetUser();

            var job = _dbContext.Jobs
                .Include(j => j.Company) 
                .FirstOrDefault(j => j.Id == jobID);

            if (job == null)
            {
                return NotFound();
            }
            
            if(user != null)
            {
				var cvList = _dbContext.Resumes.Where(r => r.CandidateId == user.Id).ToList();
				ViewBag.CVList = cvList;
                var isApplied = _dbContext.Applications.Any(a => a.JobId == jobID && a.Resume.CandidateId == user.Id);

                ViewBag.IsApplied = isApplied;
				
            }
            else 
            {
                
                ViewBag.CVList = null;
                ViewBag.IsApplied = false;

            }

            return View(job); 
        }

		[HttpPost]
		public IActionResult ApplyJob(int jobID, int cvID, string coverLetter, IFormFile resumeFile, string cvSelectionMethod)
		{
			var user = _userService.GetUser();
			int resumeID = cvID;

			if (user == null)
			{
				TempData["ErrorMessage"] = "Bạn cần đăng nhập để ứng tuyển!";
				return RedirectToAction("JobDetail", new { jobID });
			}


			if (cvID == null && resumeFile == null)
			{
				TempData["ErrorMessage"] = "Hãy chọn ít nhất 1 CV";
				return RedirectToAction("JobDetail", new { jobID });
			}

			
			if (cvSelectionMethod == "upload")
			{

				// Gọi ResumeService để upload CV và lấy ID
				Resume resumeUploaded = UploadResume(resumeFile, user.Id);

				if (resumeUploaded == null)
				{
					TempData["ErrorMessage"] = "Lỗi khi tải lên CV. Vui lòng thử lại!";
					return RedirectToAction("JobDetail", new { jobID });
				}
				resumeID = resumeUploaded.Id;

			}

			var application = new Application();
			application.JobId = jobID;
			application.ResumeId = resumeID;
			application.CoverLetter = coverLetter;
			application.ApplicationStatus = "Đã ứng tuyển";
			application.ApplicationDate = DateTime.Now;


			_dbContext.Applications.Add(application);
			_dbContext.SaveChanges();

			TempData["SuccessMessage"] = "Ứng tuyển thành công!";
			return RedirectToAction("JobDetail", new { jobID });
		}

		private Resume UploadResume(IFormFile resumeFile, int userId)
		{
			

			Resume? resumeUploaded = _resumeService.UploadResume(resumeFile, userId);
			return resumeUploaded;
		}

	}
}
