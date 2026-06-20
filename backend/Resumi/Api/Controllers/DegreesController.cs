using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Resumi.Api.Data.Models;
using Resumi.Api.Data.Requests;
using Resumi.App.Services;
using Resumi.Domain.Models;
using Resumi.Infra.Data.Mappers;
using Resumi.Infra.Data.Models;
using Resumi.Infra.Database.Context;

namespace Resumi.Api.Controllers;

[ApiController]
[Route("api/degrees")]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[ProducesResponseType<Result<DegreeModel>>(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
public class DegreesController(
	DegreeService service,
	DegreeMapper mapper,
	AppDbContext dbContext,
	ILogger<DegreesController> logger)
	: ControllerBase
{
	private const string Route = "api/degrees";

	[HttpPost]
	[ProducesResponseType<Result<DegreeModel>>(StatusCodes.Status201Created)]
	public async Task<IActionResult> Create([FromBody] AddDegreeModel model)
	{
		var newDegree = mapper.NewDomainModel(model);
		var creationResult = await service.CreateAsync(newDegree);

		if (!creationResult.Succeeded)
		{
			return BadRequest(creationResult);
		}

		return Created(uri: $"{Route}/{creationResult.Data.Id}", creationResult);
	}

	[HttpPut("{id:int}")]
	[ProducesResponseType(typeof(Result<DegreeModel>), StatusCodes.Status200OK)]
	public async Task<IActionResult> Update(int id,
		[FromBody] UpdateDegreeModel model)
	{
		try
		{
			if (model.Id != id)
			{
				return BadRequest(Result.Failure(nameof(DegreeModel), Entity.UpdatePrimaryKeyMismatch));
			}

			var target = await dbContext.AcademicDegrees.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);

			if (target is null)
			{
				return NotFound();
			}

			if (target.Id != model.Id)
			{
				return BadRequest(Result.Failure(nameof(DegreeModel), Entity.UpdatePrimaryKeyMismatch));
			}

			var updated = mapper.UpdatedDomainModel(model, target);
			var updateResult = await service.UpdateAsync(target, updated);

			return !updateResult.Succeeded ? BadRequest(updateResult) : Ok(updateResult);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "An error occurred while updating degree with id {DegreeId}: {Message}", id,
				ex.Message);

			return UnprocessableEntity();
		}
	}

	[HttpDelete("{id:int}")]
	[ProducesResponseType<Result>(StatusCodes.Status204NoContent)]
	public async Task<IActionResult> Delete(int id)
	{
		var removalResult = await service.DeleteAsync(id);

		if (!removalResult.Succeeded)
		{
			return BadRequest(removalResult);
		}

		return NoContent();
	}
}