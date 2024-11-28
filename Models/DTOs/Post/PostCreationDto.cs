namespace SOA_CA2.Models.DTOs.Post
{
    //Used when creating a post
    public class PostCreationDto
    {
        public required string Content { get; set; }
        public string? Image_URL { get; set; }
    }
}
