using System;
using Entity.Models;

namespace Data.Interfaces;

public interface INotificationData : IRepositoryData<Notification>
{
    Task<IEnumerable<Notification>> GetByParkingAsync(int parkingId, bool onlyUnread = false);
}
