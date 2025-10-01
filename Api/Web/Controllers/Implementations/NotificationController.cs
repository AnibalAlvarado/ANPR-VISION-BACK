using System;
using Business.Interfaces;
using Entity.Dtos;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Interfaces;

namespace Web.Controllers.Implementations;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : RepositoryController<Notification, NotificationDto>
{
    private readonly INotificationBusiness _business;

    public NotificationController(INotificationBusiness business) : base(business)
    {
        _business = business;
    }

    [HttpGet("by-parking/{parkingId}")]
    public async Task<ActionResult<IEnumerable<NotificationDto>>> GetByParking(int parkingId, [FromQuery] bool onlyUnread = false)
    {
        try
        {
            var data = await _business.GetByParkingAsync(parkingId, onlyUnread);

            if (data == null || !data.Any())
            {
                var responseNull = new ApiResponse<IEnumerable<NotificationDto>>(null, false, "Registro no encontrado", null);
                return NotFound(responseNull);
            }

            var response = new ApiResponse<IEnumerable<NotificationDto>>(data, true, "Ok", null);
            return Ok(response);
        }
        catch (Exception ex)
        {
            var response = new ApiResponse<IEnumerable<NotificationDto>>(null, false, ex.Message.ToString(), null);
            return StatusCode(StatusCodes.Status500InternalServerError, response);
        }
    }
}