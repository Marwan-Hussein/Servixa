using Servixa.Domain.Models.SpecialtyEntity;
using System.Collections.Generic;

namespace Servixa.Domain.Models.TaskEntity
{
    public class Task : BaseEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public decimal AvgCost { get; set; }
        public DateTime AvgTime { get; set; }

        public int SpecialtyId { get; set; }
        public virtual Specialty Specialty { get; set; } = null!;

        public virtual ICollection<WorkerTask> WorkerTasks { get; set; } = new HashSet<WorkerTask>();
    }
}
