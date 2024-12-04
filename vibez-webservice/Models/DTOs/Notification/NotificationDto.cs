namespace SOA_CA2.Models.DTOs.Notification
{
    /// <summary>
    /// DTO used for transferring notification data to the client.
    /// </summary>
    public class NotificationDto
    {
        public int NotificationId { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public required string Type { get; set; }
        public int? ReferenceId { get; set; }
        public required string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
