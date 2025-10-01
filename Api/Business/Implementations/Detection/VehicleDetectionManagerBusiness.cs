using System;
using Business.Interfaces.Detection;
using Entity.Records;
using Microsoft.Extensions.Logging;

namespace Business.Implementations.Detection;

public class VehicleDetectionManagerBusiness : IVehicleDetectionManagerBusiness
{
    private readonly ILogger<VehicleDetectionManagerBusiness> _logger;
    public VehicleDetectionManagerBusiness(ILogger<VehicleDetectionManagerBusiness> logger)
    {
        _logger = logger;
    }

    public async Task ProcessDetectionAsync(PlateDetectedEventRecord evt, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Procesando Detección");

        await Task.Delay(100, cancellationToken);
    }
}
