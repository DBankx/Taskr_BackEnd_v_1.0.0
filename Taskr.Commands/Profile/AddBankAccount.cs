using MediatR;

namespace Taskr.Commands.Profile
{
    public class AddBankAccount : IRequest
    {
        public string RoutingNumber { get; set; }
        public string AccountNumber { get; set; }
        public string AccountHolderName { get; set; }
        public string AccountHolderType { get; set; }
    }
}