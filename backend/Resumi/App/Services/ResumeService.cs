using Resumi.App.Data.Models;
using Resumi.App.Services.Interfaces;
using Resumi.Infra.Data.Models;
using Resumi.Infra.Database.Context;
using Resumi.Infra.Database.Interfaces;

namespace Resumi.App.Services;

public class ResumeService : IResumeService
{
    private readonly IDomainValidator<Resume> _validator;
    private readonly IRepository<Resume> _repository;
    private readonly AppDbContext _dbContext;

    public ResumeService(
        IDomainValidator<Resume> validator, 
        IRepository<Resume> repository, 
        AppDbContext dbContext)
    {
        _validator = validator;
        _repository = repository;
        _dbContext = dbContext;
    }

    public async Task<Result<Resume>> CreateAsync(Resume? newResume)
    {
        var validationResult = _validator.ValidateCreation(newResume);

        if (!validationResult.Succeeded)
        {
            return Result<Resume>.Failure(validationResult.Errors);
        }

        var createdResume = await _repository.AddAsync(newResume!);
        
        if (createdResume is null)
        {
            return Result<Resume>.Failure(nameof(Resume), "Não foi possível criar o currículo.");
        }

        await _dbContext.SaveChangesAsync();

        return Result<Resume>.Success(createdResume);
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

    public Task<Result<IEnumerable<Resume>>> FindAllAsync(int skip = 0, int take = 20)
    {
        throw new NotImplementedException();
    }
}