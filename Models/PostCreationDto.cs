namespace SOA_CA2.Models
{
	//Used when creating a post
	public class PostForCreationDto
	{
		public required string Content { get; set; }
		public string? Image_URL { get; set; }
	}
}
