using System.Threading.Tasks;
using Taskr.Domain;
using Taskr.Dtos.Auth;

namespace Taskr.RepositoryServices.AuthService
{
    public interface IAuthService
    {
        Task<AuthResponse> SignInAsync(string email, string password);
        Task<AuthResponse> SignUpAsync(ApplicationUser user, string password);
    }
}