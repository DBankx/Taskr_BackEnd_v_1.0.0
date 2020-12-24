using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Taskr.Domain;
using Taskr.Dtos.ApiResponse;

namespace Taskr.RepositoryServices.BidService
{
    /// <summary>
    /// Service for making, recieving, accepting and deleting bids
    /// </summary>
    public interface IBidService
    {
        Task<ApiResponse<List<Bid>>> GetAllJobBidsAsync(Guid jobId);
        Task<ApiResponse<Bid>> GetBidByIdAsync(Guid bidId);
        Task<ApiResponse<object>> CreateBidAsync(Guid jobId, string userId, Bid bid);
    }
}