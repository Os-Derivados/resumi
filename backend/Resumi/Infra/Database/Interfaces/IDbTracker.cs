namespace Resumi.Infra.Database.Interfaces;

/// <summary>
/// Este ccontrato oferece APIs para rastreamento de operações de banco de dados.
/// </summary>
public interface IDbTracker
{
    /// <summary>
    /// Efetua o commit das operações pendentes no banco de dados.
    /// </summary>
    /// <returns>Retorna true se o commit foi bem-sucedido; caso contrário, false.</returns>
    Task<bool> CommitAsync();
}
