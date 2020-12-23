using System.Collections.Generic;
using MediatR;
using Taskr.Domain;
using Taskr.Dtos.ApiResponse;

namespace Taskr.Queries
{
    public class GetAllTasksQuery : IRequest<ApiResponse<List<Job>>>
    {
        
    }
}