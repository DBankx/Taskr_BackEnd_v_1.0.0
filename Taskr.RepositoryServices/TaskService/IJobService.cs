using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Taskr.Domain;
using Taskr.Dtos.ApiResponse;

namespace Taskr.RepositoryServices.TaskService
{
    /// <summary>
    /// Interface contract with all task methods
    /// </summary>
    public interface IJobService
    {
        Task<ApiResponse<List<Domain.Job>>> GetAllJobsAsync();
        Task<ApiResponse<Domain.Job>> GetJobByIdAsync(Guid id);
        Task<ApiResponse<object>> DeleteJobAsync(Guid id, string userId);
        Task<ApiResponse<object>> CreateJobAsync(Job job, string userId);
        Task<bool> UserOwnsJobAsync(Guid jobId, string userId);
    }
}