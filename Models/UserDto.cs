namespace SOA_CA2.Models
{
	public class UserDTO
	{
		public int User_ID { get; set; }
		public required string Full_Name { get; set; }
		public required string Username { get; set; }
		public required string Email { get; set; }
		public string? Bio { get; set; }
		public string? Profile_Pic { get; set; }
		public DateTime Created_At { get; set; }
	}
}
