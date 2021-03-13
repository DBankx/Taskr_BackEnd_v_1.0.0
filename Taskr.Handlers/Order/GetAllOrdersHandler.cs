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
using Taskr.Dtos.Order;
using Taskr.Infrastructure.Helpers;
using Taskr.Infrastructure.Security;
using Taskr.Queries.Filter;
using Taskr.Queries.Order;

namespace Taskr.Handlers.Order
{
    public class GetAllOrdersHandler : IRequestHandler<GetAllOrders, List<ListOrderDto>>
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IMapper _mapper;
        private readonly IUserAccess _userAccess;

        public GetAllOrdersHandler(IQueryProcessor queryProcessor, IMapper mapper, IUserAccess userAccess)
        {
            _queryProcessor = queryProcessor;
            _mapper = mapper;
            _userAccess = userAccess;
        }
        
        public async Task<List<ListOrderDto>> Handle(GetAllOrders request, CancellationToken cancellationToken)
        {
            var user = await _queryProcessor.Query<ApplicationUser>()
                .SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId(), cancellationToken);

            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, new {error = "You are unauthorized"});

            var orders = await _queryProcessor.Query<Domain.Order>().Include(x => x.Job).ThenInclude(x => x.Photos)
                .Include(x => x.User).Include(x => x.PayTo).ToListAsync(cancellationToken);

            var filteredOrders = AddFilterToQueries.FilterOrders(request.Predicate, orders, user.Id);

            return _mapper.Map<List<ListOrderDto>>(filteredOrders);
        }
    }
}