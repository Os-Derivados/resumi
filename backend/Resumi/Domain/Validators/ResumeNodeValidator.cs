using Resumi.Domain.Models;
using Resumi.Domain.Validators.Interfaces;
using Resumi.Infra.Data.Models;

namespace Resumi.Domain.Validators;

public class ResumeNodeValidator : IDomainValidator<ResumeNode>
{
	private const int MaxNameLength = 128;
	private const int MaxDescriptionLength = 256;
	private const int MaxInstitutionNameLength = 128;

	public Result ValidateCreation(ResumeNode? newEntity)
	{
		if (newEntity is null)
		{
			return Result.Failure(nameof(ResumeNode), Entity.InvalidState);
		}

		ResultErrors errors = [];

		if (newEntity.Name.Length > MaxNameLength)
		{
			errors.AddError(nameof(ResumeNode.Name), ResumeNode.NameTooLong);
		}

		if (newEntity.Description.Length > MaxDescriptionLength)
		{
			errors.AddError(nameof(ResumeNode.Description), ResumeNode.DescriptionTooLong);
		}

		if (newEntity.InstitutionName.Length > MaxInstitutionNameLength)
		{
			errors.AddError(nameof(ResumeNode.InstitutionName), ResumeNode.InstitutionNameTooLong);
		}

		return errors.Count > 0
			? Result.Failure(errors)
			: Result.Success;
	}

	public Result ValidateDeletion(ResumeNode? targetEntity)
	{
		throw new NotImplementedException();
	}

	public Result ValidateUpdate(ResumeNode? current, ResumeNode? updated)
	{
		if (current is null)
		{
			return Result.Failure(nameof(ResumeNode), Entity.NotFound);
		}

		if (updated is null)
		{
			return Result.Failure(nameof(ResumeNode), Entity.InvalidState);
		}

		ResultErrors errors = [];

		if (current.Id != updated.Id)
		{
			errors.AddError(nameof(ResumeNode), Entity.UpdatePrimaryKeyMismatch);
		}

		if (ReferenceEquals(current, updated))
		{
			errors.AddError(nameof(ResumeNode), Entity.CannotUpdateFromSameEntity);
		}

		if (updated.Name.Length > MaxNameLength)
		{
			errors.AddError(nameof(ResumeNode.Name), ResumeNode.NameTooLong);
		}

		if (updated.Description.Length > MaxDescriptionLength)
		{
			errors.AddError(nameof(ResumeNode.Description), ResumeNode.DescriptionTooLong);
		}

		if (updated.InstitutionName.Length > MaxInstitutionNameLength)
		{
			errors.AddError(nameof(ResumeNode.InstitutionName), ResumeNode.InstitutionNameTooLong);
		}

		return errors.Count > 0
			? Result.Failure(errors)
			: Result.Success;
	}
}