using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Taskr.Infrastructure.BackgroundServices;

namespace Taskr.Api.Background
{
    public class HostedBackGroundService : IHostedService
    {
        private readonly ILogger<HostedBackGroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public HostedBackGroundService(ILogger<HostedBackGroundService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(async o =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IJobEndingService>();
                     await scopedProcessingService.EndAllInactiveJobs(cancellationToken);
                }
            }, null, TimeSpan.Zero, TimeSpan.FromMinutes(30));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Hosted background service stopped");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}