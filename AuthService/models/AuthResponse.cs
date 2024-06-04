using AuthService.Repository.Models;

namespace AuthService.models
{
    public class AuthResponse
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string UserRole { get; set; }

        public AuthResponse(User user, string token)
        {
            UserID = user.UserCD;
            UserName = user.UserName;
            Email = user.Email;
            UserRole = user.UserRole;
            Token = token;
        }
    }
}
