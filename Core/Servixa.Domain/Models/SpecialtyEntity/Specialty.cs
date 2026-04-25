using Servixa.Domain.Models.TaskEntity;
using System.Collections.Generic;

namespace Servixa.Domain.Models.SpecialtyEntity
{
    public class Specialty : BaseEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        
        public virtual ICollection<TaskEntity.Task> Tasks { get; set; } = new HashSet<TaskEntity.Task>();
    }
}
