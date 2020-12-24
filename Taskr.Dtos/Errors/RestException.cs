using System;
using System.Net;

namespace Taskr.Dtos.Errors
{
    public class RestException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public object Errors { get; set; }

        public RestException(HttpStatusCode statusCode, object errors = null)
        {
            Errors = errors;
            StatusCode = statusCode;
        }
    }
}