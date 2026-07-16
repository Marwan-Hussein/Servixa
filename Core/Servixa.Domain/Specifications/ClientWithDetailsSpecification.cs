using Servixa.Domain.Contracts;
using Servixa.Domain.Models.Users;

namespace Servixa.Domain.Specifications
{
    public class ClientWithDetailsSpecification : BaseSpecification<Client>
    {
        public ClientWithDetailsSpecification() : base(c => !c.IsDeleted)
        {
        }

        public ClientWithDetailsSpecification(int id) : base(c => c.Id == id && !c.IsDeleted)
        {
        }
    }
}
