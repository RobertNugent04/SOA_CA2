namespace SOA_CA2.Models.DTOs.Call
{
    /// <summary>
    /// DTO used for transferring call data to the client.
    /// </summary>
    public class CallDto
    {
        /// <summary>
        /// The unique identifier of the call.
        /// </summary>
        public int CallId { get; set; }

        /// <summary>
        /// The ID of the user who initiated the call.
        /// </summary>
        public int CallerId { get; set; }

        /// <summary>
        /// The ID of the user who received the call.
        /// </summary>
        public int ReceiverId { get; set; }

        /// <summary>
        /// The type of the call (Voice or Video).
        /// </summary>
        public required string CallType { get; set; }

        /// <summary>
        /// The status of the call (Initiated, Accepted, Rejected, Missed, Ended).
        /// </summary>
        public required string CallStatus { get; set; }

        /// <summary>
        /// The timestamp when the call started - UTC.
        /// </summary>
        public DateTime? StartedAt { get; set; }

        /// <summary>
        /// The timestamp when the call ended - UTC.
        /// </summary>
        public DateTime? EndedAt { get; set; }

        /// <summary>
        /// Duration of the call in seconds.
        /// </summary>
        public int? Duration { get; set; }
    }
}
