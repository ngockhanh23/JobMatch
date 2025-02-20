using JobMatch.Extensions;
using JobMatch.Models;
using Microsoft.AspNetCore.Http;

namespace JobMatch.Services
{
    public class UserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Lấy user từ session
        public UserModel GetUser()
        {
            return _httpContextAccessor.HttpContext?.Session.GetObject<UserModel>("UserSession");
        }

        // Lưu user vào session
        public void SetUser(UserModel user)
        {
            _httpContextAccessor.HttpContext?.Session.SetObject("UserSession", user);
        }

        // Xóa user khỏi session
        public void ClearUser()
        {
            _httpContextAccessor.HttpContext?.Session.Remove("UserSession");
        }
    }
}
