﻿using System.Collections.Generic;
using System.Linq;
using MediatR;
using Taskr.Domain;

namespace Taskr.Queries.Bid
{
    public class GetAllJobsQuery : IRequest<IQueryable<Job>>
    {
        
    }
}