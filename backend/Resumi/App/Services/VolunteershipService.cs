using Resumi.Api.Data.Models;
using Resumi.Domain.Models;
using Resumi.Infra.Data.Models;

namespace Resumi.App.Services;

public class VolunteershipService
{
    public Task<Result<VolunteershipModel>> CreateAsync(Volunteership? newEntity)
    {
        throw new NotImplementedException();
    }

    public Task<Result<VolunteershipModel>> FindAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<List<VolunteershipModel>>> FindAllAsync(int resumeId, int skip = 0, int take = 20)
    {
        throw new NotImplementedException();
    }

    public Task<Result<VolunteershipModel>> UpdateAsync(Volunteership? current, Volunteership? updated)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}