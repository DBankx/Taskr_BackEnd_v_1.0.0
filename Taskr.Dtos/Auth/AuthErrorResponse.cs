using System.Collections.Generic;

namespace Taskr.Dtos.Auth
{
    public class AuthErrorResponse
    {
        public IEnumerable<string> Errors { get; set; } 
    }
}