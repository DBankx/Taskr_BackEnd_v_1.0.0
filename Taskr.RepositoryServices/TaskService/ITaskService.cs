using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task = Taskr.Domain.Task;

namespace Taskr.RepositoryServices.TaskService
{
    /// <summary>
    /// Interface contract with all task methods
    /// </summary>
    public interface ITaskService
    {
        Task<List<Domain.Task>> GetAllTasksAsync();
        Task<Domain.Task> GetTaskByIdAsync(Guid id);
        Task<bool> DeleteTaskAsync(Guid id);
        Task<bool> CreateTaskAsync(Task task);
    }
}