using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taskr.Domain;
using Taskr.Dtos.ApiResponse;
using Taskr.Persistance;

namespace Taskr.RepositoryServices.BidService
{
    public class BidService : IBidService
    {
        private readonly DataContext _context;

        public BidService(DataContext context)
        {
            _context = context;
        }


        public async Task<ApiResponse<List<Bid>>> GetAllJobBidsAsync(Guid jobId)
        {
            var bids = await _context.Bids.Where(x => x.JobId == jobId).ToListAsync();
            return new ApiResponse<List<Bid>>
            {
                Data = bids,
                Success = true
            };
        }

        public async Task<ApiResponse<Bid>> GetBidByIdAsync(Guid bidId)
        {
            var bid = await _context.Bids.SingleOrDefaultAsync(x => x.Id == bidId);
            if (bid == null)
                return new ApiResponse<Bid>
                {
                    Errors = new[] {"Bid was not found"},
                    StatusCode = HttpStatusCode.NotFound,
                };
            return new ApiResponse<Bid>
            {
                Data = bid,
                Success = true
            };
        }

        public async Task<ApiResponse<object>> CreateBidAsync(Guid jobId, string userId, Bid bid)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                return new ApiResponse<object>
                {
                    Errors = new[] {"You are unauthorized"},
                    StatusCode = HttpStatusCode.Unauthorized
                };
            bid.User = user; 
            var job = await _context.Jobs.SingleOrDefaultAsync(x => x.Id == jobId);
            if (job == null)
            {
                return new ApiResponse<object>
                {
                    Errors = new[] {"Job not found"},
                    StatusCode = HttpStatusCode.Unauthorized
                };
            }

            bid.Job = job;
            await _context.Bids.AddAsync(bid);
            var created = await _context.SaveChangesAsync() > 0;
            if (!created)
            {
                return new ApiResponse<object>
                {
                    Errors = new[] {"Server error occurred"},
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }

            return new ApiResponse<object>
            {
                Success = true,
                Data = new {Message = "Your bid was successfully created"}
            };
        }
    }
}