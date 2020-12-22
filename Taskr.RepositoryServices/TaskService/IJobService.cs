using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Taskr.Domain;

namespace Taskr.RepositoryServices.TaskService
{
    /// <summary>
    /// Interface contract with all task methods
    /// </summary>
    public interface IJobService
    {
        Task<List<Domain.Job>> GetAllTasksAsync();
        Task<Domain.Job> GetTaskByIdAsync(Guid id);
        Task<bool> DeleteTaskAsync(Guid id);
        Task<bool> CreateTaskAsync(Job job);
    }
}