using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Taskr.Commands.Profile;
using Taskr.Dtos.Errors;
using Taskr.Infrastructure.Security;
using Taskr.Persistance;
using BankAccount = Taskr.Domain.BankAccount;

namespace Taskr.Handlers.Profile
{
    public class AddBankAccountHandler : IRequestHandler<AddBankAccount>
    {
        private readonly DataContext _context;
        private readonly IUserAccess _userAccess;

        public AddBankAccountHandler(DataContext context, IUserAccess userAccess)
        {
            _context = context;
            _userAccess = userAccess;
        }
        
        public async Task<Unit> Handle(AddBankAccount request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId(),
                cancellationToken);

            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, new {error = "You are unauthorized"});

            var userBank = new BankAccount
            {
                Country = "US",
                Currency = "usd",
                AccountNumber = request.AccountNumber,
                RoutingNumber = request.RoutingNumber,
                BankName = "HSBC",
                AccountHolderName = request.AccountHolderName,
                AccountHolderType = request.AccountHolderType
            };
            
           user.BankAccount = userBank;

           var saved = await _context.SaveChangesAsync(cancellationToken) > 0;

           if (!saved) throw new Exception("Problem saving changes");
           
           return Unit.Value;

        }
    }
}