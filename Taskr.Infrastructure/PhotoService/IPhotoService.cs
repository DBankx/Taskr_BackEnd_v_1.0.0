using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Taskr.Dtos.Photo;

namespace Taskr.Infrastructure.PhotoService
{
    public interface IPhotoService
    {
        Task<List<PhotoUploadResult>> UploadPhoto(List<IFormFile> files);
        string DeletePhoto(string[] publicIds);
    }
}