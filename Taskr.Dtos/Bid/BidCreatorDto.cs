using System;

namespace Taskr.Dtos.Bid
{
    public class BidCreatorDto
    {
        public string UserName { get; set; }
        public string Id { get; set; }
        public string Avatar { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}