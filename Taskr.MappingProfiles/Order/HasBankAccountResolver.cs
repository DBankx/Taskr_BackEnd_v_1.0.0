using AutoMapper;
using Taskr.Domain;
using Taskr.Dtos.Order;

namespace Taskr.MappingProfiles.Order
{
    public class HasBankAccountResolver : IValueResolver<ApplicationUser, OrderUser, bool>
    {
        public bool Resolve(ApplicationUser source, OrderUser destination, bool destMember, ResolutionContext context)
        {
            return source.BankAccount != null;
        }
    }
}