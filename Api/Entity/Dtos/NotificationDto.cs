using Entity.Dtos;

namespace Entity.Dtos;

public class NotificationDto : BaseDto
{
    public Guid ParkingId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = "Info";
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
    public int? RelatedEntityId { get; set; }
}
