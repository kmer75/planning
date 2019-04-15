using PlanningApi.DTO;
using Repositories.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PlanningApi.ViewModel;

namespace PlanningApi.Helpers
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            #region DTO
            CreateMap<User, UserDto>().ReverseMap();
            /*
            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ReverseMap();
            */
            #endregion

            #region ViewModel
            CreateMap<RegisterViewModel, User>();
            #endregion

        }
    }
}
