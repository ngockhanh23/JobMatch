using Microsoft.AspNetCore.Mvc;

namespace JobMatch.Controllers
{
    public class AplicationUserController : Controller
    {
        public IActionResult AplicationList(int idCompany)
        {
            return View();
        }
    }
}
