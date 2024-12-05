using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOA_CA2.Models
{
    /// <summary>
    /// Represents a call (voice or video) between two users in the Vibez application.
    /// </summary>
    public class Call
    {
        /// <summary>
        /// Primary key for the Call entity.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CallId { get; set; }

        /// <summary>
        /// Foreign key referencing the User who initiated the call.
        /// </summary>
        [Required]
        [ForeignKey("User")]
        public int CallerId { get; set; }

        /// <summary>
        /// Navigation property for the User who initiated the call.
        /// </summary>
        public User Caller { get; set; }

        /// <summary>
        /// Foreign key referencing the User who received the call.
        /// </summary>
        [Required]
        [ForeignKey("User")]
        public int ReceiverId { get; set; }

        /// <summary>
        /// Navigation property for the User who received the call.
        /// </summary>
        public User Receiver { get; set; }

        /// <summary>
        /// The type of the call (Voice or Video).
        /// </summary>
        [Required]
        [StringLength(10)]
        public required string CallType { get; set; }

        /// <summary>
        /// The status of the call (Initiated, Accepted, Rejected, Missed, Ended).
        /// </summary>
        [Required]
        [StringLength(15)]
        public required string CallStatus { get; set; }

        /// <summary>
        /// The timestamp when the call started - UTC.
        /// </summary>
        [Column(TypeName = "timestamptz")]
        public DateTime? StartedAt { get; set; }

        /// <summary>
        /// The timestamp when the call ended - UTC.
        /// </summary>
        [Column(TypeName = "timestamptz")]
        public DateTime? EndedAt { get; set; }

        /// <summary>
        /// Duration of the call in seconds.
        /// </summary>
        public int? Duration { get; set; }
    }
}
