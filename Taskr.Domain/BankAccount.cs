using System;

namespace Taskr.Domain
{
    public class BankAccount
    {
        public Guid Id { get; set; }
        public string AccountHolderName { get; set; }
        public string AccountHolderType { get; set; }
        public string BankName { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public string RoutingNumber { get; set; }
        public string AccountNumber { get; set; }
    }
}