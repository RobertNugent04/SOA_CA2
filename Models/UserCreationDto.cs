using System.ComponentModel.DataAnnotations;

namespace SOA_CA2.Models
{
	// Used for creating a new user
	public class UserCreationDto
	{
		public required string Full_Name { get; set; }

		public required string Username { get; set; }

		public required string Email { get; set; }

		public required string Password { get; set; }
	}
}
