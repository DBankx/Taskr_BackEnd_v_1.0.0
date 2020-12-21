namespace Taskr.Dtos.Auth
{
    /// <summary>
    /// Response of details when user logs in or signs up
    /// @@@ TODO -- ADD AVATAR!
    /// </summary>
    public class AuthUserResponse
    {
        public string Username { get; set; }
        public string Email { get; set; }
    }
}