using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Contexts.parking;
using Microsoft.AspNetCore.Http;

namespace Utilities.Middleware
{
    public class ParkingContextMiddleware
    {
        private readonly RequestDelegate _next;

        public ParkingContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IParkingContext parkingContext)
        {
            if (context.Request.Headers.TryGetValue("X-Parking-Id", out var parkingIdHeader))
            {
                if (int.TryParse(parkingIdHeader, out var parkingId))
                {
                    (parkingContext as ParkingContext)?.SetParkingId(parkingId);
                }
            }

            await _next(context);
        }
    }

}
