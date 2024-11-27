namespace SOA_CA2.Models.DTOs.Message
{
	// Used for creating a new message
	public class MessageForCreationDto
	{
		public int Receiver_ID { get; set; }
		public required string Content { get; set; }
	}
}
