using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Resumi.Api.Data.Models;
using Resumi.Api.Data.Requests;
using Resumi.App.Services;
using Resumi.Domain.Models;
using Resumi.Infra.AuthZ;
using Resumi.Infra.Auth.Constants;
using Resumi.Infra.Data.Models;
using Resumi.Infra.Database.Context;
using Resumi.Infra.Data.Projections;

namespace Resumi.Api.Controllers;

[ApiController]
[Route("api/resumes")]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[ProducesResponseType<Result<ResumeModel>>(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
public class ResumesController(
	UserContext userContext,
	ResumeService service,
	AppDbContext dbContext,
	ILogger<ResumesController> logger) : ControllerBase
{
	private const string Route = "api/resumes";

	[HttpPost]
	[ProducesResponseType<Result<ResumeModel>>(StatusCodes.Status201Created)]
	public async Task<IActionResult> Create([Required] string title)
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

		return !result.Succeeded
			? BadRequest(result)
			: Created($"{Route}/{result.Data.Id}", result);
	}

	[HttpPost("{id:int}")]
	[ProducesResponseType<Result<ResumeModel>>(StatusCodes.Status200OK)]
	public async Task<IActionResult> Read(int id)
	{
		var readResult = await service.FindAsync(id, ResumeProjectionMode.Full);

		if (!readResult.Succeeded) return BadRequest(readResult);

		return Ok(readResult);
	}

	[HttpGet]
	[ProducesResponseType<Result<List<ResumeModel>>>(StatusCodes.Status200OK)]
	public async Task<IActionResult> ReadAll([Required] int userId, int skip = 0, int take = 20)
	{
		var findByUserResult = await service.FindByUserAsync(userId, ResumeProjectionMode.Basic, skip, take);

		return !findByUserResult.Succeeded ? BadRequest(findByUserResult) : Ok(findByUserResult);
	}

	[HttpPut("{id:int}")]
	[ProducesResponseType<Result<ResumeModel>>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Update(int id, [FromBody] UpdateResumeRequest model)
	{
		try
		{
			var current = await dbContext.Resumes.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);

			if (current is null) return NotFound();

			var updated = current.ShallowCopy();

			if (updated is null) return BadRequest(Result<ResumeModel>.Failure(nameof(Resume), Resume.InvalidState));

			updated.Title = model.Title ?? current.Title;
			updated.OwnerName = model.OwnerName ?? current.OwnerName;
			updated.Location = model.Location ?? current.Location;
			updated.Email = model.Email ?? current.Email;
			updated.PhoneNumber = model.PhoneNumber ?? current.PhoneNumber;

			var result = await service.UpdateAsync(current, updated);

			return !result.Succeeded ? BadRequest(result) : Ok(result);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to update Resume: {Message}", ex.Message);

			return UnprocessableEntity();
		}
	}

	[HttpDelete("{id:int}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<IActionResult> Delete(int id)
	{
		var result = await service.DeleteAsync(id);

		return !result.Succeeded ? BadRequest(result) : NoContent();
	}
}
