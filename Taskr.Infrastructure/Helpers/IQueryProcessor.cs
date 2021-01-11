using System.Linq;

namespace Taskr.Infrastructure.Helpers
{
    public interface IQueryProcessor
    {
        IQueryable<T> Query<T>() where T : class;
    }
}