using AutoMapper;
using Resumi.App.Data.Models;
using Resumi.App.DTOs.Resume;
using Resumi.App.DTOs.Experience;
using Resumi.App.DTOs.AcademicDegree;

namespace Resumi.App.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapeamentos do Resume
        CreateMap<Resume, ResumeResponseDto>()
            .ReverseMap();
        
        CreateMap<CreateResumeDto, Resume>();
        CreateMap<UpdateResumeDto, Resume>();

        // Mapeamentos do Experience
        CreateMap<Experience, ExperienceResponseDto>()
            .ReverseMap();
        
        CreateMap<CreateExperienceDto, Experience>();
        CreateMap<UpdateExperienceDto, Experience>();

        // Mapeamentos do AcademicDegree
        CreateMap<AcademicDegree, AcademicDegreeResponseDto>()
            .ReverseMap();
        
        CreateMap<CreateAcademicDegreeDto, AcademicDegree>();
        CreateMap<UpdateAcademicDegreeDto, AcademicDegree>();
    }
}
