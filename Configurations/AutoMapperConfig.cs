using AutoMapper;
using CollegeApp.Models;
using CollegeApp.Data;

namespace CollegeApp.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig() 
        {
            CreateMap<StudentDTO, Student>()
                .ReverseMap()
                .ForMember(n => n.Name, opt => opt.MapFrom(x => x.StudentName))
                .ForMember(n => n.Email, opt => opt.MapFrom(n => string.IsNullOrEmpty(n.Email) ? "Email not found!" : n.Email))
                .ForMember(n => n.Address, opt => opt.MapFrom(n => string.IsNullOrEmpty(n.Address) ? "Address not found!" : n.Address));
        }   
    }
}
