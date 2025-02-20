using Microsoft.AspNetCore.Mvc;

namespace JobMatch.Controllers
{
    public class NotificationController : Controller
    {
        public IActionResult NotificationRecruiterList()
        {
            return View();
        }
    }
}
