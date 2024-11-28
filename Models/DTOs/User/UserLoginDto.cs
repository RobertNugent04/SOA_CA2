namespace SOA_CA2.Models.DTOs.User
{
    // Login DTO
    public class UserLoginDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
