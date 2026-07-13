using Servixa.Domain.Contracts;
using Servixa.Domain.Models.TaskEntity;

namespace Servixa.Domain.Specifications
{
    public class TaskWithSpecialtySpecification : BaseSpecification<Servixa.Domain.Models.TaskEntity.Task>
    {
        public TaskWithSpecialtySpecification() : base()
        {
            AddInclude(t => t.Specialty);
        }

        public TaskWithSpecialtySpecification(int id) : base(t => t.Id == id)
        {
            AddInclude(t => t.Specialty);
        }
        
        public TaskWithSpecialtySpecification(int specialtyId, bool isBySpecialty) : base(t => t.SpecialtyId == specialtyId)
        {
            AddInclude(t => t.Specialty);
        }
    }
}
