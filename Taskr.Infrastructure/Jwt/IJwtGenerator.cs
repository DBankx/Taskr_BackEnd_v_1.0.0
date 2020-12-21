using Taskr.Domain;

namespace Taskr.Infrastructure.Jwt
{
    public interface IJwtGenerator
    {
        string GenerateToken(ApplicationUser user);
    }
}