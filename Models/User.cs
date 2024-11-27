using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//Referenced Article: https://github.com/mahedee/articles/blob/master/dot-net-core/HowToCreateWebAPIinASP.NETCOrewitMySQL.md

namespace SOA_CA2.Models
{
	public class User
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int User_ID { get; set; }

		[Required]
		[Column(TypeName = "nvarchar(150)")]
		[StringLength(150, MinimumLength = 2)]
		public string Full_Name { get; set; }

		[Required]
		[Column(TypeName = "varchar(50)")]
		[StringLength(50, MinimumLength = 3)]
		public string Username { get; set; }

		[Required]
		[EmailAddress]
		[Column(TypeName = "varchar(100)")]
		[StringLength(100)]
		public string Email { get; set; }

		[Required]
		[Column(TypeName = "varchar(255)")]
		public string Password { get; set; }

		[Column(TypeName = "nvarchar(500)")]
		[StringLength(500)]
		public string? Bio { get; set; }

		[Column(TypeName = "varchar(255)")]
		[StringLength(255)]
		public string? Profile_Pic { get; set; }

		[Column(TypeName = "datetime")]
		public DateTime Created_At { get; set; } = DateTime.UtcNow;

		[Column(TypeName = "datetime")]
		public DateTime? Updated_At { get; set; }
	}
}