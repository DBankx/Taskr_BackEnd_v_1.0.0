using System;
using System.Collections.Generic;
using System.Linq;
using Taskr.Domain;

namespace Taskr.RepositoryServices.TaskService
{
    public class TaskService : ITaskService
    {
        private readonly List<Task> _tasks = new List<Task>
        {
            new Task{Id = Guid.Parse("64fa643f-2d35-46e7-b3f8-31fa673d719b"), Title = "Nick Chapsas", Description = "New Dami Task 1 desc", InitialPrice = 20.30m},
            new Task{Id = Guid.Parse("fc7cdfc4-f407-4955-acbe-98c666ee51a2"), 
                InitialPrice = 10.30m,
                Description = "new Dami Task 2 desc",
                Title = "New Dami task 2"}
        };

        public List<Task> GetAllTasks()
        {
            return _tasks;
        }

        public Task GetTaskById(Guid id)
        {
            var task = _tasks.SingleOrDefault(x => x.Id == id);

            return task;
        }
    }
}