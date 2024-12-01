using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOA_CA2.Models
{
	public class Message
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Message_ID { get; set; }

		[Required]
		[ForeignKey("User")] 
		public int Sender_User_ID { get; set; } 

		[Required]
		[ForeignKey("User")] 
		public int Receiver_User_ID { get; set; } 

		[Required]
		[Column(TypeName = "text")]
		public required string Content { get; set; } 

		[Column(TypeName = "timestamp")]
		public DateTime Sent_At { get; set; } = DateTime.UtcNow; 

	}
}
