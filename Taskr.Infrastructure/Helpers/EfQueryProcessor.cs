using System.Linq;
using Microsoft.EntityFrameworkCore;
using Taskr.Persistance;

namespace Taskr.Infrastructure.Helpers
{
    public class EfQueryProcessor : IQueryProcessor
    {
        private readonly DataContext _context;

        public EfQueryProcessor(DataContext context)
        {
            _context = context;
        }
        
        public IQueryable<T> Query<T>() where T : class
        {
            return _context.Set<T>().AsNoTracking();
        }
    }
}