using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOA_CA2.Models
{
	public class Like
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Like_ID { get; set; }

		[Required]
		[ForeignKey("Post")] 
		public int Post_ID { get; set; } 

		[Required]
		[ForeignKey("User")]
		public int User_ID { get; set; } 

		[Column(TypeName = "timestamp")]
		public DateTime Created_At { get; set; } = DateTime.UtcNow; 
	}
}
