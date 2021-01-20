using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Http;
using Taskr.Domain;

namespace Taskr.Commands.Task
{
    
    public class CreateJobCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal InitialPrice { get; set; }
        public string UserId { get; set; }
        public List<IFormFile> ImageFiles { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime EndDate { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public int Views { get; set; }
        public Category Category { get; set; }
        public DeliveryTypes DeliveryType { get; set; }
    }
}