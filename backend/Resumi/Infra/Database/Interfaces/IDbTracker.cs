using Resumi.App.Data.Models;

namespace Resumi.Infra.Database.Interfaces;

/// <summary>
/// Este ccontrato oferece APIs para rastreamento de operações de banco de dados.
/// </summary>
public interface IDbTracker
{
    /// <summary>
    /// Efetua o commit das operações pendentes no banco de dados.
    /// Atualizando propriedades de rastreamento impostas por <see cref="ITrackable"/>
    /// </summary>
    /// <returns>Retorna true se o commit foi bem-sucedido; caso contrário, false.</returns>
    Task<bool> CommitAsync();
}
