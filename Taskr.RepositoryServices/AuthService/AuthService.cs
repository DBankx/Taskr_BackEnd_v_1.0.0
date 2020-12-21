using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Taskr.Domain;
using Taskr.Dtos.Auth;
using Taskr.Infrastructure.Jwt;
using Taskr.Persistance;

namespace Taskr.RepositoryServices.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly DataContext _context;

        public AuthService(UserManager<ApplicationUser> userManager, IJwtGenerator jwtGenerator, DataContext context)
        {
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
            _context = context;
        }
        
        public async Task<AuthResponse> SignInAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new AuthResponse
                {
                    Errors = new[] {"Invalid credentials"}
                };
            }

            var passwordMatch = await _userManager.CheckPasswordAsync(user, password);
            
            if (!passwordMatch)
            {
                return new AuthResponse
                {
                    Errors = new[] {"Invalid credentials"}
                };
            }

            var authUserResponse = new AuthUserResponse
            {
                Email = user.Email,
                Username = user.UserName
            };

            return new AuthResponse
            {
                Success = true,
                Token = _jwtGenerator.GenerateToken(user),
                User = authUserResponse
            };
        }

        public async Task<AuthResponse> SignUpAsync(ApplicationUser user, string password)
        {
             var exisitingUserEmail = await _userManager.Users.SingleOrDefaultAsync(x => x.Email == user.Email || x.UserName == user.UserName); 
            
                        if (exisitingUserEmail != null)
                        {
                            return new AuthResponse
                            {
                                Errors = new[] {"Invalid credentials"},
                            };
                        }
            
                        var created = await _userManager.CreateAsync(user, password);
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