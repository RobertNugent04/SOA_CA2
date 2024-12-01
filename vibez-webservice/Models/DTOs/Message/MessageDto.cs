using System.ComponentModel.DataAnnotations.Schema;
using SOA_CA2.Models.DTOs.User;

namespace SOA_CA2.Models.DTOs.Message
{
	// Standard read DTO for Message
	public class MessageDto
	{
		public int Message_ID { get; set; }
		public int Sender_User_ID { get; set; }
		public int Receiver_User_ID { get; set; }
		public required string Content { get; set; }
		public DateTime Sent_At { get; set; } = DateTime.UtcNow;
	}
}
