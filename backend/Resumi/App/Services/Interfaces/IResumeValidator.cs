using Resumi.App.Data.Models;
using Resumi.Infra.Data.Models;

namespace Resumi.App.Services.Interfaces;

/// <summary>
/// Este contrato fornece APIs para validação de operações de domínio
/// em entidades <see cref="Resume"/>.
/// </summary>
public interface IResumeValidator
{
	/// <summary>
	/// Valida o estado da operação de criação do currículo <paramref name="newResume"/>.
	/// </summary>
	/// <param name="newResume">
	///	O currículo a ser validado para cadastro.
	/// </param>
	/// <returns>
	///	Uma instância de <see cref="Result{T}"/>,
	/// contendo o resultado da validação.
	/// </returns>
	Result<Resume> ValidateCreation(Resume? newResume);

	/// <summary>
	/// Valida o estado da operação de busca do currículo <paramref name="targetResume"/>.
	/// </summary>
	/// <param name="targetResume">
	///	O currículo a ser validado para busca.
	/// </param>
	/// <returns>
	///	Uma instância de <see cref="Result{T}"/>,
	/// contendo o resultado da validação.
	/// </returns>
	Result<Resume> ValidateSearch(Resume? targetResume);

	/// <summary>
	/// Valida o estado da operação de atualização do currículo <paramref name="current"/> com os dados <paramref name="updated"/>.
	/// </summary>
	/// <param name="current">
	///	O currículo atual a ser validado para atualização.
	/// </param>
	/// <param name="updated">
	///	O currículo com os dados atualizados a ser validado para atualização.
	/// </param>
	/// <returns>
	///	Uma instância de <see cref="Result{T}"/>,
	/// contendo o resultado da validação.
	/// </returns>
	Result<Resume> ValidateUpdate(Resume? current, Resume? updated);

	/// <summary>
	/// Valida o estado da operação de exclusão do currículo <paramref name="targetResume"/>.
	/// </summary>
	/// <param name="targetResume">
	///	O currículo a ser validado para exclusão.
	/// </param>
	/// <returns>
	///	Uma instância de <see cref="Result{T}"/>,
	/// contendo o resultado da validação.
	/// </returns>
	Result<Resume> ValidateDeletion(Resume? targetResume);
}