namespace SOA_CA2.Models.DTOs.Call
{
    /// <summary>
    /// DTO used for updating the status of a call.
    /// </summary>
    public class CallStatusUpdateDto
    {
        /// <summary>
        /// The updated status of the call (Accepted, Rejected, Missed, Ended).
        /// </summary>
        public required string CallStatus { get; set; }

        /// <summary>
        /// The timestamp when the call started.
        /// </summary>
        public DateTime? StartedAt { get; set; }

        /// <summary>
        /// The timestamp when the call ended.
        /// </summary>
        public DateTime? EndedAt { get; set; }
    }
}
