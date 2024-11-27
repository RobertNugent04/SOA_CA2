namespace SOA_CA2.Models.DTOs.Friend
{
	// Standard read DTO for Friend relationship
	public class FriendDto
	{
		public int Friend_ID { get; set; }
		public int User_ID { get; set; }  
		public int Friend_User_ID { get; set; }  
		public required string Status { get; set; }
	}
}
