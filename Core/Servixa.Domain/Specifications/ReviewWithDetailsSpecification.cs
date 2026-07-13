using Servixa.Domain.Contracts;
using Servixa.Domain.Models.ReviewEntity;

namespace Servixa.Domain.Specifications
{
    public class ReviewWithDetailsSpecification : BaseSpecification<Review>
    {
        public ReviewWithDetailsSpecification(int workerId) 
            : base(r => r.RevieweeId == workerId)
        {
            AddInclude(r => r.Reviewer);
            AddOrderByDescending(r => r.CreatedAt);
        }
    }
}
