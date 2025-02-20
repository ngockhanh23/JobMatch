namespace JobMatch.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string UserAvatar { get; set; }
        public string Phone { get; set; }
        public string? UserType { get; set; }
    }
}
