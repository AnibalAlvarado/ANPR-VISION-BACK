using System;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Business.Interfaces;
using Business.Interfaces.Detection;
using Microsoft.Extensions.Configuration;
using Entity.Records;
using System.Text.Json;

namespace Infrastructure.Kafka;

public class KafkaConsumerService : BackgroundService
{
    private readonly ILogger<KafkaConsumerService> _logger;
    private readonly IConsumer<string, string> _consumer;
    private readonly IVehicleDetectionManagerBusiness _business;
    private readonly IConfiguration _configuration;
    private readonly string _topic;
        
    // cacheo de las opciones JSON
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
    public KafkaConsumerService(ILogger<KafkaConsumerService> logger, IConsumer<string, string> consumer, IVehicleDetectionManagerBusiness business, IConfiguration configuration)
    {
        _logger = logger;
        _consumer = consumer;
        _business = business;

        _configuration = configuration;

        _topic = _configuration["Kafka:Topic"] ?? throw new InvalidOperationException("Kafka:Topic is missing");
    }

    /// <summary>
    /// Bucle principal de consumo de Kafka
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _consumer.Subscribe(_topic);

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var result = _consumer.Consume(cancellationToken);
                if (result?.Message?.Value is not null)
                {
                    _logger.LogInformation("Kafka => Mensaje recibido {Value}", result.Message.Value);

                    // Se usan las opciones cacheadas
                    PlateDetectedEventRecord? evt = JsonSerializer.Deserialize<PlateDetectedEventRecord>(
                        result.Message.Value,
                        _jsonOptions
                    );
                    if (evt is not null)
                    {
                        await _business.ProcessDetectionAsync(evt, cancellationToken);
                    }
                    else
                    {
                        _logger.LogWarning(" No se pudo deserializar el mensaje de Kafka: {Value}", result.Message.Value);
                    }
                }
            }
            catch (ConsumeException ex)
            {
                _logger.LogError(ex, "Error consumiendo mensaje de Kafka");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en Kafka consumer");
            }

        }
    }

    /// <summary>
    /// Liberar el consumer cuando el servicio se detiene
    /// </summary>
    public override void Dispose()
    {
        _consumer.Close();
        _consumer.Dispose();
        base.Dispose();
        GC.SuppressFinalize(this); // 👈 se agrega esto
    }
}


    
