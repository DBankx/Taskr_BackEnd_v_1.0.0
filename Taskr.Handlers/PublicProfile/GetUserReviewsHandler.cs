using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Domain;
using Taskr.Dtos.Errors;
using Taskr.Dtos.Review;
using Taskr.Infrastructure.Helpers;
using Taskr.Infrastructure.Security;
using Taskr.Queries.Review;

namespace Taskr.Handlers.PublicProfile
{
    public class GetUserReviewsHandler : IRequestHandler<GetUserReview, List<ReviewDto>>
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IMapper _mapper;

        public GetUserReviewsHandler(IQueryProcessor queryProcessor, IMapper mapper)
        {
            _queryProcessor = queryProcessor;
            _mapper = mapper;
        }
        public async Task<List<ReviewDto>> Handle(GetUserReview request, CancellationToken cancellationToken)
        {
            var user = await _queryProcessor.Query<ApplicationUser>().SingleOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

            if (user == null) throw new RestException(HttpStatusCode.NotFound, new {error = "User not found"});

            List<Domain.Review> reviews = new List<Domain.Review>();

            switch (request.Predicate)
            {
                case "Taskr":
                    reviews = await _queryProcessor.Query<Domain.Review>().Include(x => x.Order).ThenInclude(x => x.Job).Include(x => x.Reviewer)
                        .Where(x => x.Reviewee == user && x.Type == ReviewType.Taskr).ToListAsync(cancellationToken);
                    break;
                case "Runner":
                      reviews = await _queryProcessor.Query<Domain.Review>()
                                            .Include(x => x.Order).ThenInclude(x => x.Job).Include(x => x.Reviewer).Where(x => x.Reviewee == user && x.Type == ReviewType.Runner).ToListAsync(cancellationToken);
                      break;
                default:
                    break;
            }

            return _mapper.Map<List<ReviewDto>>(reviews);
        }
    }
}