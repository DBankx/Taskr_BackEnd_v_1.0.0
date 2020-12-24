using Taskr.Domain;

namespace Taskr.Infrastructure.Security
{
    public interface IUserAccess
    {
        string GetCurrentUserId();
    }
}