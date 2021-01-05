using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Commands.Task;
using Taskr.Domain;
using Taskr.Dtos.Errors;
using Taskr.Infrastructure.PhotoService;
using Taskr.Infrastructure.Security;
using Taskr.Persistance;

namespace Taskr.Handlers.Task
{
    public class CreateJobHandler : IRequestHandler<CreateJobCommand>
    {
        private readonly DataContext _context;
        private readonly IUserAccess _userAccess;
        private readonly IPhotoService _photoService;

        public CreateJobHandler(DataContext context, IUserAccess userAccess, IPhotoService photoService)
        {
            _context = context;
            _userAccess = userAccess;
            _photoService = photoService;
        }
        
        public async Task<Unit> Handle(CreateJobCommand request, CancellationToken cancellationToken)
        {
           
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId(), cancellationToken: cancellationToken);
            
            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, new {error = "You are unauthorized"});
            
            if (request.ImageFiles.Count > 3)
            {
                throw new RestException(HttpStatusCode.BadRequest, new {photos = "Only 3 images per job is allowed"});
            }
            
            var job = new Job
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                InitialPrice = request.InitialPrice,
                User = user,
                UserId = user.Id,
                PostCode = request.PostCode,
                City = request.City,
                CreatedAt = DateTime.Now,
                DeliveryDate = request.EndDate
            };
            
            var photoUploadResults = await _photoService.UploadPhoto(request.ImageFiles);
            var jobImages = new List<Photo>();
            
            foreach (var photoUpload in photoUploadResults)
            {
                jobImages.Add(new Photo
                {
                    Id = photoUpload.PublicId,
                    Url = photoUpload.Url
                });
            }

            job.Photos = jobImages;
            
            await _context.Jobs.AddAsync(job, cancellationToken);
            
            var created = await _context.SaveChangesAsync(cancellationToken) > 0;
            
            if (!created)
            {
                throw new RestException(HttpStatusCode.InternalServerError,
                    new {error = "Server error occurred"});
            }
            
            return Unit.Value;
        }
    }
}