using Resumi.App.Data.Models;
using Resumi.Infra.Data.Models;

namespace Resumi.App.Services.Interfaces;

public interface IDomainValidator<TEntity> where TEntity : ITrackable
{
    /// <summary>
    /// Valida o estado da operação de criação da entidade <paramref name="newEntity"/>.
    /// </summary>
    /// <param name="newEntity">
    ///	A entidade a ser validada para cadastro.
    /// </param>
    /// <returns>
    ///	Uma instância de <see cref="Result{T}"/>,
    /// contendo o resultado da validação.
    /// </returns>
    Result<TEntity> ValidateCreation(TEntity? newEntity);

    /// <summary>
    /// Valida o estado da operação de busca da entidade <paramref name="targetEntity"/>.
    /// </summary>
    /// <param name="targetEntity">
    ///	A entidade a ser validada para busca.
    /// </param>
    /// <returns>
    ///	Uma instância de <see cref="Result{T}"/>,
    /// contendo o resultado da validação.
    /// </returns>
    Result<TEntity> ValidateSearch(TEntity? targetEntity);

    /// <summary>
    /// Valida o estado da operação de atualização de <paramref name="current"/> com os dados <paramref name="updated"/>.
    /// </summary>
    /// <param name="current">
    ///	A entidade atual, a ser validado para atualização.
    /// </param>
    /// <param name="updated">
    ///	A entidade com os dados atualizados, a ser validado para atualização.
    /// </param>
    /// <returns>
    ///	Uma instância de <see cref="Result{T}"/>,
    /// contendo o resultado da validação.
    /// </returns>
    Result<TEntity> ValidateUpdate(TEntity? current, TEntity? updated);

    /// <summary>
    /// Valida o estado da operação de exclusão da entidade <paramref name="targetEntity"/>.
    /// </summary>
    /// <param name="targetEntity">
    ///	A entidade a ser validado para exclusão.
    /// </param>
    /// <returns>
    ///	Uma instância de <see cref="Result{T}"/>,
    /// contendo o resultado da validação.
    /// </returns>
    Result<TEntity> ValidateDeletion(TEntity? targetEntity);
}
