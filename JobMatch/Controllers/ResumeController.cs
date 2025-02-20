using JobMatch.Models;
using JobMatch.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace JobMatch.Controllers
{
    public class ResumeController : Controller
    {
        private readonly JobMatchContext _dbContext;
        private readonly ResumeService _resumeService;

        
        public ResumeController(JobMatchContext dbContext, ResumeService resumeService)
        {
            _dbContext = dbContext;
            _resumeService = resumeService;

        }

        public IActionResult Index(int userID)
        {
            
            var resumes = _dbContext.Resumes.Where(r => r.CandidateId == userID).ToList();

           
            return View(resumes);
        }

        [HttpPost]
        public IActionResult UploadResume(IFormFile resumeFile, int userId)
        {
            Resume? resumeUploaded = _resumeService.UploadResume(resumeFile, userId);
            return RedirectToAction("Index", new { userID = userId });
        }
        public IActionResult ViewResume(int id)
        {
            if (id <= 0)
            {
                return NotFound("ID hồ sơ không hợp lệ.");
            }

            var resume = _dbContext.Resumes.FirstOrDefault(r => r.Id == id);

            if (resume == null)
            {
                return NotFound("Không tìm thấy hồ sơ.");
            }

            if (resume.ResumeFile == null || resume.ResumeFile.Length == 0)
            {
                return NotFound("Hồ sơ không có file PDF.");
            }

            return File(resume.ResumeFile, "application/pdf");
        }
        [HttpPost]
        public IActionResult DeleteResume(int id)
        {
            var resume = _dbContext.Resumes.Find(id);
            if (resume == null)
            {
                return NotFound();
            }

            _dbContext.Resumes.Remove(resume);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }



        public IActionResult DownloadResume(int id)
        {
            var resumeData = _resumeService.DownloadResume(id);
            if (resumeData.HasValue)
            {
                return File(resumeData.Value.fileData, resumeData.Value.contentType, resumeData.Value.fileName);
            }
            return NotFound();
        }
    }
}
