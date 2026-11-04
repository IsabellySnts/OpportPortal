using OpportPortal.Domain.Entities;
using OpportPortal.Domain.Enums;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Application.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repository;
    public NotificationService(INotificationRepository repository) => _repository = repository;

    public async Task NotifyAsync(long userId, string title, string message, NotificationType type)
    {
        var notification = new Notification(userId, title, message, type);
        await _repository.Create(notification);
    }

    public async Task<List<NotificationResponse>> GetMyAsync(long userId)
    {
        var items = await _repository.GetByUserIdAsync(userId);
        return items.Select(n => new NotificationResponse(
            n.Id, n.Title, n.Message, (int)n.Type, n.IsRead, n.CreatedAt)).ToList();
    }

    public Task<int> GetUnreadCountAsync(long userId) => _repository.GetUnreadCountAsync(userId);

    public async Task MarkAsReadAsync(long id, long userId)
    {
        var n = await _repository.GetByIdAsync(id);
        if (n is null || n.UserId != userId) return;
        n.MarkAsRead();
        await _repository.Update(n);
    }

    public Task MarkAllAsReadAsync(long userId) => _repository.MarkAllAsReadAsync(userId);
}
