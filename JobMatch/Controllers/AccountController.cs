using JobMatch.Extensions;
using JobMatch.Models;
using JobMatch.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace JobMatch.Controllers
{
    public class AccountController : Controller
    {
        private readonly JobMatchContext _dbContext;
        private readonly UserService _userService;

        public AccountController(JobMatchContext dbContext, UserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            string hashedPassword = HashMD5(password);

            var userDB = _dbContext.Users.FirstOrDefault(u => u.Email == email && u.UserPassword == hashedPassword);
            if (userDB != null)
            {
                var user = new UserModel
                {
                    Id = userDB.Id,
                    UserName = userDB.UserName,
                    Email = userDB.Email,
                    UserAvatar = userDB.UserAvatar,
                    Phone = userDB.Phone,
                    UserType = userDB.UserType
                };

                _userService.SetUser(user); 
                if(user.UserType == "recruiter")
                {
					return RedirectToAction("Index", "Recruiter");
				}
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string userName, string email, string password, string phone, string userType)
        {
            if (_dbContext.Users.Any(u => u.Email == email))
            {
                ViewBag.Error = "Email này đã tồn tại!";
                return View();
            }

            string hashedPassword = HashMD5(password);

            var newUser = new User
            {
                UserName = userName,
                Email = email,
                UserAvatar = "~/img/uploads/default-avatar.jpg",
                UserPassword = hashedPassword,
                Phone = phone,
                UserType = userType
            };

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            _userService.ClearUser(); // Xóa user khỏi session và service
            return RedirectToAction("Index", "Home");
        }

        private string HashMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}
