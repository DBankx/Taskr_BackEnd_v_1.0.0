using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Commands.Bid;
using Taskr.Domain;
using Taskr.Dtos;
using Taskr.Dtos.Errors;
using Taskr.Infrastructure.Mail;
using Taskr.Infrastructure.MediatrNotifications;
using Taskr.Infrastructure.Security;
using Taskr.Persistance;

namespace Taskr.Handlers.Bid
{
    // TODO - change email link from localhost to main deployed url
    public class CreateBidHandler : IRequestHandler<CreateBidCommand, BidDto>
    {
        private readonly DataContext _context;
        private readonly IUserAccess _userAccess;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CreateBidHandler(DataContext context, IUserAccess userAccess, IMapper mapper, IMediator mediator)
        {
            _context = context;
            _userAccess = userAccess;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<BidDto> Handle(CreateBidCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId(),
                cancellationToken: cancellationToken);

            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, new {error = "You are unauthorized"});
            
            var job = await _context.Jobs.Include(x => x.User).SingleOrDefaultAsync(x => x.Id == request.JobId, cancellationToken: cancellationToken);
            
            if (job == null)
            { 
                throw new RestException(HttpStatusCode.NotFound, new {error = "Job not found"});
            }

            if (job.User == user)
                throw new RestException(HttpStatusCode.BadRequest, new {bid = "You cannot bid on your own post"});
            
            // delete any previous bid the user has on the job
            var prevBid = await _context.Bids.SingleOrDefaultAsync(x => x.User == user && x.Job == job, cancellationToken: cancellationToken);

            if (prevBid != null)
            {
                _context.Bids.Remove(prevBid);
            }

            if (request.Price >= job.InitialPrice)
            {
                throw new RestException(HttpStatusCode.BadRequest,
                    new {price = "The price of a bid must be lower than the job price"});
            }
            
            var bid = new Domain.Bid
            {
                Status = BidStatus.Submitted,
                CreatedAt = DateTime.Now,
                JobId = request.JobId,
                Job = job,
                Description = request.Description,
                Price = request.Price,
                UserId = user.Id,
                User = user,
                Id = request.Id
            };

            await _context.Bids.AddAsync(bid, cancellationToken);

            var created = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (!created)
            {
                throw new Exception("Error occurred while creating bid");
            }

            var appUserNotif = new UserPrivateMessageNotification(job.UserId, user.Id, user.UserName, user.Avatar, $"{user.UserName} placed a bid on {job.Title}", job.Id, DateTime.Now, NotificationType.Bid, NotificationStatus.UnRead);

            await _mediator.Publish(appUserNotif, cancellationToken);

             _mediator.Publish(new MailRequestNotification
            {
                To = job.User.Email, Subject = $"Someone has made an offer for your task {job.Title}",
                Body =
                    $"<h1>Hi {job.User.FirstName}</h1> <p>People are lining up to do your task <a href='http://localhost:3000/task/{job.Id}'>{job.Title}</a>.</p><p>Its time to make a decision, who will you choose?</p>, <p>Thanks,</p><p>Taskr</p>"
            }, cancellationToken);
            
            
            return _mapper.Map<BidDto>(bid);
        }
    }
}