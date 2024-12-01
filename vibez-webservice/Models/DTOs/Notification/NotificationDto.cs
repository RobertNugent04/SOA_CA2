namespace SOA_CA2.Models.DTOs.Notification
{
	// Standard read DTO for Notification
	public class NotificationDto
	{
		public int Notification_ID { get; set; }
		public required string Message { get; set; }
		public DateTime Created_At { get; set; } = DateTime.UtcNow;
	}
}
