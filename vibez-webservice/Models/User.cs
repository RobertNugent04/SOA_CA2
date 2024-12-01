using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOA_CA2.Models
{
	public class User
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int User_ID { get; set; }

		[Required]
		[Column(TypeName = "varchar(150)")] 
		[StringLength(150, MinimumLength = 2)]
		public required string Full_Name { get; set; }

		[Required]
		[Column(TypeName = "varchar(50)")] 
		[StringLength(50, MinimumLength = 3)]
		public required string Username { get; set; }

		[Required]
		[EmailAddress]
		[Column(TypeName = "varchar(100)")] 
		[StringLength(100)]
		public required string Email { get; set; }

		[Required]
		[Column(TypeName = "varchar(255)")] 
		public required string Password { get; set; }

		[Column(TypeName = "text")]
		[StringLength(500)] 
		public string? Bio { get; set; }

		[Column(TypeName = "varchar(255)")] 
		[StringLength(255)]
		public string? Profile_Pic { get; set; }

		[Column(TypeName = "timestamptz")] 
		public DateTime Created_At { get; set; } = DateTime.UtcNow;

		[Column(TypeName = "timestamptz")] 
		public DateTime? Updated_At { get; set; }
	}
}
