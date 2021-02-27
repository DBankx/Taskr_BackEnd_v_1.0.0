using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using Taskr.Commands.Bid;
using Taskr.Domain;
using Taskr.Dtos.Errors;
using Taskr.Dtos.Order;
using Taskr.Infrastructure.Helpers;
using Taskr.Infrastructure.Security;
using Taskr.Persistance;

namespace Taskr.Handlers.Bid
{
    public class AcceptBidHandler : IRequestHandler<AcceptBidCommand, OrderDetailsDto>
    {
        private readonly DataContext _context;
        private readonly IUserAccess _userAccess;
        private readonly IMapper _mapper;

        public AcceptBidHandler(DataContext context, IUserAccess userAccess, IMapper mapper)
        {
            _context = context;
            _userAccess = userAccess;
            _mapper = mapper;
        }
        
        public async Task<OrderDetailsDto> Handle(AcceptBidCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId(), cancellationToken);

            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, new {error = "You are unauthorized"});

            // get the job that is being paid for
            var job = await _context.Jobs.SingleOrDefaultAsync(x => x.Id == request.JobId, cancellationToken);

            if (job == null) throw new RestException(HttpStatusCode.NotFound, new {error = "Task not found"});
            
            // check if loggedIn user is the same as user that created job
            if (job.UserId != user.Id)
                throw new RestException(HttpStatusCode.Unauthorized,
                    new {error = "You are unauthorized to complete this action"});

            // get the bid on the job being accepted
            var bid = await _context.Bids.Include(x => x.User).SingleOrDefaultAsync(x => x.Id == request.BidId, cancellationToken);
            
            if(bid == null) throw new RestException(HttpStatusCode.NotFound, new {error = "Bid not found"});

            // Create order
            var order = new Order
            {
                Job = job,
                Status = OrderStatus.Pending,
                AcceptedBid = bid,
                OrderPlacementDate = DateTime.Now,
                OrderNumber = GenerateUniqueOrderNumber.GenerateNumber(),
                TotalAmount = bid.Price + 2.50m,
                User = user,
                PayTo = bid.User
            };

            var domain = "http://localhost:3000/checkout";
            
            // create stripe checkout session
            var checkoutOptions = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmountDecimal = order.TotalAmount * 1000,
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = job.Title,
                                Description = job.Description
                            },
                        },
                        Quantity = 1
                    }
                },
                Mode = "payment",
                SuccessUrl = domain + "?success=true",
                CancelUrl = domain + "?cancelled=true"
            };
            
            var service = new SessionService();
            Session session = await service.CreateAsync(checkoutOptions, cancellationToken: cancellationToken);

            // Add checkout session details to order
            order.CheckoutSessionId = session.Id;
            order.OrderCompletedDate = DateTime.Now;

            // add order to db
            _context.Orders.Add(order);

            var completedPayment = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!completedPayment) throw new Exception("Something went wrong");

            // return orderDto object
            return _mapper.Map<OrderDetailsDto>(order);

        }
    }
}