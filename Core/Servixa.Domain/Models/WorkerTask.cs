using Servixa.Domain.Models.Users;
using Servixa.Domain.Models.TaskEntity;

namespace Servixa.Domain.Models
{
    public class WorkerTask : BaseEntity<int>
    {
        public int WorkerId { get; set; }
        public virtual Worker Worker { get; set; } = null!;

        public int TaskId { get; set; }
        public virtual TaskEntity.Task Task { get; set; } = null!;

        public decimal CustomPrice { get; set; }
    }
}
