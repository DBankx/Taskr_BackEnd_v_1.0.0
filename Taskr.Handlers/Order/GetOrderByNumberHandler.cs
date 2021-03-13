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
using Taskr.Queries.Order;

namespace Taskr.Handlers.Order
{
    public class GetOrderByNumberHandler : IRequestHandler<GetOrderByNumber, OrderDetailsDto>
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IUserAccess _userAccess;
        private readonly IMapper _mapper;

        public GetOrderByNumberHandler(IQueryProcessor queryProcessor, IUserAccess userAccess, IMapper mapper)
        {
            _queryProcessor = queryProcessor;
            _userAccess = userAccess;
            _mapper = mapper;
        }
        
        public async Task<OrderDetailsDto> Handle(GetOrderByNumber request, CancellationToken cancellationToken)
        {
            var user = await _queryProcessor.Query<ApplicationUser>()
                .SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId(), cancellationToken);

            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, new {error = "You are unauthorized"});

            var order = await _queryProcessor.Query<Domain.Order>().Include(x => x.Job)
                .ThenInclude(x => x.Photos).Include(x => x.User).Include(x => x.PayTo).Include(x => x.AcceptedBid)
                .SingleOrDefaultAsync(x => x.OrderNumber == request.OrderNumber, cancellationToken);

            if (order == null) throw new RestException(HttpStatusCode.NotFound, new {error = "Order not found"});

            if (order.User.Id != user.Id && order.PayTo.Id != user.Id)
            {
                throw new RestException(HttpStatusCode.Unauthorized, new {error = "You are unauthorized to complete this action"});
                            
            }
                
            return _mapper.Map<OrderDetailsDto>(order);
        }
    }
}