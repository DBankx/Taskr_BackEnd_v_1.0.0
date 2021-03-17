using System.Threading;
using System.Threading.Tasks;

namespace Taskr.Infrastructure.BackgroundServices
{
    public interface IJobEndingService
    {
        Task EndAllInactiveJobs(CancellationToken cancellationToken);
    }
}