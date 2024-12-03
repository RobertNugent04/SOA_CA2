namespace SOA_CA2.Models.DTOs.Notification
{
    /// <summary>
    /// DTO used for creating a new notification.
    /// </summary>
    public class NotificationCreationDto
    {
        public int UserId { get; set; }
        public required string Type { get; set; }
        public int? ReferenceId { get; set; }
        public required string Message { get; set; }
    }
}
