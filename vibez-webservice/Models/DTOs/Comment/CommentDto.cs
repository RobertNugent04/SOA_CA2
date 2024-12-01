using SOA_CA2.Models.DTOs.User;

namespace SOA_CA2.Models.DTOs.Comment
{
	// Standard read DTO for Comment
	public class CommentDto
	{
		public int Comment_ID { get; set; }
		public required string Content { get; set; }
		public DateTime Created_At { get; set; }
		public int User_ID { get; set; }
	}
}
