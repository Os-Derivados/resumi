using Resumi.Domain.Models;
using Resumi.Domain.Validators.Interfaces;
using Resumi.Infra.Data.Models;

namespace Resumi.Domain.Validators;

public class DegreeValidator : IDomainValidator<Degree>
{
	private const int MaxNameLength = 128;
	private const int MaxInstitutionNameLength = 128;
	private const int MaxLocationLength = 64;
	private const int MaxDescriptionLength = 256;
	private const int MaxHighlightsLength = 1000;

	public Result<Degree> ValidateCreation(Degree? newDegree)
	{
		ResultErrors errors = [];

		if (newDegree is null)
		{
			errors.AddError(nameof(Degree), "Estado inválido para cadastro de formação.");
			return Result<Degree>.Failure(errors);
		}

		ValidateRequiredFields(newDegree, errors);
		ValidateFieldLengths(newDegree, errors);
		ValidateDates(newDegree, errors);
		ValidateDegreeLevel(newDegree, errors);

		return errors.Count > 0
			? Result<Degree>.Failure(errors)
			: Result<Degree>.Success(newDegree);
	}

	public Result<Degree> ValidateUpdate(Degree? current, Degree? updated)
	{
		ResultErrors errors = [];

		if (current is null)
		{
			errors.AddError(nameof(Degree), "Formação acadêmica atual não encontrada.");
			return Result<Degree>.Failure(errors);
		}

		if (updated is null)
		{
			errors.AddError(nameof(Degree), "Dados atualizados são inválidos.");
			return Result<Degree>.Failure(errors);
		}

		if (current.Id != updated.Id)
		{
			errors.AddError(nameof(Degree.Id), "IDs não correspondem entre as formações.");
			return Result<Degree>.Failure(errors);
		}

		ValidateRequiredFields(updated, errors);
		ValidateFieldLengths(updated, errors);
		ValidateDates(updated, errors);
		ValidateDegreeLevel(updated, errors);

		return errors.Count > 0
			? Result<Degree>.Failure(errors)
			: Result<Degree>.Success(updated);
	}

	public Result<Degree> ValidateDeletion(Degree? targetDegree)
	{
		if (targetDegree is null || targetDegree.Id <= 0)
		{
			return Result<Degree>.Failure(nameof(Degree), "Formação acadêmica inválida para exclusão.");
		}

		return Result<Degree>.Success(targetDegree);
	}

	private static void ValidateRequiredFields(Degree degree, ResultErrors errors)
	{
		if (string.IsNullOrWhiteSpace(degree.Name))
			errors.AddError(nameof(Degree.Name), "O nome do curso é obrigatório.");

		if (string.IsNullOrWhiteSpace(degree.InstitutionName))
			errors.AddError(nameof(Degree.InstitutionName), "O nome da instituição é obrigatório.");

		if (string.IsNullOrWhiteSpace(degree.Description))
			errors.AddError(nameof(Degree.Description), "A descrição é obrigatória.");

		if (degree.StartDate == default)
			errors.AddError(nameof(Degree.StartDate), "A data de início é obrigatória.");
	}

	private static void ValidateFieldLengths(Degree degree, ResultErrors errors)
	{
		if (degree.Name?.Length > MaxNameLength)
			errors.AddError(nameof(Degree.Name), $"O nome não pode exceder {MaxNameLength} caracteres.");

		if (degree.InstitutionName?.Length > MaxInstitutionNameLength)
			errors.AddError(nameof(Degree.InstitutionName),
				$"O nome da instituição não pode exceder {MaxInstitutionNameLength} caracteres.");

		if (degree.Location?.Length > MaxLocationLength)
			errors.AddError(nameof(Degree.Location), $"A localização não pode exceder {MaxLocationLength} caracteres.");

		if (degree.Description?.Length > MaxDescriptionLength)
			errors.AddError(nameof(Degree.Description),
				$"A descrição não pode exceder {MaxDescriptionLength} caracteres.");

		if (degree.Highlights?.Length > MaxHighlightsLength)
			errors.AddError(nameof(Degree.Highlights),
				$"Os destaques não podem exceder {MaxHighlightsLength} caracteres.");
	}

	private static void ValidateDates(Degree degree, ResultErrors errors)
	{
		if (degree.EndDate.HasValue && degree.EndDate < degree.StartDate)
			errors.AddError(nameof(Degree.EndDate), "A data de conclusão não pode ser anterior à data de início.");

		if (degree.StartDate > DateTime.UtcNow.AddYears(1))
			errors.AddError(nameof(Degree.StartDate), "A data de início não pode ser superior a um ano no futuro.");

		if (degree.EndDate.HasValue && degree.EndDate > DateTime.UtcNow.AddYears(1))
			errors.AddError(nameof(Degree.EndDate), "A data de conclusão não pode ser superior a um ano no futuro.");
	}

	private static void ValidateDegreeLevel(Degree degree, ResultErrors errors)
	{
		if (!Enum.IsDefined(typeof(DegreeLevel), degree.Level))
			errors.AddError(nameof(Degree.Level), "Nível de formação inválido.");
	}
}