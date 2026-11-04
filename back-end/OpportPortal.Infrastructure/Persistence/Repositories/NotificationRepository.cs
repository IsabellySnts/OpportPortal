using Microsoft.EntityFrameworkCore;
using OpportPortal.Domain.Entities;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Infrastructure.Persistence.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly OpportPortalDbContext _context;
    public NotificationRepository(OpportPortalDbContext context) => _context = context;

    public async Task<long> Create(Notification notification)
    {
        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();
        return notification.Id;
    }

    public async Task<List<Notification>> GetByUserIdAsync(long userId)
        => await _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Take(50)
            .ToListAsync();

    public async Task<int> GetUnreadCountAsync(long userId)
        => await _context.Notifications.CountAsync(n => n.UserId == userId && !n.IsRead);

    public async Task<Notification?> GetByIdAsync(long id)
        => await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);

    public async Task Update(Notification notification)
    {
        _context.Notifications.Update(notification);
        await _context.SaveChangesAsync();
    }

    public async Task MarkAllAsReadAsync(long userId)
    {
        var unread = await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead).ToListAsync();
        foreach (var n in unread) n.MarkAsRead();
        await _context.SaveChangesAsync();
    }
}
