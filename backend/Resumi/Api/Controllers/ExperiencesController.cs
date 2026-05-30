using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Resumi.Api.Data.Models;
using Resumi.App.Services;
using Resumi.Domain.Models;
using Resumi.Infra.Data.Mappers;
using Resumi.Infra.Data.Models;
using Resumi.Infra.Database.Context;

namespace Resumi.Api.Controllers;

[ApiController]
[Route("api/experiences")]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[ProducesResponseType<Result<ExperienceModel>>(StatusCodes.Status400BadRequest)]
[ProducesResponseType<Result<ExperienceModel>>(StatusCodes.Status422UnprocessableEntity)]
public class ExperiencesController(
    AppDbContext dbContext,
    ExperienceService service,
    ExperienceMapper mapper,
    ILogger<ExperiencesController> logger)
    : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<Result<ExperienceModel>>(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] AddExperienceModel model)
    {
        var newExperience = mapper.NewDomainModel(model);
        var creationResult = await service.CreateAsync(newExperience);

        return !creationResult.Succeeded
            ? BadRequest(creationResult)
            : Created($"api/experiences/{creationResult.Data.Id}", creationResult);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateExperienceModel model)
    {
        try
        {
            var target = await dbContext.Experiences.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

            if (target is null) return NotFound();

            var updated = mapper.UpdatedDomainModel(model, target);

            if (updated is null)
            {
                return BadRequest(Result<ExperienceModel>.Failure(nameof(UpdateExperienceModel),
                    Experience.InvalidState));
            }

            var updateResult = await service.UpdateAsync(target, updated);

            return !updateResult.Succeeded ? BadRequest(updateResult) : Ok(updateResult);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while updating experience with id {ExperienceId}: {Message}", id,
                ex.Message);

            return UnprocessableEntity();
        }
    }

    [HttpDelete("{id:int}")]
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