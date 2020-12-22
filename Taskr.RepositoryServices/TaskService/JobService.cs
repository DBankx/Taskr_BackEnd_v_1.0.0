using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taskr.Domain;
using Taskr.Persistance;

namespace Taskr.RepositoryServices.TaskService
{
    public class JobService : IJobService
    {
        private readonly DataContext _context;

        public JobService(DataContext context)
        {
            _context = context;
        } 
        
        public async Task<List<Job>> GetAllTasksAsync()
        {
            return await _context.Jobs.ToListAsync();
        }

        public async Task<Domain.Job> GetTaskByIdAsync(Guid id)
        {
            var task = await _context.Jobs.SingleOrDefaultAsync(x => x.Id == id);

            return task;
        }

        public async Task<bool> DeleteTaskAsync(Guid id)
        {
            var task = await GetTaskByIdAsync(id);

            if (task == null)
            {
                return false;
            }

            _context.Jobs.Remove(task);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CreateTaskAsync(Job job)
        {
            await _context.Jobs.AddAsync(job);
            return await _context.SaveChangesAsync() > 0;
        }

       
    }
}