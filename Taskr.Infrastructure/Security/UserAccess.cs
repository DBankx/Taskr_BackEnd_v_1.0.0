using System.Linq;
using Microsoft.AspNetCore.Http;
using Taskr.Domain;

namespace Taskr.Infrastructure.Security
{
    public class UserAccess : IUserAccess
    {
        private readonly IHttpContextAccessor _httpContext;

        public UserAccess(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }
        
        public string GetCurrentUserId()
        {
            return _httpContext.HttpContext.User?.Claims.Single(x => x.Type == "id")?.Value;
        }
    }
}