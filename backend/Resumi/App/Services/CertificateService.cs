using Microsoft.EntityFrameworkCore;
using Resumi.Api.Data.Models;
using Resumi.Domain.Models;
using Resumi.Domain.Validators.Interfaces;
using Resumi.Infra.Data.Models;
using Resumi.Infra.Data.Projections;
using Resumi.Infra.Database.Context;

namespace Resumi.App.Services;

public class CertificateService(
    IDomainValidator<ResumeNode> validator,
    ILogger<CertificateService> logger,
    AppDbContext dbContext)
{
    public async Task<Result<CertificateModel>> CreateAsync(Certificate? newEntity)
    {
        try
        {
            var validationResult = validator.ValidateCreation(newEntity);

            if (!validationResult.Succeeded)
            {
                return Result<CertificateModel>.Failure(validationResult);
            }

            var creationResult = await dbContext.Certificates.AddAsync(newEntity!);
            var changedEntries = await dbContext.SaveChangesAsync();

            if (creationResult.State is not EntityState.Added || changedEntries is 0)
            {
                return Result<CertificateModel>.Failure(nameof(Certificate), Certificate.FailedToCreate);
            }

            var createdEntity = await dbContext.Certificates
                .AsNoTracking()
                .Select(CertificateProjections.Basic)
                .FirstOrDefaultAsync(c => c.Id == creationResult.Entity.Id);

            return Result<CertificateModel>.Success(createdEntity!);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create certificate: {Message}", ex.Message);

            return Result<CertificateModel>.Failure(nameof(Certificate), Certificate.InternalError);
        }
    }

    public async Task<Result<CertificateModel>> UpdateAsync(Certificate? current, Certificate? updated)
    {
        try
        {
            var validationResult = validator.ValidateUpdate(current, updated);

            if (!validationResult.Succeeded)
                return Result<CertificateModel>.Failure(validationResult);

            var updateResult = dbContext.Certificates.Update(updated!);
            var changedEntries = await dbContext.SaveChangesAsync();

            if (updateResult.State is not EntityState.Modified || changedEntries is 0)
            {
                return Result<CertificateModel>.Failure(nameof(Certificate), Certificate.FailedToUpdate);
            }

            var updatedEntity = await dbContext.Certificates
                .AsNoTracking()
                .Select(CertificateProjections.Basic)
                .FirstOrDefaultAsync(c => c.Id == updateResult.Entity.Id);

            return Result<CertificateModel>.Success(updatedEntity!);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update certificate: {Message}", ex.Message);

            return Result<CertificateModel>.Failure(nameof(Certificate), Certificate.InternalError);
        }
    }

    public async Task<Result> DeleteAsync(int id)
    {
        try
        {
            var target = await dbContext.Certificates.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

            if (target is null)
            {
                return Result.Failure(nameof(Certificate), Certificate.NotFound);
            }

            var removalResult = dbContext.Certificates.Remove(target);
            var changedEntries = await dbContext.SaveChangesAsync();

            if (removalResult.State is not EntityState.Deleted || changedEntries is 0)
            {
                return Result.Failure(nameof(Certificate), Certificate.FailedToDelete);
            }

            return Result.Success;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete certificate: {Message}", ex.Message);

            return Result.Failure(nameof(Certificate), Certificate.InternalError);
        }
    }
}
