namespace Taskr.Dtos.Order
{
    public class OrderUser
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        public bool HasActiveBankAccount { get; set; }
    }
}