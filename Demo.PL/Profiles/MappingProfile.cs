using AutoMapper;
using Demo.BLL.Dtos.Departments;
using Demo.PL.ViewModels.Departments;


namespace Demo.PL.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DepartmentViewModel, DepartmentToCreateDto>();
                //.ReverseMap()
                //.ForMember(dest => dest.Name, config => config.MapFrom(src => src.Name));

        }
            
    }
}
