using System;
using AutoMapper;
using Data.Interfaces;
using Entity.Contexts;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Utilities.Audit.Services;
using Utilities.Interfaces;

namespace Data.Implementations;

public class NotificationData : RepositoryData<Notification>, INotificationData
{

    public NotificationData(ApplicationDbContext context, IConfiguration configuration, IAuditService auditService, ICurrentUserService currentUserService, IMapper mapper) : base(context, configuration, auditService, currentUserService, mapper)
    {

    }

    public async Task<IEnumerable<Notification>> GetByParkingAsync(int parkingId, bool onlyUnread = false)
    {
        var query = _context.Notifications.AsQueryable()
            .Where(n => n.ParkingId == parkingId);

        if (onlyUnread)
            query = query.Where(n => !n.IsRead);

        return await query.OrderByDescending(n => n.CreatedAt).ToListAsync();
    }
}
