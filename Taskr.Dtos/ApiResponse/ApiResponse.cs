using System.Collections.Generic;
using System.Net;

namespace Taskr.Dtos.ApiResponse
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public IEnumerable<string> Errors { get; set; } 
    }
}