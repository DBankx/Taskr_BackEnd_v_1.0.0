using System;
using MediatR;
using Taskr.Dtos.Auth;

namespace Taskr.Commands.Auth
{
    public class SignUpCommand : IRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
    }
}