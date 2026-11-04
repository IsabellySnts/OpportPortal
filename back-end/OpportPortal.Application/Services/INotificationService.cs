using OpportPortal.Domain.Enums;

namespace OpportPortal.Application.Services;

public record NotificationResponse(long Id, string Title, string Message, int Type, bool IsRead, DateTime CreatedAt);

public interface INotificationService
{
    Task NotifyAsync(long userId, string title, string message, NotificationType type);
    Task<List<NotificationResponse>> GetMyAsync(long userId);
    Task<int> GetUnreadCountAsync(long userId);
    Task MarkAsReadAsync(long id, long userId);
    Task MarkAllAsReadAsync(long userId);
}
