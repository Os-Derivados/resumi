using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Resumi.Api.Data.Models;
using Resumi.Api.Data.Requests;
using Resumi.Infra.Database.Context;
using Resumi.Infra.Data.Models;
using Resumi.App.Services;
using Resumi.Infra.Data.Mappers;
using Resumi.Domain.Models;

namespace Resumi.Api.Controllers;

[ApiController]
[Route("api/certificates")]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[ProducesResponseType<Result<CertificateModel>>(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
public class CertificatesController(
	CertificateService service,
	CertificateMapper mapper,
	AppDbContext dbContext,
	ILogger<CertificatesController> logger)
	: ControllerBase
{
	private const string Route = "api/certificates";

	[HttpPost]
	[ProducesResponseType(typeof(Result<CertificateModel>), StatusCodes.Status201Created)]
	public async Task<IActionResult> Create([Required] [FromBody] AddCertificateRequest model)
	{
		var newCertificate = mapper.NewDomainModel(model);

		var creationResult = await service.CreateAsync(newCertificate);

		return !creationResult.Succeeded
			? BadRequest(creationResult)
			: Created(uri: $"{Route}/{creationResult.Data.Id}", creationResult);
	}


	[HttpPut("{id:int}")]
	[ProducesResponseType<Result<CertificateModel>>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Update(int id, [Required] [FromBody] UpdateCertificateRequest model)
	{
		try
		{
			if (id != model.Id)
			{
				return BadRequest(Result<CertificateModel>.Failure(nameof(CertificateModel),
					Entity.UpdatePrimaryKeyMismatch));
			}

			var target = await dbContext.Certificates.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

			if (target is null) return NotFound();

			var updated = mapper.UpdatedDomainModel(model, target);
			var updateResult = await service.UpdateAsync(target, updated);

			return !updateResult.Succeeded ? BadRequest(updateResult) : Ok(updateResult);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "An error occurred while updating certificate with ID {CertificateId}: {Message}", id,
				ex.Message);

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
