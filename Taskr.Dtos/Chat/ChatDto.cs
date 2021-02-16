using System;
using System.Collections.Generic;
using Taskr.Domain;

namespace Taskr.Dtos.Chat
{
    public class ChatDto
    {
        public Guid Id { get; set; }
        public string JobTitle { get; set; }
        public decimal JobBudget { get; set; }
        public ICollection<Domain.Photo> JobPhotos { get; set; }
        public DateTime CreatedAt { get; set; }
        public string NewestMessage { get; set; }
        public MessageSenderDto Taskr { get; set; }
        public MessageSenderDto Runner { get; set; }
    }
}