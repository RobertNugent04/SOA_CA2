﻿namespace SOA_CA2.Models.DTOs.Comment
{
	//Used for creating a comment
	public class CommentCreationDto
	{
		public required string Content { get; set; }

		public int Post_ID { get; set; } 
	}
}