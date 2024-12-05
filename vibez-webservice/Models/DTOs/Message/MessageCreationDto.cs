namespace SOA_CA2.Models.DTOs.Message
{
    /// <summary>
    /// DTO used for creating a new message.
    /// </summary>
    public class MessageCreationDto
    {
        /// <summary>
        /// The ID of the user who will receive the message.
        /// </summary>
        public int ReceiverId { get; set; }

        /// <summary>
        /// The content of the message.
        /// </summary>
        public required string Content { get; set; }
    }
}
