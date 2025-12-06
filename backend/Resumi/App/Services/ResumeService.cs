using Resumi.App.Data.Models;
using Resumi.App.Services.Interfaces;
using Resumi.Infra.Data.Models;

namespace Resumi.App.Services;

public class ResumeService : IResumeService
{
    public Task<Result<Resume>> CreateAsync(Resume? newResume)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Resume>> FindAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Resume>>> FindByUserAsync(int userId, int skip = 0, int take = 20)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Resume>> UpdateAsync(Resume? current, Resume? updated)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}