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
    private readonly INotificationDispatcher _dispatcher;
    public NotificationBusiness(INotificationData data, IMapper mapper, ILogger<NotificationBusiness> logger, INotificationDispatcher dispatcher)
        : base(data, mapper)
    {
        _data = data;
        _mapper = mapper;
        _logger = logger;
        _dispatcher = dispatcher;
    }

    public async Task<List<NotificationDto>> GetByParkingAsync(int parkingId, bool onlyUnread = false)
    {
        var entities = await _data.GetByParkingAsync(parkingId, onlyUnread);
        return _mapper.Map<List<NotificationDto>>(entities);
    }

    public async Task MarkAsReadAsync(int id)
    {
        await _data.MarkAsReadAsync(id);
    }

    public async Task<NotificationDto> CreateAndNotifyAsync(NotificationDto dto)
    {
        var entity = _mapper.Map<Notification>(dto);
        entity.CreatedAt = DateTime.UtcNow;
        entity.IsRead = false;

        await _data.Save(entity);

        var savedDto = _mapper.Map<NotificationDto>(entity);

        await _dispatcher.SendAsync(entity.ParkingId, savedDto);

        return savedDto;
    }

}
