using JobMatch.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JobMatch.Services;

namespace JobMatch.Controllers
{
    public class CompanyController : Controller
    {
        private readonly JobMatchContext _context;
        private readonly UserService _userService;


        public CompanyController(JobMatchContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // Danh sách công ty
        public async Task<IActionResult> Index()
        {
            var companies = await _context.Companies.Where(c => c.UserId == _userService.GetUser().Id).ToListAsync();
            return View(companies);
        }

        // Hiển thị form thêm công ty
        public IActionResult Create()
        {
            return View();
        }

        // Xử lý thêm công ty
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Company company, IFormFile? CompanyImageFile)
        {
            if (ModelState.IsValid)
            {
                if (CompanyImageFile != null)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/uploads");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + CompanyImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await CompanyImageFile.CopyToAsync(fileStream);
                    }

                    company.CompanyImage = "~/img/uploads/" + uniqueFileName;
                }

                company.CreatedAt = DateTime.Now;
                _context.Companies.Add(company);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(company);
        }

        // Hiển thị form chỉnh sửa công ty
        public async Task<IActionResult> Edit(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // Xử lý cập nhật công ty
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Company company, IFormFile? CompanyImageFile)
        {
            if (id != company.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (CompanyImageFile != null)
                    {
                        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/uploads");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + CompanyImageFile.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await CompanyImageFile.CopyToAsync(fileStream);
                        }

                        company.CompanyImage = "~/img/uploads/" + uniqueFileName;
                    }
                    else
                    {
                        // Không có ảnh mới, giữ nguyên ảnh cũ
                        company.CompanyImage = Request.Form["ExistingCompanyImage"];
                    }

                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Companies.Any(e => e.Id == company.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // Xóa công ty
        public async Task<IActionResult> Delete(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
