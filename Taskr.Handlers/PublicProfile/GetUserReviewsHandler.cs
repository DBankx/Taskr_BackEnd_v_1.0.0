﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Domain;
using Taskr.Dtos.Errors;
using Taskr.Dtos.Profile;
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

            var userReviews = _queryProcessor.Query<Domain.Review>().Include(x => x.Order).ThenInclude(x => x.Job)
                .Include(x => x.Reviewer).Where(x => x.Reviewee == user).AsQueryable();

            var returnReviews = new List<Domain.Review>();
            
            switch (request.Predicate)
            {
                case "Taskr":
                    returnReviews = userReviews 
                        .Where(x => x.Type == ReviewType.Taskr).ToList();
                    break;
                case "Runner":
                      returnReviews = userReviews.Where(x =>  x.Type == ReviewType.Runner).ToList();
                      break;
                default:
                    break;
            }

            return _mapper.Map<List<ReviewDto>>(returnReviews);
        }
    }
}