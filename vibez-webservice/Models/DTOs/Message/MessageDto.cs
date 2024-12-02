namespace SOA_CA2.Models.DTOs.Message
{
    /// <summary>
    /// DTO used for transferring message data to the client.
    /// </summary>
    public class MessageDto
    {
        /// <summary>
        /// The unique identifier of the message.
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        /// The ID of the user who sent the message.
        /// </summary>
        public int SenderId { get; set; }

        /// <summary>
        /// The ID of the user who received the message.
        /// </summary>
        public int ReceiverId { get; set; }

        /// <summary>
        /// The content of the message.
        /// </summary>
        public required string Content { get; set; }

        /// <summary>
        /// Indicates whether the message has been deleted by the sender.
        /// </summary>
        public bool IsDeletedBySender { get; set; }

        /// <summary>
        /// Indicates whether the message has been deleted by the receiver.
        /// </summary>
        public bool IsDeletedByReceiver { get; set; }

        /// <summary>
        /// The timestamp when the message was created - UTC.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
