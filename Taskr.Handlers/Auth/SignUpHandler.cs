using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Taskr.Commands.Auth;
using Taskr.Domain;
using Taskr.Dtos.Auth;
using Taskr.Infrastructure.Jwt;
using Taskr.Persistance;

namespace Taskr.Handlers.Auth
{
    public class SignUpHandler : IRequestHandler<SignUpCommand, AuthResponse>
    {
        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtGenerator _jwtGenerator;

        public SignUpHandler(DataContext context, UserManager<ApplicationUser> userManager, IJwtGenerator jwtGenerator)
        {
            _context = context;
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
        }
        
        public async Task<AuthResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            
             var exisitingUserEmail = await _userManager.Users.SingleOrDefaultAsync(x => x.Email == request.Email || x.UserName == request.Username); 
                        
             if (exisitingUserEmail != null) 
             { 
                 return new AuthResponse
                 {
                     Errors = new[] {"Invalid credentials"},
                 };
             }
             var user = new ApplicationUser
               {
                   Email = request.Email,
                   FirstName = request.FirstName,
                   LastName = request.LastName,
                   Bio = request.Bio,
                   City = request.City,
                   UserName = request.Username,
                   CreatedAt = DateTime.Now
               };
         
             var created = await _userManager.CreateAsync(user, request.Password);
             if (!created.Succeeded)
             { 
                 return new AuthResponse
                 {
                     Errors = created.Errors.Select(x => x.Description),
                 };
             }
            
             var authReturnUser = new AuthUserResponse
             {
                 Email = user.Email,
                 Username = user.UserName
             };
            
             return new AuthResponse
             {
                 Success = true, 
                 Token = _jwtGenerator.GenerateToken(user),
                 User = authReturnUser
             };
        }
    }
}