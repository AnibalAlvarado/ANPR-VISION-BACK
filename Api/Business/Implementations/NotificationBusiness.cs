using System;
using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos;
using Entity.Models;
using Microsoft.Extensions.Logging;

namespace Business.Implementations;

public class NotificationBusiness : RepositoryBusiness<Notification, NotificationDto>, INotificationBusiness
{
    private readonly INotificationData _data;
    private readonly IMapper _mapper;
    private readonly ILogger<NotificationBusiness> _logger;
    public NotificationBusiness(INotificationData data, IMapper mapper, ILogger<NotificationBusiness> logger)
        : base(data, mapper)
    {
        _data = data;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<NotificationDto>> GetByParkingAsync(int parkingId, bool onlyUnread = false)
    {
        var entities = await _data.GetByParkingAsync(parkingId, onlyUnread);
        return _mapper.Map<List<NotificationDto>>(entities);
    }

}
