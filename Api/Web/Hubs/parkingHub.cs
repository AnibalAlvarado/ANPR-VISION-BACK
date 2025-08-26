using Microsoft.AspNetCore.SignalR;

namespace Web.Hubs
{
    public class parkingHub : Hub
    {
        // Método que notifica cuando se crea un vehículo
        //public async Task NotificarVehiculoCreado(string placa, string tipo)
        //{
        //    // Se lo envía a todos los clientes conectados
        //    await Clients.All.SendAsync("VehiculoCreado", placa, tipo);
        //}
    }
}
