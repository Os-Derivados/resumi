using Resumi.Domain.Models;
using Resumi.Infra.Data.Models;

namespace Resumi.Domain.Validators.Interfaces;

/// <summary>
/// Este contrato fornece APIs para efetuar a validação de operações básicas de escrita no domínio da aplicação.
/// </summary>
/// <typeparam name="TEntity">
/// Uma entidade <see cref="ITrackable"/> que pertença ao domínio da aplicação.
/// </typeparam>
/// <remarks>
/// Validações somente-leitura não devem ser feitas em implementações desta API, pois enquanto validações de escrita validam INVARIANTES de domínio, validações somente-leitura validam ESTADO de domínio.
/// </remarks>
public interface IDomainValidator<in TEntity> where TEntity : ITrackable
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
    Result ValidateCreation(TEntity? newEntity);


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
    Result ValidateUpdate(TEntity? current, TEntity? updated);

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
    Result ValidateDeletion(TEntity? targetEntity);
}
