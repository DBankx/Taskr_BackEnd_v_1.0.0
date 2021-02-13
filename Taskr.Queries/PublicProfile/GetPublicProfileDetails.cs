using MediatR;
using Taskr.Dtos.Profile;

namespace Taskr.Queries.PublicProfile
{
    public class GetPublicProfileDetails : IRequest<PublicProfileDto>
    {
        public string UserId { get; set; }

        public GetPublicProfileDetails(string userId)
        {
            UserId = userId;
        }
    }
}