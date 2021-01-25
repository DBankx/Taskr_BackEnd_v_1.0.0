using MediatR;
using Taskr.Dtos.Profile;

namespace Taskr.Queries.Profile
{
    public class GetProfileQuery : IRequest<ProfileDto>
    {
        public GetProfileQuery()
        {
            
        }
    }
}