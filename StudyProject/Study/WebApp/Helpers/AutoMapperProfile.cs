using AutoMapper;

namespace WebApp.Helpers;

public class AutoMapperProfile: Profile
{
    public AutoMapperProfile()
    {
        CreateMap<App.Domain.Homework, App.DTO.v1_0.Homework>().ReverseMap();
        CreateMap<App.Domain.Semester, App.DTO.v1_0.Semester>().ReverseMap();
        CreateMap<App.Domain.Subject, App.DTO.v1_0.Subject>().ReverseMap();
        CreateMap<App.Domain.UserHomework, App.DTO.v1_0.UserHomework>().ReverseMap();
        CreateMap<App.Domain.UserSemester, App.DTO.v1_0.UserSemester>().ReverseMap();
        CreateMap<App.Domain.UserSubject, App.DTO.v1_0.UserSubject>().ReverseMap();
    }
}