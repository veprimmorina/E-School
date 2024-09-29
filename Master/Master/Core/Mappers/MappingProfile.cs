using AutoMapper;
using Master.Core.DTO;
using Master.Core.DTO.ML;
using Master.Core.DTO.ML.FinalModel;

namespace Master.Core.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<object, StudentsGradeResponseDto>();
            CreateMap<StudentPredictionDto, FinalInputModel>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null && !IsDefaultValue(srcMember)));
        }

        private bool IsDefaultValue(object value)
        {
            if (value == null) return true;

            var type = value.GetType();
            if (!type.IsValueType) return false;

            return value.Equals(Activator.CreateInstance(type));
        }
    }
}
