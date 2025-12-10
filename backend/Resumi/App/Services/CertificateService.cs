using Resumi.App.Data.Models;
using Resumi.App.Services.Interfaces;
using Resumi.Infra.Data.Models;

namespace Resumi.App.Services;

public class CertificateService : ICertificateService
{
    public Task<Result<Certificate>> CreateAsync(Certificate? newEntity)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Certificate>> FindAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Certificate>>> FindAllAsync(int skip = 0, int take = 20)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Certificate>> UpdateAsync(Certificate? current, Certificate? updated)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}
