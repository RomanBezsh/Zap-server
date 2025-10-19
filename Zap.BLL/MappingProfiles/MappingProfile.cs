using AutoMapper;
using Zap.BLL.DTO;
using Zap.DAL.Entities;

namespace Zap.BLL.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Lightweight mapping for follower lists
            CreateMap<User, UserShortDTO>();

            // Full user mapping: map followers/following -> UserShortDTO by projecting through the join entity
            CreateMap<User, UserDTO>()
                .ForMember(d => d.Followers, o => o.MapFrom(s => s.Followers.Select(uf => uf.Follower)))
                .ForMember(d => d.Following, o => o.MapFrom(s => s.Following.Select(uf => uf.Followed)));

            // Reverse: ignore navigation collections to avoid AutoMapper mutating relationships
            CreateMap<UserDTO, User>()
                .ForMember(d => d.Followers, o => o.Ignore())
                .ForMember(d => d.Following, o => o.Ignore())
                .ForMember(d => d.Comments, o => o.Ignore());

            // Other existing mappings
            CreateMap<Comment, CommentDTO>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author != null ? src.Author.Username : string.Empty))
                .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments));

            CreateMap<MediaAttachment, MediaAttachmentDTO>()
                .ReverseMap()
                .ForMember(dest => dest.Post, opt => opt.Ignore())
                .ForMember(dest => dest.Comment, opt => opt.Ignore());

            CreateMap<Post, PostDTO>()
                .ForMember(dest => dest.AuthorUsername, opt => opt.MapFrom(src => src.Author != null ? src.Author.Username : string.Empty))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments));
        }
    }
}