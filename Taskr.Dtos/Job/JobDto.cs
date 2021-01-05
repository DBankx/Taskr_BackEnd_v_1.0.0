﻿using System;
using System.Collections.Generic;
using Taskr.Domain;

namespace Taskr.Dtos.Job
{
    public class JobDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal InitialPrice { get; set; }
        public ICollection<Domain.Photo> Photos { get; set; }
        public JobCreatorDto Creator { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public int Views { get; set; }
        public int BidsCount { get; set; }
        public int WatchCount { get; set; }
    }
}