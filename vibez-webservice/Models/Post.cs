using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOA_CA2.Models
{
	public class Post
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Post_ID { get; set; }

		[Required]
		[ForeignKey("User")]
		public int User_ID { get; set; }

		[Required]
		[Column(TypeName = "text")] 
		public required string Content { get; set; }

		[Column(TypeName = "varchar(255)")] 
		[StringLength(255)]
		public string? Image_URL { get; set; }

		[Column(TypeName = "timestamp")] 
		public DateTime Created_At { get; set; } = DateTime.UtcNow;

		[Column(TypeName = "timestamp")] 
		public DateTime? Updated_At { get; set; }

	}
}
