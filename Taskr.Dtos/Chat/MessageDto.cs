﻿using System;
using Taskr.Domain;

namespace Taskr.Dtos.Chat
{
    public class MessageDto
    {
        public string Text { get; set; }
        public DateTime SentAt { get; set; }
        public MessageSenderDto Sender { get; set; }
    }
}