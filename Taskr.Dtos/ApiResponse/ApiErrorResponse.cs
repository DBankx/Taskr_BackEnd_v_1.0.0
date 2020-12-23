using System.Collections.Generic;
using System.Net;

namespace Taskr.Dtos.ApiResponse
{
    public class ApiErrorResponse
    {
        public IEnumerable<string> Errors { get; set; }

        public ApiErrorResponse(IEnumerable<string> errors)
        {
            Errors = errors;
        }
    }
}