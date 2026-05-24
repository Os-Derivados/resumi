using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Resumi.Api.Data.Models;
using Resumi.App.Services;
using Resumi.Domain.Models;
using Resumi.Infra.Auth;
using Resumi.Infra.Auth.Constants;
using Resumi.Infra.Data.Models;
using Resumi.Infra.Database.Context;
using Resumi.Infra.Parameters;

namespace Resumi.Api.Controllers;

[ApiController]
[Route("api/resumes")]
[Authorize]
public class ResumesController(
    UserContext userContext,
    ResumeService service,
    AppDbContext dbContext,
    ILogger<ResumesController> logger) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(Result<ResumeModel>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result<ResumeModel>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Result<ResumeModel>>> Create([Required] string title)
    {
        var userId = userContext.GetUserId();

        Resume newResume = new()
        {
            Title = title,
            UserId = userId,
            OwnerName = HttpContext.User.FindFirstValue(SessionConstants.UserNameClaim),
            Email = HttpContext.User.FindFirstValue(SessionConstants.EmailClaim),
            PhoneNumber = HttpContext.User.FindFirstValue(ClaimTypes.MobilePhone)
        };

        var result = await service.CreateAsync(newResume);

        if (!result.Succeeded)

        {
            return BadRequest(result);
        }

        return Created($"api/resumes/{result.Data.Id}", result);
    }

    [HttpPost("{id:int}")]
    public async Task<IActionResult> Read(int id)
    {
        var result = await service.FindAsync(id, ResumeProjectionMode.Full);

        if (!result.Succeeded) return BadRequest(result);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> ReadAll([Required] int userId, int skip = 0, int take = 20)
    {
        var result = await service.FindByUserAsync(userId, ResumeProjectionMode.Full, skip, take);

        if (!result.Succeeded) return BadRequest(result);

        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateResumeModel model)
    {
        try
        {
            var current = await dbContext.Resumes.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
            var updated = current?.ShallowCopy();

            if (current is not null && updated is not null)
            {
                updated.Title = model.Title ?? current.Title;
                updated.OwnerName = model.OwnerName ?? current.OwnerName;
                updated.Location = model.Location ?? current.Location;
                updated.Email = model.Email ?? current.Email;
                updated.PhoneNumber = model.PhoneNumber ?? current.PhoneNumber;
            }

            var result = await service.UpdateAsync(current, updated);

            if (!result.Succeeded) return BadRequest(result);

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update Resume: {Message}", ex.Message);

            return UnprocessableEntity();
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await service.DeleteAsync(id);

        if (!result.Succeeded) return BadRequest(result);

        return NoContent();
    }
}
