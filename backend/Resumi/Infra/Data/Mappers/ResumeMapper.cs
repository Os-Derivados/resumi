using Resumi.Api.Data.Models;
using Resumi.App.Data.Models;
using Resumi.Infra.Data.Interfaces;

namespace Resumi.Infra.Data.Mappers;

public class ResumeMapper : IResumeMapper
{
    public Resume? NewDomainModel(CreateResumeModel dtoCreate)
    {
        return new Resume
        {
            Title = dtoCreate.Title,
            UserId = dtoCreate.UserId
        };
    }

    public Resume? UpdatedDomainModel(UpdateResumeModel? dtoUpdate, Resume? entity)
    {
        throw new NotImplementedException();
    }

    public ResumeModel? ToDto(Resume? entity)
    {
        throw new NotImplementedException();
    }
}