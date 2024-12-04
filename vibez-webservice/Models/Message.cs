using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOA_CA2.Models
{
    /// <summary>
    /// Represents a message exchanged between two users in the Vibez application.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Primary key for the Message entity.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; }

        /// <summary>
        /// Foreign key referencing the sender of the message.
        /// </summary>
        [Required]
        [ForeignKey("Sender")]
        public int SenderId { get; set; }

        /// <summary>
        /// Navigation property for the sender of the message.
        /// </summary>
        public User Sender { get; set; }

        /// <summary>
        /// Foreign key referencing the receiver of the message.
        /// </summary>
        [Required]
        [ForeignKey("Receiver")]
        public int ReceiverId { get; set; }

        /// <summary>
        /// Navigation property for the receiver of the message.
        /// </summary>
        public User Receiver { get; set; }

        /// <summary>
        /// The content of the message.
        /// </summary>
        [Required]
        [Column(TypeName = "text")]
        public required string Content { get; set; }

        /// <summary>
        /// Indicates whether the message has been deleted by the sender.
        /// </summary>
        public bool IsDeletedBySender { get; set; } = false;

        /// <summary>
        /// Indicates whether the message has been deleted by the receiver.
        /// </summary>
        public bool IsDeletedByReceiver { get; set; } = false;

        /// <summary>
        /// The timestamp when the message was created - UTC.
        /// </summary>
        [Column(TypeName = "timestamptz")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
