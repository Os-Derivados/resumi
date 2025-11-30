using Resumi.App.Data.Models;

namespace Resumi.Infra.Database.Interfaces;

/// <summary>
/// Este contrato oferece APIs genéricas
/// para operações CRUD em entidades primárias no banco de dados.
/// </summary>
/// <typeparam name="TEntity">
/// Tipo da entidade primária.
/// </typeparam>
public interface IRepository<TEntity>
    where TEntity : Entity
{
    /// <summary>
    /// Obtém uma entidade primária pelo seu identificador.
    /// </summary>
    /// <param name="id">Identificador da entidade.</param>
    /// <returns>A entidade primária correspondente ao identificador, ou nulo se não encontrada.</returns>
    Task<TEntity?> GetByIdAsync(int id);

    /// <summary>
    /// Obtém todas as entidades primárias com paginação.
    /// </summary>
    /// <param name="skip">Número de entidades a serem ignoradas.</param>
    /// <param name="take">Número máximo de entidades a serem retornadas.</param>
    /// <returns>Uma coleção de entidades primárias.</returns>
    Task<IEnumerable<TEntity>?> GetAllAsync(int skip = 0, int take = 100);

    /// <summary>
    /// Adiciona uma nova entidade primária ao banco de dados.
    /// </summary>
    /// <param name="entity">A entidade primária a ser adicionada.</param>
    /// <returns>A entidade primária adicionada, ou nulo se a operação falhar.</returns>
    Task<TEntity?> AddAsync(TEntity entity);

    /// <summary>
    /// Atualiza uma entidade primária existente no banco de dados.
    /// </summary>
    /// <param name="entity">A entidade primária a ser atualizada.</param>
    /// <returns>A entidade primária atualizada, ou nulo se a operação falhar.</returns>
    Task<TEntity?> UpdateAsync(TEntity entity);

    /// <summary>
    /// Remove uma entidade primária do banco de dados pelo seu identificador.
    /// </summary>
    /// <param name="id">Identificador da entidade a ser removida.</param>
    /// <returns>Verdadeiro se a entidade foi removida com sucesso, caso contrário, falso.</returns>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Verifica se uma entidade primária existe no banco de dados.
    /// </summary>
    /// <param name="entity">A entidade primária a ser verificada.</param>
    /// <returns>Verdadeiro se a entidade existe, caso contrário, falso.</returns>
    Task<bool> ExistsAsync(TEntity entity);
}
