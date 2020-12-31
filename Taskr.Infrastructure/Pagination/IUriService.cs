using System;

namespace Taskr.Infrastructure.Pagination
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}