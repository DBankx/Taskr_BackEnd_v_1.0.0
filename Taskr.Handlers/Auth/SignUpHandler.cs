using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Taskr.Commands.Auth;
using Taskr.Domain;
using Taskr.Dtos.Auth;
using Taskr.Dtos.Errors;
using Taskr.Infrastructure.Jwt;
using Taskr.Infrastructure.Mail;
using Taskr.Infrastructure.Security;
using Taskr.Persistance;

namespace Taskr.Handlers.Auth
{
    public class SignUpHandler : IRequestHandler<SignUpCommand>
    {
        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IMailService _mailService;

        public SignUpHandler(DataContext context, UserManager<ApplicationUser> userManager, IJwtGenerator jwtGenerator, IMailService mailService)
        {
            _context = context;
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
            _mailService = mailService;
        }
        
        public async Task<Unit> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            
             var exisitingUserEmail = await _userManager.Users.SingleOrDefaultAsync(x => x.Email == request.Email || x.UserName == request.Username); 
                        
             if (exisitingUserEmail != null)
             {
                 throw new RestException(HttpStatusCode.BadRequest, new {error = "Email is in use"});
             }
             
             var user = new ApplicationUser
               {
                   Email = request.Email,
                   FirstName = request.FirstName,
                   LastName = request.LastName,
                   Country = request.Country,
                   UserName = request.Username,
                   CreatedAt = DateTime.Now,
                   Avatar = $"https://ui-avatars.com/api/?name={request.Username}&rounded=true&bold=true&background=FCDADC&color=3D3373"
               };
         
             var created = await _userManager.CreateAsync(user, request.Password);
             
             if (!created.Succeeded)
             { 
                 throw new RestException(HttpStatusCode.BadRequest, new {error = "Error occurred while creating user, please try again!"});
             }
             
             var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
             
             code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
             
             var clientRoute = $"{Environment.GetEnvironmentVariable("CLIENT_URL")}/confirm-email";
             
             var verificationUri = QueryHelpers.AddQueryString(clientRoute, "userId", user.Id);
             
             verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
             
             await _mailService.SendMailAsync(new MailRequest
             {
                 Body =
                     $"Please confirm your account by <a href='{verificationUri}'>clicking here</a>",
                 To = user.Email,
                 Subject = "Verification email from taskr"
             });
             
            
             return Unit.Value;
             
        }
    }
}