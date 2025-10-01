using System;
using Entity.Dtos;

namespace Business.Interfaces;

public interface INotificationDispatcher
{
    Task SendAsync(int parkingId, NotificationDto notification);
}
