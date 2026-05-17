using Resumi.Domain.Models;
using Resumi.Domain.Validators.Interfaces;
using Resumi.Infra.Data.Models;

namespace Resumi.Domain.Validators;

public class VolunteershipValidator : IDomainValidator<Volunteership>
{
	public Result<Volunteership> ValidateCreation(Volunteership? newVolunteership)
	{
		throw new NotImplementedException();
	}

	public Result<Volunteership> ValidateSearch(Volunteership? targetVolunteership)
	{
		throw new NotImplementedException();
	}

	public Result<Volunteership> ValidateUpdate(Volunteership? current, Volunteership? updated)
	{
		throw new NotImplementedException();
	}

	public Result<Volunteership> ValidateDeletion(Volunteership? targetVolunteership)
	{
		throw new NotImplementedException();
	}
}