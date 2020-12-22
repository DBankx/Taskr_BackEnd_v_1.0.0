using System.Collections.Generic;
using MediatR;
using Taskr.Domain;

namespace Taskr.Queries
{
    public class GetAllTasksQuery : IRequest<List<Job>>
    {
        
    }
}