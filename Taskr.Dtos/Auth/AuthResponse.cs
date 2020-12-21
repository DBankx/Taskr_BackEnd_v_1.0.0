using System.Collections.Generic;

namespace Taskr.Dtos.Auth
{
    /// <summary>
    /// Response for every auth request
    /// </summary>
    public class AuthResponse
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public AuthUserResponse User { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}