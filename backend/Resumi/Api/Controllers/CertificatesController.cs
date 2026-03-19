using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resumi.Api.Data.Models;
using Resumi.App.Data.Models;
using Resumi.App.Modules;
using Resumi.Infra.Auth;
using Resumi.Infra.Database.Context;
using Resumi.Infra.Data.Models;

namespace Resumi.Api.Controllers;

[ApiController]
[Route("api/resumes/{resumeId:int}/certificates")]
[Authorize]
public class CertificatesController : ControllerBase
{
    private readonly CertificatesModule _module;
    private readonly AppDbContext _dbContext;
    private readonly UserContext _userContext;

    public CertificatesController(CertificatesModule module, AppDbContext dbContext, UserContext userContext)
    {
        _module = module;
        _dbContext = dbContext;
        _userContext = userContext;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Result<CertificateModel>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result<CertificateModel>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(int resumeId, [FromBody] AddCertificateModel model)
    {
        if (model is null)
            return BadRequest(Result<CertificateModel>.Failure("model", "Request body is required."));

        if (model.ResumeId != resumeId)
            return BadRequest(Result<CertificateModel>.Failure("resumeId", "Resume ID in URL and body must match."));

        var resume = await _dbContext.Resumes.FindAsync(resumeId);
        if (resume is null)
            return NotFound(Result<CertificateModel>.Failure("resumeId", "Resume not found."));

        if (resume.UserId != _userContext.GetUserId())
            return Forbid();

        if (!Enum.TryParse<CertificateType>(model.Type, true, out var parsedType))
        {
            return BadRequest(Result<CertificateModel>.Failure("type", "Tipo de certificado inválido."));
        }

        var certificate = new Certificate
        {
            ResumeId = model.ResumeId,
            Name = model.Name,
            Description = model.Description,
            InstitutionName = model.InstitutionName,
            Location = model.Location,
            IsRemote = model.IsRemote,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            StillEngaged = model.StillEngaged,
            CredentialId = model.CredentialId,
            CredentialUrl = model.CredentialUrl,
            Type = parsedType,
        };

        var creationResult = await _module.Service.CreateAsync(certificate);

        if (!creationResult.Succeeded)
            return BadRequest(Result<CertificateModel>.Failure(creationResult.Errors));

        var dto = ToDto(creationResult.Data!);

        return CreatedAtAction(nameof(Read), new { resumeId, id = dto.Id }, Result<CertificateModel>.Success(dto));
    }

    [HttpGet]
    [ProducesResponseType(typeof(Result<IEnumerable<CertificateModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ReadAll(int resumeId)
    {
        var resume = await _dbContext.Resumes.FindAsync(resumeId);
        if (resume is null)
            return NotFound(Result<IEnumerable<CertificateModel>>.Failure("resumeId", "Resume not found."));

        if (resume.UserId != _userContext.GetUserId())
            return Forbid();

        var certificates = _dbContext.Certificates.Where(c => c.ResumeId == resumeId).ToList();
        var list = certificates.Select(ToDto);

        return Ok(Result<IEnumerable<CertificateModel>>.Success(list));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Result<CertificateModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Read(int resumeId, int id)
    {
        var resume = await _dbContext.Resumes.FindAsync(resumeId);
        if (resume is null)
            return NotFound(Result<CertificateModel>.Failure("resumeId", "Resume not found."));

        if (resume.UserId != _userContext.GetUserId())
            return Forbid();

        var fetched = await _module.Service.FindAsync(id);
        if (!fetched.Succeeded || fetched.Data is null)
            return NotFound(Result<CertificateModel>.Failure("id", "Certificate not found."));

        if (fetched.Data.ResumeId != resumeId)
            return NotFound(Result<CertificateModel>.Failure("id", "Certificate does not belong to this resume."));

        return Ok(Result<CertificateModel>.Success(ToDto(fetched.Data)));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Result<CertificateModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(int resumeId, int id, [FromBody] UpdateCertificateModel model)
    {
        if (model is null)
            return BadRequest(Result<CertificateModel>.Failure("model", "Request body is required."));

        if (id != model.Id)
            return BadRequest(Result<CertificateModel>.Failure("id", "ID in URL and body must match."));

        var resume = await _dbContext.Resumes.FindAsync(resumeId);
        if (resume is null)
            return NotFound(Result<CertificateModel>.Failure("resumeId", "Resume not found."));

        if (resume.UserId != _userContext.GetUserId())
            return Forbid();

        var currentResult = await _module.Service.FindAsync(id);
        if (!currentResult.Succeeded || currentResult.Data is null)
            return NotFound(Result<CertificateModel>.Failure("id", "Certificate not found."));

        if (currentResult.Data.ResumeId != resumeId)
            return NotFound(Result<CertificateModel>.Failure("id", "Certificate does not belong to this resume."));

        var current = currentResult.Data;

        if (!string.IsNullOrEmpty(model.Name)) current.Name = model.Name;
        if (!string.IsNullOrEmpty(model.Description)) current.Description = model.Description;
        if (!string.IsNullOrEmpty(model.InstitutionName)) current.InstitutionName = model.InstitutionName;
        if (model.Location != null) current.Location = model.Location;
        if (model.IsRemote.HasValue) current.IsRemote = model.IsRemote.Value;
        if (model.StartDate.HasValue) current.StartDate = model.StartDate.Value;
        if (model.EndDate.HasValue) current.EndDate = model.EndDate;
        if (model.StillEngaged.HasValue) current.StillEngaged = model.StillEngaged.Value;
        if (model.CredentialId != null) current.CredentialId = model.CredentialId;
        if (model.CredentialUrl != null) current.CredentialUrl = model.CredentialUrl;
        if (model.Type != null)
        {
            if (!Enum.TryParse<CertificateType>(model.Type, true, out var parsedType))
                return BadRequest(Result<CertificateModel>.Failure("type", "Tipo de certificado inválido."));

            current.Type = parsedType;
        }

        var updateResult = await _module.Service.UpdateAsync(current, current);
        if (!updateResult.Succeeded || updateResult.Data is null)
            return BadRequest(Result<CertificateModel>.Failure(updateResult.Errors));

        return Ok(Result<CertificateModel>.Success(ToDto(updateResult.Data)));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int resumeId, int id)
    {
        var resume = await _dbContext.Resumes.FindAsync(resumeId);
        if (resume is null)
            return NotFound(Result<bool>.Failure("resumeId", "Resume not found."));

        if (resume.UserId != _userContext.GetUserId())
            return Forbid();

        var currentResult = await _module.Service.FindAsync(id);
        if (!currentResult.Succeeded || currentResult.Data is null)
            return NotFound(Result<bool>.Failure("id", "Certificate not found."));

        if (currentResult.Data.ResumeId != resumeId)
            return NotFound(Result<bool>.Failure("id", "Certificate does not belong to this resume."));

        var deleteResult = await _module.Service.DeleteAsync(id);

        if (!deleteResult.Succeeded)
            return BadRequest(Result<bool>.Failure("id", "Could not delete certificate."));

        return NoContent();
    }

    private static CertificateModel ToDto(Certificate certificate)
    {
        return new CertificateModel
        {
            Id = certificate.Id,
            ResumeId = certificate.ResumeId,
            Name = certificate.Name,
            Description = certificate.Description,
            InstitutionName = certificate.InstitutionName,
            Location = certificate.Location,
            IsRemote = certificate.IsRemote,
            StartDate = certificate.StartDate,
            EndDate = certificate.EndDate,
            StillEngaged = certificate.StillEngaged,
            CredentialId = certificate.CredentialId,
            CredentialUrl = certificate.CredentialUrl,
            Type = certificate.Type.ToString(),
        };
    }
}
