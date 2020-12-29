using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Domain;
using Taskr.Dtos.Job;
using Taskr.Persistance;
using Taskr.Queries.Bid;

namespace Taskr.Handlers.Task
{
    public class GetAllJobsHandler : IRequestHandler<GetAllJobsQuery, IQueryable<AllJobsDto>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public GetAllJobsHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        
        public async Task<IQueryable<AllJobsDto>> Handle(GetAllJobsQuery request, CancellationToken cancellationToken)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Domain.Job, AllJobsDto>().ForMember(x => x.CreatorId, opt => opt.MapFrom(x => x.User.Id))
                    .ForMember(x => x.CreatorUsername, opt => opt.MapFrom(x => x.User.UserName));
            });
            
            var jobs = _context.Jobs.ProjectTo<AllJobsDto>(configuration);
            return jobs;
        }
    }
}