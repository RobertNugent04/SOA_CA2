namespace SOA_CA2.Models.DTOs.Message
{
    /// <summary>
    /// DTO used for updating an existing message.
    /// </summary>
    public class MessageUpdateDto
    {
        /// <summary>
        /// The updated content of the message.
        /// </summary>
        public string? Content { get; set; }
    }
}
