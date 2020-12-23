using System.Net;

namespace Taskr.Dtos.ApiResponse
{
    public class ApiSuccessResponse<T>
    {
        public bool Success { get; set; } = true;
        
        public T Data { get; set; }

        public ApiSuccessResponse(T data)
        {
            Data = data;
        }
    }
}