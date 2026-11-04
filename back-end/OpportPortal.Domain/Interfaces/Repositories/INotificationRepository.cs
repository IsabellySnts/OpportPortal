using OpportPortal.Domain.Entities;

namespace OpportPortal.Domain.Interfaces.Repositories;

public interface INotificationRepository
{
    Task<long> Create(Notification notification);
    Task<List<Notification>> GetByUserIdAsync(long userId);
    Task<int> GetUnreadCountAsync(long userId);
    Task<Notification?> GetByIdAsync(long id);
    Task Update(Notification notification);
    Task MarkAllAsReadAsync(long userId);
}
