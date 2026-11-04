using OpportPortal.Domain.Enums;

namespace OpportPortal.Domain.Entities;

public class Notification : BaseEntity
{
    public long UserId { get; private set; }
    public string Title { get; private set; }
    public string Message { get; private set; }
    public NotificationType Type { get; private set; }
    public bool IsRead { get; private set; }

    private Notification() { }

    public Notification(long userId, string title, string message, NotificationType type) : base()
    {
        UserId = userId;
        Title = title;
        Message = message;
        Type = type;
        IsRead = false;
    }

    public void MarkAsRead()
    {
        IsRead = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
