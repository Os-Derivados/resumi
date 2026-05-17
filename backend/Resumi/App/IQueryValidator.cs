using Resumi.Infra.Data.Models;

namespace Resumi.App;

/// <summary>
/// Este contrato fornece APIs para a validação de operações somente-leitura sem exigir o carregamento completo de entidades de domínio.
/// </summary>
/// <remarks>Este contrato não deve ser implementado em classes que validem operações de escrita, pois estas exigem o carregamento da entidade completa para garantir invariantes de domínio.</remarks>
public interface IQueryValidator
{
	/// <summary>
	/// Valida o estado de uma operação de busca por uma entidade em particular.
	/// </summary>
	/// <param name="id">O identificador da entidade.</param>
	/// <returns>Uma <see cref="Task"/>, contendo o resultado da operação.</returns>
	Task<Result> ValidateSearch(int id);
}