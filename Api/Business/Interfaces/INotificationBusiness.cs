using System;
using Entity.Dtos;
using Entity.Models;

namespace Business.Interfaces;

public interface INotificationBusiness : IRepositoryBusiness<Notification, NotificationDto>
{
    Task<List<NotificationDto>> GetByParkingAsync(int parkingId, bool onlyUnread = false);
    Task MarkAsReadAsync(int id);
    Task<NotificationDto> CreateAndNotifyAsync(NotificationDto dto);
}
