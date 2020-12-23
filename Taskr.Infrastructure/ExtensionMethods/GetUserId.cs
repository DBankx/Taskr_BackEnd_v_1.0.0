using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Taskr.Infrastructure.ExtensionMethods
{
    public static class GetUserId
    {
        public static string GetCurrentUserId(this HttpContext context)
        {
           return context.User.Claims.Single(x => x.Type == "id").Value; 
        }
    }
}