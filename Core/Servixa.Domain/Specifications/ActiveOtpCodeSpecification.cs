using Servixa.Domain.Contracts;
using Servixa.Domain.Models.OtpEntity;

namespace Servixa.Domain.Specifications
{
    public class ActiveOtpCodeSpecification : BaseSpecification<OtpCode>
    {
        public ActiveOtpCodeSpecification(int userId, string purpose, DateTime now)
            : base(o => o.UserId == userId && o.Purpose == purpose && !o.IsUsed && o.ExpiresAt > now)
        {
            AddOrderByDescending(o => o.CreatedAt);
        }
    }
}
