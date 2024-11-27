namespace SOA_CA2.Models.DTOs.Like
{
	// Standard read DTO for Like
	public class LikeDto
	{
		public int Like_ID { get; set; }
		public DateTime Liked_At { get; set; }
		public int User_ID { get; set; }  
	}
}
