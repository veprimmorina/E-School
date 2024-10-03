using AutoMapper;
using Core.DTO;

namespace Core.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() { 
            CreateMap<InputModel, StudentDto>().ReverseMap();
        }
    }
}
