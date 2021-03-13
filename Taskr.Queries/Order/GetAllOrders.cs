using System.Collections.Generic;
using MediatR;
using Taskr.Dtos.Order;

namespace Taskr.Queries.Order
{
    public class GetAllOrders : IRequest<List<ListOrderDto>>
    {
        public string Predicate { get; set; }

        public GetAllOrders(string predicate)
        {
            Predicate = predicate;
        }
    }
}