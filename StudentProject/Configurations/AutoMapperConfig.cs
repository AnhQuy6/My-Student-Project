using AutoMapper;
using StudentProject.Data;
using StudentProject.Models;

namespace StudentProject.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Student, StudentDTO>().ReverseMap();
        }
    }
}
