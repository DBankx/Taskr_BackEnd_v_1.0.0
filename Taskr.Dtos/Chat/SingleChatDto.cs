using System;
using System.Collections.Generic;

namespace Taskr.Dtos.Chat
{
    public class SingleChatDto
    {
        public Guid Id { get; set; }
         public string JobTitle { get; set; }
         public decimal JobBudget { get; set; }
         public ICollection<Domain.Photo> JobPhotos { get; set; }
         public DateTime CreatedAt { get; set; }
         public ICollection<MessageDto> Messages { get; set; }
    }
}