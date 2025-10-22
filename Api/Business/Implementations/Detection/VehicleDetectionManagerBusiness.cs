using Business.Interfaces;
using Business.Interfaces.Detection;
using Business.Interfaces.Operational;
using Business.Interfaces.Parameter;
using Data.Implementations.Operational;
using Entity.Dtos.Operational;
using Entity.Models;
using Entity.Records;
using Microsoft.Extensions.Logging;
using System;
using Utilities.BackgroundTasks;
using Utilities.Interfaces;

namespace Business.Implementations.Detection;

public class VehicleDetectionManagerBusiness : IVehicleDetectionManagerBusiness
{
    private readonly ILogger<VehicleDetectionManagerBusiness> _logger;
    private readonly IVehicleBusiness _vehicleBusiness;
    private readonly IRegisteredVehicleBusiness _registeredVehicleBusiness;
    private readonly INotificationBusiness _notificationBusiness;
    private readonly IBlackListBusiness _blackListBusiness;
    private readonly ITypeVehicleBusiness _typeVehicleBusiness;
    private readonly IBackgroundTaskQueue _taskQueue;

    public VehicleDetectionManagerBusiness(ILogger<VehicleDetectionManagerBusiness> logger, IVehicleBusiness vehicleBusiness , IRegisteredVehicleBusiness registeredVehicleBusiness, INotificationBusiness notificationBusiness, IBlackListBusiness blackListBusiness, ITypeVehicleBusiness typeVehicleBusiness, IBackgroundTaskQueue taskQueue)
    {
        _logger = logger;
        _registeredVehicleBusiness = registeredVehicleBusiness;
        _notificationBusiness = notificationBusiness;
        _blackListBusiness = blackListBusiness;
        _vehicleBusiness = vehicleBusiness;
        _typeVehicleBusiness = typeVehicleBusiness;
        _taskQueue = taskQueue;
    }

    public async Task ProcessDetectionAsync(PlateDetectedEventRecord evt, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Procesando Detección");
        _taskQueue.Enqueue(async token =>
        {
            await _notificationBusiness.CreateAndNotifyAsync(new NotificationDto
            {
                ParkingId = evt.ParkingId ?? 0,
                Title = "Detección iniciada",
                Message = $"Se detectó la placa {evt.Plate}.",
                Type = "Info"
            });
        });

        //validar si existe
        bool exists = await _vehicleBusiness.ExistsAsync(v => v.Plate.ToUpper() == evt.Plate.ToUpper());
        if (!exists)
        {
            await NewVehicleDetection(evt);
        }

        await ExistingVehicleDetection(evt);

        await Task.Delay(100, cancellationToken);
    }

    private async Task NewVehicleDetection(PlateDetectedEventRecord evt)
    {
        int typeVehicle = await _typeVehicleBusiness.GetTypeVehicleByPlate(evt.Plate);
        VehicleDto vehicleDto = new()
        {
            Plate = evt.Plate,
            Color = "",
            TypeVehicleId = typeVehicle,
            ClientId = 3
        };
        VehicleDto vehicleResult = await _vehicleBusiness.Save(vehicleDto);
        RegisteredVehiclesDto entryRegister = await _vehicleBusiness.RegisterVehicleWithSlotAsync(vehicleResult.Id);
        _taskQueue.Enqueue(async token =>
        {
            await _notificationBusiness.CreateAndNotifyAsync(new NotificationDto
            {
                ParkingId = evt.ParkingId ?? 0,
                Title = "Vehículo registrado y entrada creada",
                Message = $"El vehículo con placa {evt.Plate} fue registrado como {vehicleDto.TypeVehicleId}, " +
                          $" se generó su entrada en el parqueadero {evt.ParkingId}." +
                          $"Y se asigno el slot {entryRegister.Slots}.",
                Type = "Success",
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                RelatedEntityId = entryRegister.Id
            });
        });

    }

    private async Task ExistingVehicleDetection(PlateDetectedEventRecord evt)
    {
        VehicleDto existedVehicle = await _vehicleBusiness.GetVehicleByPlate(evt.Plate);
        bool existInBlacklist = await _blackListBusiness.ExistsAsync(b => b.VehicleId == existedVehicle.Id);
        if (existInBlacklist)
        {   
            //_taskQueue.Enqueue(async token =>
            //{
            //    await _notificationBusiness.CreateAndNotifyAsync(new NotificationDto
            //    {
            //        ParkingId = evt.ParkingId ?? 0,
            //        Title = "Vehículo registrado y entrada creada",
            //        Message = $"El vehículo con placa {evt.Plate} fue registrado como {vehicleDto.TypeVehicleId}, " +
            //                  $" se generó su entrada en el parqueadero {evt.ParkingId}." +
            //                  $"Y se asigno el slot {entryRegister.Slots}.",
            //        Type = "Success",
            //        CreatedAt = DateTime.UtcNow,
            //        IsRead = false,
            //        RelatedEntityId = entryRegister.Id
            //    });
            //});
        }

    }
}
