using Resumi.App.Data.Models;
using Resumi.Infra.Data.Models;

namespace Resumi.App.Services.Interfaces;

/// <summary>
/// Este contrato fornece APIs de alto nível para operações de domínio relacionadas
/// a entidades <see cref="Resume"/>.
/// </summary>
public interface IResumeService
{
	/// <summary>
	/// Cria um novo currículo.
	/// </summary>
	/// <param name="newResume">O currículo a ser criado.</param>
	/// <returns>O currículo criado.</returns>
	Task<Result<Resume>> CreateAsync(Resume? newResume);

	/// <summary>
	/// Busca um currículo pelo seu identificador.
	/// </summary>
	/// <param name="id">
	///	O identificador do currículo a ser buscado.
	/// </param>
	/// <returns></returns>
	Task<Result<Resume>> FindAsync(int id);

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

	/// <summary>
	/// Atualiza um currículo existente.
	/// </summary>
	/// <param name="current">
	///	O currículo atual a ser atualizado.
	/// </param>
	/// <param name="updated">O currículo com as informações atualizadas.</param>
	/// <returns>
	///	Uma <see cref="Task"/>, representando uma instância de <see cref="Result{T}"/>,
	///	contendo o resultado da operação.
	/// </returns>
	Task<Result<Resume>> UpdateAsync(Resume? current, Resume? updated);

	/// <summary>
	/// Exclui um currículo pelo seu identificador.
	/// </summary>
	/// <param name="id">O identificador do currículo a ser excluído.</param>
	/// <returns>
	///	Uma <see cref="Task"/>, representando uma instância de <see cref="Result{T}"/>,
	///	contendo o resultado da operação.
	/// </returns>
	Task<Result<bool>> DeleteAsync(int id);
}