using AutoMapper;
using System.Linq;
using Zap.BLL.DTO;
using Zap.DAL.Entities;

namespace Zap.BLL.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // === USER ===
            CreateMap<User, UserShortDTO>();

            CreateMap<User, UserDTO>()
                .ForMember(d => d.Followers, o => o.MapFrom(s => s.Followers.Select(uf => uf.Follower)))
                .ForMember(d => d.Following, o => o.MapFrom(s => s.Following.Select(uf => uf.Followed)));

            CreateMap<UserDTO, User>()
                .ForMember(d => d.Followers, o => o.Ignore())
                .ForMember(d => d.Following, o => o.Ignore())
                .ForMember(d => d.Comments, o => o.Ignore());

            // === MEDIA ATTACHMENTS ===
            CreateMap<MediaAttachment, MediaAttachmentDTO>()
                .ReverseMap()
                .ForMember(dest => dest.Post, opt => opt.Ignore())
                .ForMember(dest => dest.Comment, opt => opt.Ignore());

            // === COMMENTS ===
            CreateMap<Comment, CommentDTO>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author != null ? src.Author.Username : string.Empty))
                .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments))
                .ForMember(dest => dest.LikesCount, opt => opt.MapFrom(src => src.CommentLikes != null ? src.CommentLikes.Count : 0))
                .ForMember(dest => dest.IsLikedByCurrentUser, opt => opt.Ignore()); // вычисляется в сервисе

            CreateMap<CommentDTO, Comment>()
                .ForMember(dest => dest.Author, opt => opt.Ignore())
                .ForMember(dest => dest.Attachments, opt => opt.Ignore())
                .ForMember(dest => dest.CommentLikes, opt => opt.Ignore());

            // === POSTS ===
            CreateMap<Post, PostDTO>()
                .ForMember(dest => dest.AuthorUsername, opt => opt.MapFrom(src => src.Author != null ? src.Author.Username : string.Empty))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments))
                .ForMember(dest => dest.LikesCount, opt => opt.MapFrom(src => src.PostLikes != null ? src.PostLikes.Count : 0))
                .ForMember(dest => dest.IsLikedByCurrentUser, opt => opt.Ignore()); // вычисляется в сервисе

            CreateMap<PostDTO, Post>()
                .ForMember(dest => dest.Author, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.Attachments, opt => opt.Ignore())
                .ForMember(dest => dest.PostLikes, opt => opt.Ignore());

            // === LIKES ===
            CreateMap<PostLike, PostLikeDTO>()
                .ForMember(d => d.ID, o => o.MapFrom(s => s.Id))
                .ReverseMap()
                .ForMember(d => d.Post, o => o.Ignore())
                .ForMember(d => d.User, o => o.Ignore());

            CreateMap<CommentLike, CommentLikeDTO>()
                .ForMember(d => d.ID, o => o.MapFrom(s => s.Id))
                .ReverseMap()
                .ForMember(d => d.Comment, o => o.Ignore())
                .ForMember(d => d.User, o => o.Ignore());
        }
    }
}
