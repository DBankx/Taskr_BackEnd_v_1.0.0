namespace Taskr.Dtos.Auth
{
    /// <summary>
    /// Response of details when user logs in or signs up
    /// </summary>
    public class AuthUserResponse
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public bool HasUnReadNotifications { get; set; }
    }
}