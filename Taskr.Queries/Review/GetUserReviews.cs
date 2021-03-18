using System.Collections.Generic;
using MediatR;
using Taskr.Dtos.Profile;
using Taskr.Dtos.Review;

namespace Taskr.Queries.Review
{
    public class GetUserReview : IRequest<List<ReviewDto>>
    {
        public string UserId { get; set; }
        
        public string Predicate { get; set; }

        public GetUserReview(string userId, string predicate)
        {
            UserId = userId;
            Predicate = predicate;
        }
    }
}