using System.Linq;
using Microsoft.AspNetCore.Http;
using Taskr.Domain;

namespace Taskr.Infrastructure.Security
{
    public class UserAccess : IUserAccess
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAccess(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        public string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.Claims?.SingleOrDefault(x => x.Type == "id")?.Value;
        }
    }
}