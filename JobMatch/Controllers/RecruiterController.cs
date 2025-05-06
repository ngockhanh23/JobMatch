using JobMatch.Models;
using JobMatch.Services;
using JobMatch.Utils.MailUtils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JobMatch.Controllers
{
    public class RecruiterController : Controller
    {
        private readonly JobMatchContext _context;
        private readonly UserService _userService;

        public RecruiterController(JobMatchContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> JobList()
        {
            int userId = _userService.GetUser().Id;
            var jobs = await _context.Jobs
                                        .Include(j => j.Company)
                                        .Include(j => j.Applications).Where(j => j.Company.UserId == userId).ToListAsync();
            return View(jobs);
        }


        public IActionResult Create()
        {
            ViewBag.Companies = _context.Companies.ToList(); // Lấy danh sách công ty
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Job job)
        {
            if (ModelState.IsValid)
            {
                job.UploadDate = DateTime.Now;
                _context.Jobs.Add(job);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(JobList));
            }
            ViewBag.Companies = _context.Companies.ToList();
            return View(job);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var job = await _context.Jobs.FindAsync(id);
            if (job == null) return NotFound();

            ViewBag.Companies = _context.Companies.ToList();
            return View(job);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Job job)
        {
            if (id != job.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(job);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Companies = _context.Companies.ToList();
            return View(job);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var job = await _context.Jobs
                                    .FirstOrDefaultAsync(j => j.Id == id);

            if (job == null) return NotFound();

            // Xóa tất cả đơn ứng tuyển liên quan trước khi xóa công việc
            _context.Applications.RemoveRange(job.Applications);

            // Xóa công việc
            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(JobList));
        }
        public async Task<IActionResult> JobApplications(int jobId)
        {
            var job = await _context.Jobs
                                    .Include(j => j.Applications)
                                        .ThenInclude(a => a.Resume) // Nếu cần hiển thị thông tin ứng viên
                                    .FirstOrDefaultAsync(j => j.Id == jobId);

            if (job == null) return NotFound();

            return View(job);
        }

        public async Task<IActionResult> ViewApplication(int applicationId)
        {
            var application = await _context.Applications
                .Include(a => a.Resume)
                .Include(a => a.Job)
                    .ThenInclude(j => j.Company)
                .FirstOrDefaultAsync(a => a.Id == applicationId);

            if (application == null || application.Resume == null || application.Resume.ResumeFile == null)
            {
                return NotFound("Không tìm thấy CV");
            }

            // Trả về file PDF dưới dạng base64 để hiển thị trực tiếp
            string base64String = Convert.ToBase64String(application.Resume.ResumeFile);
            string pdfDataUrl = $"data:application/pdf;base64,{base64String}";

            ViewBag.PdfUrl = pdfDataUrl;
            return View(application);
        }



        [HttpPost]
        public async Task<IActionResult> UpdateApplicationStatus(int applicationId, string status, string jobTitle, string companyName)
        {
            var application = await _context.Applications
                    .Include(a => a.Job)
                        .ThenInclude(j => j.Company)
                            .ThenInclude(c => c.User)
                    .Include(a => a.Resume)
                        .ThenInclude(r => r.Candidate) // <- để lấy user ứng viên
                    .FirstOrDefaultAsync(a => a.Id == applicationId);


            if (application == null)
            {
                return NotFound();
            }

            application.ApplicationStatus = status;
            await _context.SaveChangesAsync();


            if (status == "Rejected")
            {
                // Lấy email người nộp (user) 
                var userEmail = application.Resume?.Candidate?.Email;
                if (string.IsNullOrEmpty(userEmail))
                {
                    // Xử lý lỗi hoặc ghi log
                    Console.WriteLine("Không tìm thấy email của ứng viên.");
                    return BadRequest("Email không hợp lệ.");
                }


                Console.WriteLine("Candidate Email: " + userEmail);
                Console.WriteLine("Company Name: " + companyName);

                // gửi email từ công ty đến ứng viên:
                await MailUtils.SendMail(
                    from: "ngockhanh23032003@gmail.com",
                    to: userEmail ?? "",
                    subject: $"Kết quả đơn ứng tuyển vị trí '{jobTitle}' tại {companyName}",
                    body: MailUtils.GetRejectionEmailBody(jobTitle, companyName)
                );
            }

            return RedirectToAction("ViewCV", new { applicationId });
        }



    }
}
