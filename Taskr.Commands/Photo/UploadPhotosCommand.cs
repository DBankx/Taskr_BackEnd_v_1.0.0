using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Taskr.Commands.Photo
{
    public class UploadPhotosCommand : IRequest<List<Domain.Photo>>
    {
        public List<IFormFile> Files { get; set; }
    }
}