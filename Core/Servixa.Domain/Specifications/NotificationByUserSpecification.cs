using Servixa.Domain.Contracts;
using Servixa.Domain.Models.NotificationEntity;

namespace Servixa.Domain.Specifications
{
    public class NotificationByUserSpecification : BaseSpecification<Notification>
    {
        public NotificationByUserSpecification(int userId) 
            : base(n => n.ReceiverId == userId)
        {
            AddOrderByDescending(n => n.CreatedAt);
        }
        
        public NotificationByUserSpecification(int userId, bool unreadOnly) 
            : base(n => n.ReceiverId == userId && (!unreadOnly || !n.IsRead))
        {
            AddOrderByDescending(n => n.CreatedAt);
        }
    }
}
