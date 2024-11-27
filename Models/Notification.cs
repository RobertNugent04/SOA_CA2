using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOA_CA2.Models
{
	public class Notification
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
		public int Notification_ID { get; set; }

		[Required]
		[ForeignKey("User")] 
		public int User_ID { get; set; } 

		[Required]
		[StringLength(255)] 
		public required string Message { get; set; } 

		[Column(TypeName = "timestamp")]
		public DateTime Created_At { get; set; } = DateTime.UtcNow; 

	}
}
