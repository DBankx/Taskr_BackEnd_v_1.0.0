using Taskr.Domain;

namespace Taskr.Queries.Task.Filter
{
    public class GetAllJobsFilter
    {
        public string Title { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public string SortBy { get; set; }
        public Category? Category { get; set; }
        public DeliveryTypes? DeliveryType{ get; set; }

        public bool ValidPriceRange => MaxPrice >= MinPrice;
    }

}