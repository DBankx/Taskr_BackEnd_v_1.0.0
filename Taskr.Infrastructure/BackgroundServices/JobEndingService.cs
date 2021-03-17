using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Taskr.Domain;
using Taskr.Persistance;

namespace Taskr.Infrastructure.BackgroundServices
{
    public class JobEndingService : IJobEndingService
    {
        private readonly DataContext _context;
        private readonly ILogger<JobEndingService> _logger;

        public JobEndingService(DataContext context, ILogger<JobEndingService> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public async Task EndAllInactiveJobs(CancellationToken cancellationToken)
        {
           _logger.LogInformation("Start end all inactive jobs");

           var currentDate = DateTime.UtcNow;
           var inActiveJobs = await _context.Jobs
               .Where(job => job.JobStatus == JobStatus.Active && job.DeliveryDate < currentDate)
               .ToListAsync(cancellationToken);

           foreach (var job in inActiveJobs)
           {
               job.JobStatus = JobStatus.InActive;
           }
           
           var updateSuccessful = await _context.SaveChangesAsync(cancellationToken) > 1;
    
           if(!updateSuccessful)
           {
               _logger.LogError("End inactive jobs failed");
           }
    
           _logger.LogInformation("Stop end inactive jobs");
        }
    }
}