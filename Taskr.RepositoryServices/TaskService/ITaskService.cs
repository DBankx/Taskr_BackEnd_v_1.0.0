using System;
using System.Collections.Generic;
using Taskr.Domain;

namespace Taskr.RepositoryServices.TaskService
{
    /// <summary>
    /// Interface contract with all task methods
    /// </summary>
    public interface ITaskService
    {
        List<Task> GetAllTasks();
        Task GetTaskById(Guid id);
    }
}