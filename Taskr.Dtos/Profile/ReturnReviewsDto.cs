using System.Collections.Generic;
using Taskr.Dtos.Review;

namespace Taskr.Dtos.Profile
{
    public class ReturnReviewsDto
    {
        public double AverageRating { get; set; }
        public int ReviewsCount { get; set; }
        public List<ReviewDto> Reviews{ get; set; }
    }
}