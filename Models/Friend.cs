using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOA_CA2.Models
{
	public class Friend
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
		public int Friend_ID { get; set; }

		[Required]
		[ForeignKey("User")]
		public int User_ID { get; set; } 

		[Required]
		[ForeignKey("User")]
		public int Friend_User_ID { get; set; }

		[Required]
		[StringLength(50)] 
		public string Status { get; set; } 

		[Column(TypeName = "timestamp")]
		public DateTime Created_At { get; set; } = DateTime.UtcNow; 
	}
}
