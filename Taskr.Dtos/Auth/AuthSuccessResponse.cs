namespace Taskr.Dtos.Auth
{
    public class AuthSuccessResponse
    {
        public string Token { get; set; }
        public AuthUserResponse User { get; set; } 
    }
}