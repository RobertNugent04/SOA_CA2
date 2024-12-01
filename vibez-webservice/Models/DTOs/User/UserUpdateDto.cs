namespace SOA_CA2.Models.DTOs.User
{
    // Used for updating an existing user
    public class UserUpdateDto
    {
        public string? Full_Name { get; set; }
        public string? Bio { get; set; }
        public string? Profile_Pic { get; set; }
    }
}
