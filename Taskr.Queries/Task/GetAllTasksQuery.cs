using System.Collections.Generic;
using MediatR;

namespace Taskr.Queries.Task
{
    public class GetAllTasksQuery : IRequest<List<Domain.Task>>
    {
        
    }
}