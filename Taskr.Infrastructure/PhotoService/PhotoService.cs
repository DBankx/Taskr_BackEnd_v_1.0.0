using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Taskr.Dtos.Photo;

namespace Taskr.Infrastructure.PhotoService
{
    /// <summary>
    /// TODO - check if photo deletion is working
    /// </summary>
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        
        public PhotoService(IOptions<CloudinarySettings> config)
        {
            _cloudinary = new Cloudinary(config.Value.ApiUrl);
        }
        
        public async Task<List<PhotoUploadResult>> UploadPhoto(List<IFormFile> files)
        {
            var photoUploadResults = new List<PhotoUploadResult>();
            // check if the file uploaded is empty
            foreach (var file in files)
            {
                var uploadResult = new ImageUploadResult();
                if (file.Length > 0)
                {
                    using (var fileStream = file.OpenReadStream())
                    {
                        var uploadParams = new ImageUploadParams
                        {
                            File = new FileDescription(file.FileName, fileStream),
                            Transformation = new Transformation().Crop("fill")
                        };
                        uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    }
                }

                if (uploadResult.Error != null)
                {
                    throw new Exception(uploadResult.Error.Message);
                }

                var successfulUpload = new PhotoUploadResult
                {
                    PublicId = uploadResult.PublicId,
                    Url = uploadResult.SecureUrl.AbsoluteUri
                };
                
                photoUploadResults.Add(successfulUpload);
            }
            return photoUploadResults;
        }

        public string DeletePhoto(string[] publicIds)
        {
            bool allDeleted = false;
            foreach (var id in publicIds)
            {
                var deleteParams = new DeletionParams(id);
                var result = _cloudinary.Destroy(deleteParams);
                allDeleted = result.Result == "ok" ? true  : false;    
            }
            if (allDeleted) return "ok";
            return null;
        }
    }
}