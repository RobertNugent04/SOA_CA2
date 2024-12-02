namespace SOA_CA2.Models.DTOs.Call
{
    /// <summary>
    /// DTO used for creating a new call.
    /// </summary>
    public class CallCreationDto
    {
        /// <summary>
        /// The ID of the user who will receive the call.
        /// </summary>
        public int ReceiverId { get; set; }

        /// <summary>
        /// The type of the call (Voice or Video).
        /// </summary>
        public required string CallType { get; set; }
    }
}
