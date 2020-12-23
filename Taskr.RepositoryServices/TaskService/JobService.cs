using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taskr.Domain;
using Taskr.Dtos.ApiResponse;
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
        
        public async Task<ApiResponse<List<Job>>> GetAllJobsAsync()
        {
            return new ApiResponse<List<Job>>()
            {
                Data = await _context.Jobs.ToListAsync(),
                Success = true
            };
        }

        public async Task<ApiResponse<Domain.Job>> GetJobByIdAsync(Guid id)
        {
            var job = await _context.Jobs.SingleOrDefaultAsync(x => x.Id == id);
            if (job == null)
            {
                return new ApiResponse<Job>
                {
                    Success = false,
                    Errors = new[] {"Job not found"}
                };
            }
            return new ApiResponse<Job>
            {
                Data = job,
                Success = true
            };
        }

        public async Task<ApiResponse<object>> DeleteJobAsync(Guid id, string userId)
        {
            var userOwnsJob = await UserOwnsJobAsync(id, userId);
            
            if (!userOwnsJob) return new ApiResponse<object>
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Errors = new []{"You do not have access to delete this content"}
            };
            
            var job = await GetJobByIdAsync(id);

            if (job == null)
            {
                return new ApiResponse<object>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Errors = new[] {"Job not found"}
                };
            }

            _context.Jobs.Remove(job.Data);

            var deleted = await _context.SaveChangesAsync() > 0;

            if (!deleted)
                return new ApiResponse<object>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Errors = new[] {"Internal server error occurred"}
                };

            return new ApiResponse<object>()
            {
                StatusCode = HttpStatusCode.OK,
                Data = new {message = "Task deleted successfully"}
            };
        }

        public async Task<ApiResponse<object>> CreateJobAsync(Job job, string userId)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId);
            if (user == null) return new ApiResponse<object>
            {
                Errors = new []{"You are not authorized"},
                StatusCode = HttpStatusCode.Unauthorized
            };
            job.User = user;
            await _context.Jobs.AddAsync(job);
            var created = await _context.SaveChangesAsync() > 0;
            if (!created)
            {
                return new ApiResponse<object>
                {
                    Errors = new[] {"Error occured creating Job"},
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }

            return new ApiResponse<object>
            {
                Data = new {message = "Task created successfully"},
                Success = true,
                StatusCode = HttpStatusCode.Created
            };
        }

        public async Task<bool> UserOwnsJobAsync(Guid jobId, string userId)
        {
            var job = await _context.Jobs.SingleOrDefaultAsync(x => x.Id == jobId);
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId);
            if (user == null) return false;
            if (job == null) return false;
            return job.User == user;
        }
    }
}