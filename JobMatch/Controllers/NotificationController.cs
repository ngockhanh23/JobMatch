using JobMatch.Models;
using JobMatch.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JobMatch.Controllers
{
    public class NotificationController : Controller
    {
        private readonly JobMatchContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly UserService _userService;

        public NotificationController(JobMatchContext context, IHubContext<NotificationHub> hubContext, UserService userService)
        {
            _context = context;
            _hubContext = hubContext;
            _userService = userService;
        }

        public async Task<IActionResult> NotificationRecruiterList()
        {
            int userId = _userService.GetUser().Id; 
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
            return View(notifications);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();

                // Gửi thông báo real-time đến client
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", "Thông báo đã được đọc");

                return Ok();
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddNotification(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return BadRequest("Nội dung không được để trống");
            }

            var notification = new Notification
            {
                UserId = _userService.GetUser().Id,
                Content = content,
                IsRead = false,
                CreatedAt = DateTime.Now
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            // Gửi thông báo qua SignalR
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", content);

            return RedirectToAction("NotificationRecruiterList");
        }
    }
}
