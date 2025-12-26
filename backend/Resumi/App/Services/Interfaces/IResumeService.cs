using Resumi.App.Data.Models;
using Resumi.Infra.Data.Models;

namespace Resumi.App.Services.Interfaces;

/// <summary>
/// Este contrato fornece APIs de alto nível para operações de domínio relacionadas
/// a entidades <see cref="Resume"/>.
/// </summary>
public interface IResumeService : IDomainService<Resume>
{
	/// <summary>
	/// Busca currículos pelo identificador do usuário.
	/// </summary>
	/// <param name="userId">O identificador do usuário.</param>
	/// <param name="skip">Número de currículos a pular.</param>
	/// <param name="take">Número máximo de currículos a retornar.</param>
	/// <returns>
	/// Uma <see cref="Task"/>, representando uma instância de <see cref="Result{T}"/>,
	/// contendo o resultado da operação.
	/// </returns>
	Task<Result<IEnumerable<Resume>>> FindByUserAsync(int userId, int skip = 0, int take = 20);
}