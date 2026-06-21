using Resumi.Domain.Models;
using Resumi.Domain.Validators.Interfaces;
using Resumi.Infra.Data.Models;

namespace Resumi.Domain.Validators;

public class VolunteershipValidator : IDomainValidator<Volunteership>
{
	public Result ValidateCreation(Volunteership? newVolunteership)
	{
		throw new NotImplementedException();
	}

	public Result ValidateUpdate(Volunteership? current, Volunteership? updated)
	{
		throw new NotImplementedException();
	}

	public Result ValidateDeletion(Volunteership? targetVolunteership)
	{
		throw new NotImplementedException();
	}
}