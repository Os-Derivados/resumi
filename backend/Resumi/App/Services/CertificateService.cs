using Microsoft.EntityFrameworkCore;
using Resumi.App.Services.Interfaces;
using Resumi.Domain.Validators.Interfaces;
using Resumi.Infra.Data.Models;
using Resumi.Infra.Database.Context;
using Resumi.Infra.Database.Interfaces;

namespace Resumi.App.Services;

public class CertificateService : ICertificateService
{
    private readonly IDomainValidator<Certificate> _validator;
    private readonly IRepository<Certificate> _repository;
    private readonly AppDbContext _dbContext;

    public CertificateService(
        IDomainValidator<Certificate> validator,
        IRepository<Certificate> repository,
        AppDbContext dbContext)
    {
        _validator = validator;
        _repository = repository;
        _dbContext = dbContext;
    }

    public async Task<Result<Certificate>> CreateAsync(Certificate? newEntity)
    {
        var validation = _validator.ValidateCreation(newEntity);
        if (!validation.Succeeded)
            return Result<Certificate>.Failure(validation.Errors);

        var added = await _repository.AddAsync(newEntity!);
        if (added is null)
            return Result<Certificate>.Failure("Certificate", "Failed to add certificate.");

        await _dbContext.CommitAsync();
        return Result<Certificate>.Success(added);
    }

    public async Task<Result<Certificate>> FindAsync(int id)
    {
        var certificate = await _repository.GetByIdAsync(id);
        if (certificate is null)
            return Result<Certificate>.Failure("id", "Certificate not found.");

        return Result<Certificate>.Success(certificate);
    }

    public async Task<Result<IEnumerable<Certificate>>> FindAllAsync(int skip = 0, int take = 20)
    {
        var certificates = await _dbContext.Certificates.AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        return Result<IEnumerable<Certificate>>.Success(certificates);
    }

    public async Task<Result<Certificate>> UpdateAsync(Certificate? current, Certificate? updated)
    {
        if (current is null || updated is null)
            return Result<Certificate>.Failure("certificate", "Invalid certificate data.");

        var validation = _validator.ValidateUpdate(current, updated);
        if (!validation.Succeeded)
            return Result<Certificate>.Failure(validation.Errors);

        current.Name = updated.Name ?? current.Name;
        current.Description = updated.Description ?? current.Description;
        current.InstitutionName = updated.InstitutionName ?? current.InstitutionName;
        current.Location = updated.Location ?? current.Location;
        current.IsRemote = updated.IsRemote;
        current.StartDate = updated.StartDate;
        current.EndDate = updated.EndDate;
        current.StillEngaged = updated.StillEngaged;
        current.CredentialId = updated.CredentialId ?? current.CredentialId;
        current.CredentialUrl = updated.CredentialUrl ?? current.CredentialUrl;
        current.Type = updated.Type;

        var updatedEntity = await _repository.UpdateAsync(current);
        if (updatedEntity is null)
            return Result<Certificate>.Failure("certificate", "Failed to update certificate.");

        await _dbContext.CommitAsync();
        return Result<Certificate>.Success(updatedEntity);
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        var exists = await _repository.GetByIdAsync(id);
        if (exists is null)
            return Result<bool>.Failure("id", "Certificate not found.");

        var deleted = await _repository.DeleteAsync(id);
        if (!deleted)
            return Result<bool>.Failure("id", "Could not delete certificate.");

        await _dbContext.CommitAsync();
        return Result<bool>.Success(true);
    }
}
