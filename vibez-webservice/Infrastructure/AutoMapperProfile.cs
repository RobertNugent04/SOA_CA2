using AutoMapper;
using SOA_CA2.Models;
using SOA_CA2.Models.DTOs.User;
using SOA_CA2.Models.DTOs.Post;
using SOA_CA2.Models.DTOs.Comment;
using SOA_CA2.Models.DTOs.Like;
using SOA_CA2.Models.DTOs.Friendship;
using SOA_CA2.Models.DTOs.Message;
using SOA_CA2.Models.DTOs.Notification;
using SOA_CA2.Models.DTOs.Call;

namespace SOA_CA2.Infrastructure
{
    /// <summary>
    /// AutoMapper profile to configure mappings for all DTOs and entities in the system.
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // **User Mappings**
            CreateMap<UserCreationDto, User>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Bio, opt => opt.Ignore())
                .ForMember(dest => dest.ProfilePicturePath, opt => opt.Ignore());

            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.ProfilePicturePath, opt => opt.MapFrom(src => src.ProfilePicturePath));

            CreateMap<UserUpdateDto, User>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ProfilePicturePath, opt => opt.Ignore());

            // **Post Mappings**
            CreateMap<PostCreationDto, Post>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<Post, PostDTO>();

            CreateMap<PostUpdateDto, Post>();

            CreateMap<Post, PostDTO>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

            // **Comment Mappings**
            CreateMap<CommentCreationDto, Comment>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<Comment, CommentDto>();

            CreateMap<CommentUpdateDto, Comment>();

            // **Like Mappings**
            CreateMap<Like, LikeDto>();

            CreateMap<LikeCreationDto, Like>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // **Friendship Mappings**
            CreateMap<FriendshipCreationDto, Friendship>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Pending"));

            CreateMap<Friendship, FriendshipDto>();

            CreateMap<FriendshipUpdateDto, Friendship>();

            // **Message Mappings**
            CreateMap<MessageCreationDto, Message>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsDeletedBySender, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.IsDeletedByReceiver, opt => opt.MapFrom(src => false));

            CreateMap<Message, MessageDto>();

            CreateMap<MessageUpdateDto, Message>();

            // **Notification Mappings**
            CreateMap<NotificationCreationDto, Notification>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsRead, opt => opt.MapFrom(src => false));

            CreateMap<Notification, NotificationDto>();

            // **Call Mappings**
            CreateMap<CallCreationDto, Call>()
                .ForMember(dest => dest.StartedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CallStatus, opt => opt.MapFrom(src => "Initiated"));

            CreateMap<Call, CallDto>();

            CreateMap<CallStatusUpdateDto, Call>()
                .ForMember(dest => dest.StartedAt, opt => opt.MapFrom(src => src.StartedAt))
                .ForMember(dest => dest.EndedAt, opt => opt.MapFrom(src => src.EndedAt));
        }
    }
}
