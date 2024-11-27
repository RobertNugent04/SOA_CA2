namespace SOA_CA2.Models
{
	public class PostDTO
	{
		public int Post_ID { get; set; }
		public int User_ID { get; set; }
		public required string Content { get; set; }
		public string? Image_URL { get; set; }
		public DateTime Created_At { get; set; }
	}

}
