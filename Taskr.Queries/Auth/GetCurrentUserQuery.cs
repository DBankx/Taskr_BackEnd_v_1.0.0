using MediatR;
using Taskr.Dtos.Auth;

namespace Taskr.Queries.Auth
{
    public class GetCurrentUserQuery : IRequest<AuthResponse>
    {
        
    }
}