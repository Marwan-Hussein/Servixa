using Servixa.Domain.Contracts;
using Servixa.Domain.Models.BookingEntity;

namespace Servixa.Domain.Specifications
{
    public class BookingWithDetailsSpecification : BaseSpecification<Booking>
    {
        public BookingWithDetailsSpecification() : base()
        {
            AddInclude(b => b.Client);
            AddInclude(b => b.Worker);
            AddInclude(b => b.Task);
            AddInclude(b => b.Payment!);
        }

        public BookingWithDetailsSpecification(int id) : base(b => b.Id == id)
        {
            AddInclude(b => b.Client);
            AddInclude(b => b.Worker);
            AddInclude(b => b.Task);
            AddInclude(b => b.Payment!);
        }

        public BookingWithDetailsSpecification(string clientOrWorkerId, bool isClient) 
            : base(b => (isClient && b.ClientId.ToString() == clientOrWorkerId) || (!isClient && b.WorkerId.ToString() == clientOrWorkerId))
        {
            AddInclude(b => b.Client);
            AddInclude(b => b.Worker);
            AddInclude(b => b.Task);
            AddInclude(b => b.Payment!);
            AddOrderByDescending(b => b.CreatedAt);
        }
    }
}
