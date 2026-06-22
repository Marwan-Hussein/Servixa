using Servixa.Domain.Contracts;
using Servixa.Domain.Models.Users;

namespace Servixa.Domain.Specifications
{
    public class WorkerWithDetailsSpecification : BaseSpecification<Worker>
    {
        public WorkerWithDetailsSpecification() : base()
        {
            AddInclude(w => w.WorkerTasks);
            AddInclude(w => w.ReceivedReviews);
        }

        public WorkerWithDetailsSpecification(int id) : base(w => w.Id == id)
        {
            AddInclude(w => w.WorkerTasks);
            AddInclude(w => w.ReceivedReviews);
        }

        public WorkerWithDetailsSpecification(int specialtyId, bool forSpecialty) 
            : base(w => w.WorkerTasks.Any(wt => wt.Task.SpecialtyId == specialtyId))
        {
            AddInclude(w => w.WorkerTasks);
        }
    }
}
