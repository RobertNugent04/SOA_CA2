using SOA_CA2.Models.DTOs.Post;
using SOA_CA2.Models.DTOs.Friendship;

namespace SOA_CA2.Models.DTOs.User
{
    public class UserProfileDto
    {
        public UserDTO User { get; set; }
        public IEnumerable<PostDTO> Posts { get; set; }
        public IEnumerable<UserDTO> Friends { get; set; }
    }
}
