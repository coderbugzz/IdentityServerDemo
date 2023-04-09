using API.Models;
using AutoMapper;
using DataAccess.Entities;

namespace API.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserModel, User>().ReverseMap();
        }
    }
}
