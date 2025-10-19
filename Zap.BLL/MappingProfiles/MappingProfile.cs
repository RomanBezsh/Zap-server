using AutoMapper;
using Zap.BLL.DTO;
using Zap.DAL.Entities;

namespace Zap.BLL.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();

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