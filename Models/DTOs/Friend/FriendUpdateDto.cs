﻿namespace SOA_CA2.Models.DTOs.Friend
{
	// Used for updating a friendship status (e.g., Accept, Reject)
	public class FriendForUpdateDto
	{
		public required string Status { get; set; } 
	}
}
