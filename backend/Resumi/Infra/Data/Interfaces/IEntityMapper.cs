using Resumi.App.Data.Models;

namespace Resumi.Infra.Data.Interfaces;

/// <summary>
/// Este contrato oferece APIs para mapear entidades de domínio para DTOs e vice-versa.
/// </summary>
/// <typeparam name="TEntity">
/// Uma entidade de domínio que implementa <see cref="ITrackable"/>.
/// </typeparam>
/// <typeparam name="TDto">
/// Um DTO (Data Transfer Object).
/// </typeparam>
public interface IEntityMapper<TEntity, out TDto, in TCreaate, in TUpdate> where TEntity : ITrackable
{
  /// <summary>
  /// Cria uma nova entidade de domínio a partir de um modelo de criação.
  /// </summary>
  /// <param name="createModel">
  /// Objeto de parâmetro de criação.
  /// </param>
  /// <returns>
  /// A instância da entidade de domínio criada.
  /// </returns>
  TEntity? NewDomainModel(TCreaate? createModel);

  /// <summary>
  /// Atualiza uma entidade de domínio existente a partir de um modelo de atualização.
  /// </summary>
  /// <param name="updateModel">
  /// Objeto de parâmetro de atualização.
  /// </param>
  /// <param name="entity">
  /// A entidade de domínio a ser atualizada.
  /// </param>
  /// <returns>
  /// A instância da entidade de domínio atualizada.
  /// </returns>
  TEntity? UpdatedDomainModel(TUpdate? updateModel, TEntity? entity);

  /// <summary>
  /// Converte uma entidade de domínio em um DTO (Data Transfer Object).
  /// </summary>
  /// <param name="entity">A entidade de domínio a ser convertida.</param>
  /// <returns>O DTO correspondente à entidade de domínio.</returns>
  TDto? ToDto(TEntity? entity);
}
