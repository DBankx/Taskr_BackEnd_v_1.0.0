using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taskr.Persistance;
using Task = Taskr.Domain.Task;

namespace Taskr.RepositoryServices.TaskService
{
    public class TaskService : ITaskService
    {
        private readonly DataContext _context;

        public TaskService(DataContext context)
        {
            _context = context;
        } 
        
        public async Task<List<Task>> GetAllTasksAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<Domain.Task> GetTaskByIdAsync(Guid id)
        {
            var task = await _context.Tasks.SingleOrDefaultAsync(x => x.Id == id);

            return task;
        }

        public async Task<bool> DeleteTaskAsync(Guid id)
        {
            var task = await GetTaskByIdAsync(id);

            if (task == null)
            {
                return false;
            }

            _context.Tasks.Remove(task);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CreateTaskAsync(Task task)
        {
            await _context.Tasks.AddAsync(task);
            return await _context.SaveChangesAsync() > 0;
        }

       
    }
}