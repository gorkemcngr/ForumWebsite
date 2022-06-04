using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
             CreateMap<RegisterDto, AppUser>();
             CreateMap<Post,PostDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => 
                    src.AppUser.UserName.ToString()))
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
                    src.AppUser.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => 
                    src.Category.CategoryName.ToString()));

            CreateMap<Comment,CommentDto>()
                .ForMember(dest => dest.PhotoUrl,opt => opt.MapFrom(src =>
                src.AppUser.Photos.FirstOrDefault(p => p.IsMain).Url))
            .ReverseMap();
            CreateMap<MemberUpdateDto,AppUser>();
            CreateMap<AppUser,UserDto>();
            CreateMap<AppUser,MemberDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => 
                    src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(src => 
                    src.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.RecipientPhotoUrl, opt => opt.MapFrom(src => 
                    src.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));
            CreateMap<AppUser,UserRolesDto>().ReverseMap();
            CreateMap<Category, CategoryDto>();
            CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
        }
    }
}