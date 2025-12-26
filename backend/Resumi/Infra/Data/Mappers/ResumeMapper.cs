using Resumi.Api.Data.Models;
using Resumi.App.Data.Models;
using Resumi.Infra.Data.Interfaces;

namespace Resumi.Infra.Data.Mappers;

public class ResumeMapper : IResumeMapper
{
    public Resume? NewDomainModel(CreateResumeModel? dtoCreate)
    {
        throw new NotImplementedException();
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