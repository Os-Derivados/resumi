using Resumi.Domain.Validators.Interfaces;
using Resumi.Infra.Data.Models;
using Resumi.Infra.Database.Context;
using Resumi.Infra.Database.Interfaces;

namespace Resumi.App.Services;

public class DegreeService : IDegreeService
{
    private readonly IDomainValidator<Degree> _validator;
    private readonly IRepository<Degree> _repository;
    private readonly AppDbContext _dbContext;

    public DegreeService(
        IDomainValidator<Degree> validator,
        IRepository<Degree> repository,
        AppDbContext dbContext)
    {
        _validator = validator;
        _repository = repository;
        _dbContext = dbContext;
    }

    public async Task<Result<Degree>> CreateAsync(Degree? newEntity)
    {
        var validationResult = _validator.ValidateCreation(newEntity);

        if (!validationResult.Succeeded)
        {
            return Result<Degree>.Failure(validationResult.Errors);
        }

        var createdDegree = await _repository.AddAsync(newEntity!);

        if (createdDegree is null)
        {
            return Result<Degree>.Failure(nameof(Degree), "Não foi possível criar a formação acadêmica.");
        }

        await _dbContext.SaveChangesAsync();

        return Result<Degree>.Success(createdDegree);
    }

    public async Task<Result<Degree>> FindAsync(int id)
    {
        if (id <= 0)
        {
            return Result<Degree>.Failure(nameof(Degree), "ID inválido para busca de formação acadêmica.");
        }

        var degree = await _repository.GetByIdAsync(id);

        if (degree is null)
        {
            return Result<Degree>.Failure(nameof(Degree), "Formação acadêmica não encontrada.");
        }

        return Result<Degree>.Success(degree);
    }

    public async Task<Result<IEnumerable<Degree>>> FindAllAsync(int skip = 0, int take = 20)
    {
        if (skip < 0 || take <= 0 || take > 100)
        {
            return Result<IEnumerable<Degree>>.Failure(
                nameof(Degree),
                "Parâmetros de paginação inválidos. Skip deve ser >= 0 e Take deve estar entre 1 e 100.");
        }

        var degrees = await _repository.GetAllAsync(skip, take);

        if (degrees is null)
        {
            return Result<IEnumerable<Degree>>.Failure(
                nameof(Degree),
                "Não foi possível recuperar as formações acadêmicas.");
        }

        return Result<IEnumerable<Degree>>.Success(degrees);
    }

    public async Task<Result<Degree>> UpdateAsync(Degree? current, Degree? updated)
    {
        var validationResult = _validator.ValidateUpdate(current, updated);

        if (!validationResult.Succeeded)
        {
            return Result<Degree>.Failure(validationResult.Errors);
        }

        var updatedDegree = await _repository.UpdateAsync(updated!);

        if (updatedDegree is null)
        {
            return Result<Degree>.Failure(
                nameof(Degree),
                "Não foi possível atualizar a formação acadêmica.");
        }

        await _dbContext.SaveChangesAsync();

        return Result<Degree>.Success(updatedDegree);
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        if (id <= 0)
        {
            return Result<bool>.Failure(nameof(Degree), "ID inválido para exclusão de formação acadêmica.");
        }

        var deleted = await _repository.DeleteAsync(id);

        if (!deleted)
        {
            return Result<bool>.Failure(
                nameof(Degree),
                "Não foi possível deletar a formação acadêmica.");
        }

        await _dbContext.SaveChangesAsync();

        return Result<bool>.Success(true);
    }
}
