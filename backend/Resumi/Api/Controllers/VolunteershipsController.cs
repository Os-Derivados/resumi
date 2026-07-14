using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Resumi.Api.Data.Models;
using Resumi.Api.Data.Requests;
using Resumi.App.Services;
using Resumi.Domain.Exceptions;
using Resumi.Domain.Models;
using Resumi.Infra.Data.Mappers;
using Resumi.Infra.Data.Models;
using Resumi.Infra.Database.Context;

namespace Resumi.Api.Controllers;

[ApiController]
[Route("api/volunteerships")]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[ProducesResponseType<Result<VolunteershipModel>>(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
public class VolunteershipsController(
	VolunteershipManager service,
	VolunteershipMapper mapper,
	AppDbContext dbContext,
	ILogger<VolunteershipsController> logger) : ControllerBase
{
	private const string Route = "api/volunteerships";

	[HttpPost]
	[ProducesResponseType<Result<VolunteershipModel>>(StatusCodes.Status201Created)]
	public async Task<IActionResult> Create([FromBody] AddVolunteershipRequest model)
	{
		try
		{
			var newVolunteership = mapper.NewDomainModel(model);

			if (newVolunteership is null)
			{
				return BadRequest(
					Result<VolunteershipModel>.Failure(nameof(VolunteershipModel), Entity.InvalidState));
			}

			var creationResult = await service.CreateAsync(newVolunteership);

			return !creationResult.Succeeded
				? BadRequest(creationResult)
				: Created(uri: $"{Route}/{creationResult.Data.Id}", creationResult);
		}
		catch (StillEngagedException)
		{
			return BadRequest(Result<VolunteershipModel>.Failure(
				errorKey: nameof(VolunteershipModel),
				errorMessage: ResumeNode.InvalidEngagement)
			);
		}
	}

	[HttpPut("{id:int}")]
	[ProducesResponseType<Result<VolunteershipModel>>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Update(int id, [FromBody] UpdateVolunteershipRequest model)
	{
		try
		{
			var target = await dbContext.VolunteerExperiences.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);

			if (target is null)
			{
				return NotFound();
			}

			if (model.Id != id)
			{
				return BadRequest(Result.Failure(nameof(VolunteershipModel), Entity.UpdatePrimaryKeyMismatch));
			}

			var updated = mapper.UpdatedDomainModel(model, target);

			if (updated is null)
			{
				return BadRequest(
					Result<VolunteershipModel>.Failure(nameof(VolunteershipModel), Entity.InvalidState));
			}

			var updateResult = await service.UpdateAsync(target, updated);

			return !updateResult.Succeeded ? BadRequest(updateResult) : Ok(updateResult);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "An error occurred while updating volunteership with ID {VolunteershipId}: {Message}",
				id, ex.Message);

			return UnprocessableEntity();
		}
	}

	[HttpDelete("{id:int}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<IActionResult> Delete(int id)
	{
		var removalResult = await service.DeleteAsync(id);

		return !removalResult.Succeeded ? BadRequest(removalResult) : NoContent();
	}
}